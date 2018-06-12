using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class MovingCamera : MonoBehaviour
{

    public Transform playerTr;
    public Hand interactableHand = null;
    public Hand interactableHand2 = null;
    public float speed = 1.0f;
    public float playerHeight = 2.0f;

    private bool isMoving = false;


    private void Start()
    {
    }

    private void HandHoverUpdate(Hand hand)
    {

        //print(hand.GetStandardInteractionButton());
        //Trigger got pressed
        if (interactableHand == null && hand.controller.GetHairTriggerDown())
        {
            hand.AttachObject(gameObject, Hand.AttachmentFlags.ParentToHand | Hand.AttachmentFlags.DetachOthers | Hand.AttachmentFlags.DetachFromOtherHand);
            interactableHand = hand;

        }
    }

    void Update()
    {
        if (interactableHand != null && interactableHand.controller != null && interactableHand.controller.GetHairTriggerDown())
        {
            isMoving = !isMoving;
        }
        else if (interactableHand2 != null && interactableHand2.controller != null && interactableHand2.controller.GetHairTriggerDown())
        {
            isMoving = !isMoving;
        }

        if(CamelControler.leftStop && CamelControler.rightStop)
        {
            isMoving = false;
            CamelControler.leftStop = CamelControler.rightStop = false;
        }
        else if (isMoving)
        {
            Vector3 newPosition = playerTr.transform.position;
            newPosition += playerTr.forward * speed * Time.deltaTime;
            playerTr.transform.position = newPosition;
        }

        RaycastHit ray;

        if(Physics.Raycast(playerTr.position, Vector3.down, out ray))
        {
            playerTr.position = ray.point + Vector3.up * playerHeight;
        }
    }

}