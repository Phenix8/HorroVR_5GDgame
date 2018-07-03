using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisapearTrigger : MonoBehaviour
{

    public string tagToEnable;


    void Start()
    {

    }



    void Update()
    {

    }


    private void OnTriggerEnter(Collider other)
    {
        if (FogManager.triggers.ContainsKey(tagToEnable))
            FogManager.triggers[tagToEnable] = true;
    }


}
