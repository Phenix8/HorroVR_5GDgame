using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundTrigger : MonoBehaviour {

    public ColorfulFog fog;


    private float fogStartDensity = 0.00f;
    public float fogStrongDensity = 0.1f;   
    private float enableFogStartTime;
    public float fogDensityDelay = 1.0f;
    public float teleportDistance = 3.0f;

    private bool fogEnabled = false;

    public Transform playerTr;

    void Start ()
    {
        fogStartDensity = fog.fogDensity;
    }
	

	void Update ()
    {
        SetNewFogDensity();
    }


    private void SetNewFogDensity()
    {
        if (!fogEnabled && fog.fogDensity <= fogStartDensity)
            return;

        // intensification de la densité
        if (fogEnabled)
        {
            if (fog.fogDensity < fogStrongDensity)
                fog.fogDensity += fogStrongDensity / 100.0f;
            else
                fog.fogDensity = fogStrongDensity;
        }
        // réduction de la densité
        else
        {
            if (fog.fogDensity > fogStartDensity)
                fog.fogDensity -= fogStartDensity / 100.0f;
            else
                fog.fogDensity = fogStartDensity;
        }

        
        if (fogEnabled && Time.time > enableFogStartTime + fogDensityDelay)
        {
            print("Maximum fog density :" + fog.fogDensity);

            fogEnabled = false;
            Vector3 newPosition = playerTr.transform.position - playerTr.forward * teleportDistance;
            playerTr.transform.position = newPosition;

            Vector3 rotationVector = Vector3.up * 180.0f;
            playerTr.Rotate(rotationVector);
        }
    }


    #region Événements de trigger

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Hand")
        {          
            print("Enter bounds");
            fogEnabled = true;
            enableFogStartTime = Time.time;
        }
    }
    #endregion
}
