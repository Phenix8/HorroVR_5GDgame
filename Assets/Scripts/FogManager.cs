using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogManager : MonoBehaviour {

    public ColorfulFog fog;
    public float fogStartDensity = 0.00f;
    public float fogGoalDensity = 0.02f;

    private bool enableFog = false;

    void Start ()
    {
        fog.fogDensity = fogStartDensity;
    }
	


	void Update () {

        if (enableFog && fog.fogDensity < fogGoalDensity)
            fog.fogDensity += 0.001f;

    }


    private void OnTriggerEnter(Collider other)
    {
        
    }
}
