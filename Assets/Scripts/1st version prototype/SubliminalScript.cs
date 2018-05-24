using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubliminalScript : MonoBehaviour {
    
    public float apparitionDelay = 3.0f;
    
    private float lastAppearance;

    public SkinnedMeshRenderer meshRenderer;

    private Color visibleColor;
    private Color invisibleColor;


    private void Start()
    {
        meshRenderer.enabled = false;
        lastAppearance = Time.time;
    }

    void Update ()
    {
        if(Time.time - lastAppearance > apparitionDelay)
        {
            lastAppearance = Time.time;
            meshRenderer.enabled = true;
        }
    }


    void FixedUpdate()
    {
        if (meshRenderer.enabled)
            meshRenderer.enabled = false; 
        
    }


}
