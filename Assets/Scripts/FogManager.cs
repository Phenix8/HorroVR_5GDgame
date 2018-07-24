using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogManager : MonoBehaviour {

    public ColorfulFog fog;
    private float fogStartDensity = 0.00f;
    public float fogStrongDensity = 0.1f;

    public int disappearStepsNb = 5;
    private int step = 0;

    private bool fogEnabled = false;
    private bool isFirstDisappearance = true;

    private float enableFogStartTime;
    public float fogDensityDelay = 1.0f;
    public float minDisapperanceDelay = 5.0f;
    public float maxDisapperanceDelay = 120.0f;

    // Booléen de désactivation des objets par tag
    public static Dictionary<string, bool> triggers;

    private List<string> tempTagsList;
    public float _timer = 0.0f;


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
        if (tempTagsList == null)
            return;

        
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
            if (isFirstDisappearance)
            {
                isFirstDisappearance = false;
                step = 1;
            }          
        }

        SetNewFogDensity();

        if (UIPierres._stonecount >= 3)
        {
            _timer += Time.deltaTime;
        }

        if(70 <_timer && _timer < 72)
        {
            step = 2;
            CheckDisapperanceStepsAfterFirst();
        }

        else if (125 < _timer && _timer < 127)
        {
            step = 3;
            CheckDisapperanceStepsAfterFirst();
        }

        else if (175 < _timer && _timer < 177)
        {
            step = 4;
            CheckDisapperanceStepsAfterFirst();
        }

        else if (225 < _timer && _timer < 227)
        {
            step = 5;
            CheckDisapperanceStepsAfterFirst();
        }

        else if (270 < _timer && _timer < 272)
        {
            step = 6;
            CheckDisapperanceStepsAfterFirst();
        }

        else if (305 < _timer && _timer < 307)
        {
            step = 7;
            CheckDisapperanceStepsAfterFirst();
        }

        else if (335 < _timer && _timer < 337)
        {
            step = 8;
            CheckDisapperanceStepsAfterFirst();
        }
    }
     

    private void CheckDisapperanceStepsAfterFirst()
    {
        if (isFirstDisappearance)
            return;
   
        // Activation d'un nouveau palier       
        
            // Disparition d'éléments graphiques
            if (step == 2 || step == 3 || step == 6 || step == 8) {
                triggers[string.Format("toHideStep{0}", step)] = true;

                print("Hiding objects with tag : " + string.Format("toHideStep{0}", step));
            }
            else if (step == 4)
            {
                fogEnabled = true;
                enableFogStartTime = Time.time;
                SoundManager._snakesoundtoplay = true;
                // disparition du son
            }
            else if (step == 5 || step == 7)
            {
            
                fogEnabled = true;
                enableFogStartTime = Time.time;
            // disparition des controleurs
            if (step == 5)
                    CamelControler.disableSpeedChange = true;

                if (step == 7)
                    CamelControler.disableRotationChange = true;
            }
        
    }

    private void MakeDisapearObjects()
    {
        if (tempTagsList.Count == 0)
            return;

        foreach(string tag in tempTagsList)
        { 
            GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);

            triggers[tag] = false;

            if (objects.Length == 0)           
                print("No object found to make disapear.");           
            else
                foreach (GameObject obj in objects)
                    obj.SetActive(false);
        }
    }


    private void SetNewFogDensity()
    {
        if (!fogEnabled && fog.fogDensity == fogStartDensity)
            return;

        //print("Setting new fog density");

        // intensification de la densité
        if (fogEnabled)
        {
            print("augmenting fog density");
            if (fog.fogDensity < fogStrongDensity)
                fog.fogDensity += fogStrongDensity / 100.0f;
            else
                fog.fogDensity = fogStrongDensity;
        }
        // réduction de la densité
        else
        {
            print("reducing fog density");
            if (fog.fogDensity > fogStartDensity)
                fog.fogDensity -= fogStartDensity / 100.0f;
            else
                fog.fogDensity = fogStartDensity;
        }
        
        if (fogEnabled && Time.time > enableFogStartTime + fogDensityDelay)
        {
            print("Fog has to reduce now");
            fogEnabled = false;
            MakeDisapearObjects();
        }
    }

}
