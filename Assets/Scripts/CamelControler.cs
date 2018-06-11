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
    private bool stop = false;

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


    void OnTriggerStay(Collider other)
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
        else if (other.tag == "InnerTrigger")
        {
            stop = true;
            print("Inner " + other.name);
        }
    }
}
