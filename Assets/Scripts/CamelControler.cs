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
    private static float currentSpeed = 0.0f;
    private static float rotationSpeed = 0.0f;
    public float smoothRatio = 300.0f;
    
    public bool isLeft;
    private static bool isInDecceleration = false;
    private static bool isMoving = false;
    private static bool leftAcceleration = false;
    private static bool rightAcceleration = false;
    private static bool rotateToLeft = false;
    private static bool rotateToRight = false;
    private static bool leftStop = false;
    private static bool rightStop = false;
    private static bool rotationLeftSlow = false;
    private static bool rotationRightSlow = false;

    private List<float> yPositions;


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

        // Régle la distance au sol du personnage
        RaycastHit ray;
        if (Physics.Raycast(playerTr.position, Vector3.down, out ray))
            playerTr.position = ray.point + Vector3.up * playerHeight;      
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
            // print("Déccélération : " + currentSpeed);
            if (currentSpeed <= 0.0f)
            {
                currentSpeed = 0.0f;
                isInDecceleration = false;
                isMoving = false;
                return;
            }
            Vector3 newPosition = playerTr.transform.position;
            newPosition += playerTr.forward * currentSpeed * Time.deltaTime;
            playerTr.transform.position = newPosition;
        }
        else if(isMoving)
        {
            currentSpeed += (currentSpeed < maxSpeed) ? (maxSpeed / smoothRatio) : 0.0f;
            // print(currentSpeed);
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
        if (!isMoving)
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
            if (yPosArray[i] < yPosArray[i-1] - 0.05f)
            {
                //print(yPosArray[i] + " < " + yPosArray[i - 1]);
                return false;
            }
        }

        // Vérification des position en phase descendante
        for (int i = maxIndex; i < yPosArray.Count(); i++)
        {
            if (yPosArray[i] > yPosArray[i - 1] + 0.05f)
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
            rotationSpeed += (rotationSpeed < rotationTolerance) ? (rotationTolerance / smoothRatio) : 0.0f;
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
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "InnerTrigger")
        {
            if (isLeft)
                leftStop = true;
            else
                rightStop = true;
            //print("Inner enter " + other.name);
        }
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
