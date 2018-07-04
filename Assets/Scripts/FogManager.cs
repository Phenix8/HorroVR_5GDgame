using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogManager : MonoBehaviour {

    public ColorfulFog fog;
    private float fogStartDensity = 0.00f;
    public float fogStrongDensity = 0.1f;

    public int disappearStepsNb = 5;

    private bool fogEnabled = false;

    private float enableFogStartTime;
    public float fogDensityDelay = 1.0f;

    // Booléen de désactivation des objets par tag
    public static Dictionary<string, bool> triggers;

    private List<string> tempTagsList;


    void Start ()
    {
        fogStartDensity = fog.fogDensity;

        triggers = new Dictionary<string, bool>();
        for (int i = 1; i <= disappearStepsNb; i++)
            triggers.Add("toHideStep" + i, false);

        tempTagsList = new List<string>();
    }
	


	void Update ()
    {
        tempTagsList.Clear();

        foreach (string tag in triggers.Keys)       
            if (triggers[tag])
                tempTagsList.Add(tag);
        

        // Lancement de l'intensification du brouillard
        if (tempTagsList.Count != 0 && !fogEnabled)
        {
            print("fogEnabled set to true");
            fogEnabled = true;
            enableFogStartTime = Time.time;
        }

        SetNewFogDensity(tempTagsList);
    }


    private void MakeDisapearObjects(List<string> tags)
    {
        if (tags.Count == 0)
            return;

        foreach(string tag in tags)
        { 
            GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);

            if (objects.Length == 0)           
                print("No object found to make disapear.");           
            else
                foreach (GameObject obj in objects)
                    obj.SetActive(false);

            triggers[tag] = false;
        }
    }


    private void SetNewFogDensity(List<string> tags)
    {
        if (!fogEnabled && fog.fogDensity == fogStartDensity)
            return;

        //print("Setting new fog density");

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

        print(Time.time + "  " + (enableFogStartTime + fogDensityDelay));

        if (fogEnabled && Time.time > enableFogStartTime + fogDensityDelay)
        {
            fogEnabled = false;
            MakeDisapearObjects(tempTagsList);
        }
    }

}
