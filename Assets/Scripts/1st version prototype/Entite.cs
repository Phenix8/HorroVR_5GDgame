using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entite : MonoBehaviour
{

    
    public Transform playerTransform;

    public float angleTolerance = 35.0f;


    void Update()
    {
        Vector3 posToLookAt = playerTransform.position;
        posToLookAt.y = transform.position.y;
        transform.LookAt(posToLookAt);

        Vector3 targetDir = transform.position - playerTransform.position;
        float angle = Vector3.Angle(targetDir, playerTransform.forward);

        print("Angle calculé : " +angle);


        if (angle < angleTolerance)
        {
            Vector3 newPosition = playerTransform.position;
            newPosition -= playerTransform.forward;
            this.transform.position = newPosition;
        }
               
        
    }    

}

