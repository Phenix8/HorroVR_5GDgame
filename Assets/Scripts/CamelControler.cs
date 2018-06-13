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
    public float speed = 4.0f;
    public float playerHeight = 3.0f;
    public float yAmplitureTolerance = 1.0f;

    public bool isLeft;
    private static bool isMoving = false;
    private static bool leftAcceleration = false;
    private static bool rightAcceleration = false;
    public static bool leftStop = false;
    public static bool rightStop = false;

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

        CheckMovement();

        // Régle la distance au sol du personnage
        RaycastHit ray;
        if (Physics.Raycast(playerTr.position, Vector3.down, out ray))
            playerTr.position = ray.point + Vector3.up * playerHeight;      
    }

    private void CheckMovement()
    {
        if (leftStop && rightStop)
        {
            print("Stopping movement");
            isMoving = false;
            leftStop = rightStop = leftAcceleration = rightAcceleration = false;
            return;
        }

        if (isMoving)
        {
            Vector3 newPosition = playerTr.transform.position;
            newPosition += playerTr.forward * speed * Time.deltaTime;
            playerTr.transform.position = newPosition;
        }
    }


    // Vérifie que le mouvement "vers le haut puis redescente" 
    // est détecté pour l'accélération du chameau
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

        print("isMoving = true");
        return true;
    }


    private void OnTriggerStay(Collider other)
    {
        if (isLeft && other.tag == "LeftTrigger")
        {
            Vector3 rotationVector = Vector3.up * rotationTolerance * -1;
            playerTr.Rotate(rotationVector);
        }
        else if (!isLeft && other.tag == "RightTrigger")
        {
            Vector3 rotationVector = Vector3.up * rotationTolerance;
            playerTr.Rotate(rotationVector);
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
    }
}
