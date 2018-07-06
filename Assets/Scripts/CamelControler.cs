using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using System.Linq;

public class CamelControler : MonoBehaviour {

    public Transform playerTr;
    private Hand interactableHand = null;

    public float rotationTolerance = 0.1f;
    public float maxSpeed = 4.0f;
    public float playerHeight = 3.0f;
    public float yAmplitureTolerance = 1.0f;
    public float smoothRatio = 300.0f;
    private static float vibrationStartTime = -0.6f;
    private static float currentSpeed = 0.0f;
    private static float rotationSpeed = 0.0f;
    
    public bool isLeft;
    private static bool isInDecceleration = false;
    private static bool isMoving = false;
    private static bool isInFasterAcceleration = false;
    private static bool leftAcceleration = false, rightAcceleration = false;
    private static bool rotateToLeft = false, rotateToRight = false;
    private static bool leftStop = false, rightStop = false;
    private static bool rotationLeftSlow = false, rotationRightSlow = false;

    private List<float> yPositions;

    #region Méthodes

    void Start () {
        yPositions = new List<float>();
        interactableHand = this.GetComponent<Hand>();
	}


    private void HandHoverUpdate(Hand hand)
    {
        //Trigger got pressed
        if (interactableHand == null && hand.controller.GetHairTriggerDown())
        {
            hand.AttachObject(gameObject, Hand.AttachmentFlags.ParentToHand | Hand.AttachmentFlags.DetachOthers | Hand.AttachmentFlags.DetachFromOtherHand);
            interactableHand = hand;
        }
    }


    void Update () {
        
        CheckAcceleration();       

        CheckMovement();

        CheckRotation();

        if (vibrationStartTime + 0.5f > Time.time && interactableHand != null)
            SteamVR_Controller.Input((int)interactableHand.controller.index).TriggerHapticPulse(2000);
        
        SetHeightAndOrientation();
    }

    /// <summary>
    /// Vérifie si le mouvement doit être activé (après une détection d'accélération)
    /// et déplace le personnage le cas échéant
    /// </summary>
    private void CheckMovement()
    {
        if (leftStop && rightStop)
        {
            isInDecceleration = true;
            leftStop = rightStop = leftAcceleration = rightAcceleration = false;
            return;
        }

        if (isInDecceleration)
        {
            currentSpeed -= (maxSpeed / 115.0f);


            // Arrêt de la seconde accélération
            if (currentSpeed <= 0.0f)
            {
                currentSpeed = 0.0f;
                isInDecceleration = false;
                isMoving = false;
                return;
            }
            else if (isInFasterAcceleration && currentSpeed < maxSpeed)
            {
                //print("stopping faster acceleration");
                currentSpeed = maxSpeed;
                isInDecceleration = false;
                isInFasterAcceleration = false;
                return;
            }

            vibrationStartTime = Time.time;

            Vector3 newPosition = playerTr.transform.position;
            newPosition += playerTr.forward * currentSpeed * Time.deltaTime;
            playerTr.transform.position = newPosition;
        }

        else if(isMoving)
        {
            float tempMaxSpeed = (isInFasterAcceleration) ? maxSpeed * 2.0f : maxSpeed;
            if(currentSpeed < tempMaxSpeed)
                currentSpeed += (tempMaxSpeed / smoothRatio);
                             
            //print(currentSpeed);

            Vector3 newPosition = playerTr.transform.position;
            newPosition += playerTr.forward * currentSpeed * Time.deltaTime;
            playerTr.transform.position = newPosition;
        }
        
    }

    
    /// <summary>
    /// Vérifie s'il faut activer l'accélération pour la manette courante
    /// </summary>
    private void CheckAcceleration()
    {
        if (isInDecceleration)
            return;

        if (!isMoving || (isMoving && !isInFasterAcceleration))
        {
            // Pour ne pas surcharger la liste + pour détecter le mouvement uniquement avec 100 valeurs
            yPositions.Add(interactableHand.transform.position.y);
            if (yPositions.Count() > 100)
            {
                yPositions.RemoveAt(0);

                if (isLeft)
                    leftAcceleration = HasToEnableAcceleration();
                else
                    rightAcceleration = HasToEnableAcceleration();

                if (leftAcceleration && rightAcceleration)
                {
                    if (isMoving && !isInFasterAcceleration)
                    {
                        //print("isInFasterAcceleration = true");
                        isInFasterAcceleration = true;
                    }
                    vibrationStartTime = Time.time;
                    leftAcceleration = rightAcceleration = false;
                    yPositions.Clear();
                    isMoving = true;
                }
            }
        }
    }


    /// <summary>
    /// Vérifie que le mouvement "vers le haut puis redescente" 
    /// est détecté pour l'accélération du chameau
    /// </summary>
    /// <returns></returns>
    private bool HasToEnableAcceleration()
    {
        // Vérification sur l'amplitude de hauteur     
        if (yPositions.Max() < (yPositions.Min() + yAmplitureTolerance))
            return false;        

        int maxIndex = yPositions.IndexOf(yPositions.Max());
        float[] yPosArray = yPositions.ToArray();
        
        if (maxIndex == 0 || maxIndex == yPosArray.Count() - 1)
        {
            //print("Doesn't moving because maxIndex = " + maxIndex);
            return false;
        }

        // Vérification des position en phase ascendante
        for (int i=1; i< maxIndex; i++)
        {
            if (yPosArray[i] < yPosArray[i-1] - 0.02f)
            {
                //print(yPosArray[i] + " < " + yPosArray[i - 1]);
                return false;
            }
        }

        // Vérification des position en phase descendante
        for (int i = maxIndex; i < yPosArray.Count(); i++)
        {
            if (yPosArray[i] > yPosArray[i - 1] + 0.02f)
            {
                //print(yPosArray[i] + " > " + yPosArray[i - 1]);
                return false;
            }
        }

        // print("isMoving = true");
        return true;
    }


    /// <summary>
    /// Vérifie s'il faut effectuer uen rotation smooth
    /// </summary>
    private void CheckRotation()
    {
        if (rotateToLeft && rotateToRight)
            return;

        if (rotateToLeft || rotateToRight)
        {
            // Vitesse de rotation dépendant de la vitesse de déplacement
            float tempRotationTolerance = isInFasterAcceleration ? rotationTolerance : ( isMoving ? rotationTolerance * 2.0f : rotationTolerance * 3.0f);
            rotationSpeed += (rotationSpeed < tempRotationTolerance) ? (tempRotationTolerance / smoothRatio) : 0.0f;
            Vector3 rotationVector = Vector3.up * rotationSpeed;
            rotationVector *= (rotateToLeft) ? -1 : 1;
            playerTr.Rotate(rotationVector);
        }
        else if (rotationLeftSlow || rotationRightSlow)
        {
            rotationSpeed -= (rotationTolerance / 80.0f);
            if (rotationSpeed <= 0.0f)
            {
                rotationSpeed = 0.0f;
                rotationLeftSlow = rotationRightSlow = false;
                return;
            }
            Vector3 rotationVector = Vector3.up * rotationSpeed;
            rotationVector *= (rotationLeftSlow) ? -1 : 1;
            playerTr.Rotate(rotationVector);
        }
    }


    //private void ManageControlerVibrations()
    //{
    //    if (vibrationStartTime + 0.6f < Time.time)
    //        CancelInvoke("ManageControlerVibrations");
    //    SteamVR_Controller.Input((int)interactableHand.controller.index).TriggerHapticPulse(2000);
    //}

    /// <summary>
    /// Règle la distance au sol du personnage et son orientation
    /// </summary>
    private void SetHeightAndOrientation()
    {

        RaycastHit ray;
        if (Physics.Raycast(playerTr.position, Vector3.down, out ray))
        { 
            playerTr.position = ray.point + Vector3.up * playerHeight;
            playerTr.up = Vector3.RotateTowards(playerTr.up, ray.normal, Mathf.PI * Time.deltaTime / 30, 1);
        }
    }

    #endregion

    #region Événements de trigger

    private void OnTriggerStay(Collider other)
    {
        if (rotationLeftSlow || rotationRightSlow)
            return;

        if (isLeft && other.tag == "LeftTrigger")
        {
            rotateToLeft = true;
        }
        else if (!isLeft && other.tag == "RightTrigger")
        {
            rotateToRight = true;
        }
        else if (other.tag == "InnerTrigger")           // TEST POUR MAINTENIR LA DECCELERATION DE TRES RAPIDE JUSQU'A L'ARRET
        {
            if (isLeft)
                leftStop = true;
            else
                rightStop = true;
            //print("Inner enter " + other.name);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (other.tag == "InnerTrigger")
        //{
        //    if (isLeft)
        //        leftStop = true;
        //    else
        //        rightStop = true;
        //    //print("Inner enter " + other.name);
        //}
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "InnerTrigger")
        {
            if (isLeft)
                leftStop = false;
            else
                rightStop = false;
            //print("Inner exit " + other.name);
        }
        else if (isLeft && other.tag == "LeftTrigger")
        {
            rotateToLeft = false;
            rotationLeftSlow = true;
        }
        else if (!isLeft && other.tag == "RightTrigger")
        {
            rotateToRight = false;
            rotationRightSlow = true;
        }
    }

    #endregion

}


