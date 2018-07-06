using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderManager : MonoBehaviour {


    public Transform playerTr;

    private bool hasToReturn = false;

    public ColorfulFog fog;

    private float fogStartDensity = 0.00f;
    public float fogStrongDensity = 0.1f;

    private float enableFogStartTime;
    public float fogDensityDelay = 1.0f;

    void Start ()
    {
        fogStartDensity = fog.fogDensity;
    }
	

	void Update ()
    {
        if (hasToReturn)
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

        if (hasToReturn && Time.time > enableFogStartTime + fogDensityDelay)
        {
            hasToReturn = false;
            playerTr.Rotate(playerTr.forward * -1.0f);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            hasToReturn = true;
        }
    }

}
