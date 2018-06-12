using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class CamelControler : MonoBehaviour {

    public Transform playerTr;
    public Hand interactableHand = null;

    public float rotationTolerance = 0.1f;

    public bool isLeft;
    public static bool leftStop = false;
    public static bool rightStop = false;

    void Start () {

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

        //print(interactableHand.controller.transform.pos);

    }


    private void OnTriggerStay(Collider other)
    {
        if (isLeft && other.tag == "LeftTrigger")
        {
            print("Turning to left");
            Vector3 rotationVector = Vector3.up * rotationTolerance * -1;
            playerTr.Rotate(rotationVector);
        }
        else if (!isLeft && other.tag == "RightTrigger")
        {
            print("Turning to right");
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
