using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogManager : MonoBehaviour {

    public ColorfulFog fog;
    private float fogStartDensity = 0.00f;
    private float fogStrongDensity = 0.02f;

    public int disappearStepsNb = 5;

    private bool enableFog = false;

    // Booléen de désactivation des objets par tag
    public static Dictionary<string, bool> triggers;


    void Start ()
    {
        fogStartDensity = fog.fogDensity;
        fogStrongDensity = fog.fogDensity * 2.0f;

        triggers = new Dictionary<string, bool>();
        for (int i = 1; i <= disappearStepsNb; i++)
            triggers.Add("toHideStep" + i, false);
    }
	


	void Update () {

        foreach (string tag in triggers.Keys)
        {
            if (triggers[tag])
                MakeDisapearObjects(tag);
        }

    }


    private void MakeDisapearObjects(string tag)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);

        if (objects.Length == 0)
        {
            print("No object found to make disapear.");
            return;
        }

        foreach (GameObject obj in objects)
            obj.SetActive(false);

        triggers[tag] = false;
    }


}
