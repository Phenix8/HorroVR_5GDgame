using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class UIPierres : MonoBehaviour {

    public List<GameObject> _redstones = new List<GameObject>();
    public List<GameObject> _bluestones = new List<GameObject>();
    public List<GameObject> _greenstones = new List<GameObject>();
    public int _redcount, _bluecount, _greencount;
    public static int _stonecount;
    public GameObject _1red1, _2red1, _2red2, _3red1, _3red2, _3red3;
    public GameObject _1blue1, _2blue1, _2blue2, _3blue1, _3blue2, _3blue3;
    public GameObject _1gre1, _2gre1, _2gre2, _3gre1, _3gre2, _3gre3;
    public bool _display;
    public Hand interactableHand = null;


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "toHideStep3")
        {
            _redcount++;
        }
        else if (collision.gameObject.tag == "toHideStep6")
        {
            _bluecount++;
        }
        else if (collision.gameObject.tag == "toHideStep8")
        {
            _greencount++;
        }


        _stonecount = _greencount + _redcount + _bluecount;
        // déclenchement du 1er pallier
        if (_stonecount == 3 && FogManager.triggers.ContainsKey("toHideStep1"))
            FogManager.triggers["toHideStep1"] = true;
    }

    
	void Update () {


        print(interactableHand.controller);

        //_display = false;
        _stonecount = _greencount + _redcount + _bluecount;


        if (interactableHand != null && interactableHand.controller.GetHairTriggerDown())  //if(Input.GetKeyDown(KeyCode.Space))
        {
            if (_redcount == 1)
            {
               
                if (_redstones.Count == 0)
                {
                    _redstones.Add(_1red1);
                }
            }

            if (_redcount == 2)
            {
                
                if (_redstones.Count < 2)
                {
                    _redstones.Add(_2red1);
                    _redstones.Add(_2red2);
                }
            }

            if (_redcount == 3)
            {
               
                if (_redstones.Count < 3)
                {
                    _redstones.Add(_3red1);
                    _redstones.Add(_3red2);
                    _redstones.Add(_3red3);
                }
            }

            if (_bluecount == 1)
            {
               
                if (_bluestones.Count == 0)
                {
                    _bluestones.Add(_1blue1);
                }

            }

            if (_bluecount == 2)
            {
               
                if (_bluestones.Count < 2)
                {
                    _bluestones.Add(_2blue1);
                    _bluestones.Add(_2blue2);
                }
            }

            if (_bluecount == 3)
            {
                
                if (_bluestones.Count < 3)
                {
                    _bluestones.Add(_3blue1);
                    _bluestones.Add(_3blue2);
                    _bluestones.Add(_3blue3);
                }
            }

            if (_greencount == 1)
            {
                
                if (_greenstones.Count == 0)
                {
                    _greenstones.Add(_1gre1);
                }

            }

            if (_greencount == 2)
            {
                
                if (_greenstones.Count < 2)
                {
                    _greenstones.Add(_2gre1);
                    _greenstones.Add(_2gre2);
                }
            }

            if (_greencount == 3)
            {
                
                if (_greenstones.Count < 3)
                {
                    _greenstones.Add(_3gre1);
                    _greenstones.Add(_3gre2);
                    _greenstones.Add(_3gre3);
                }
            }

            foreach (GameObject g in _redstones)            
                g.SetActive(true);              
            
            foreach (GameObject g in _bluestones)            
                g.SetActive(true);            

            foreach (GameObject g in _greenstones)           
                g.SetActive(true);           
        }

        if (interactableHand != null && interactableHand.controller.GetHairTriggerUp())//(Input.GetKeyUp(KeyCode.Space))
            {
            foreach (GameObject g in _redstones)
                g.SetActive(false);           

            foreach (GameObject g in _bluestones)
                g.SetActive(false);            

            foreach (GameObject g in _greenstones)
                g.SetActive(false);
            _redstones = new List<GameObject>();
            _bluestones = new List<GameObject>();
            _greenstones = new List<GameObject>();
        }

       // else _display = false;

           /* if (_display)
            {
                foreach (GameObject g in _redstones)
                {
                    g.SetActive(true);
                }

                foreach (GameObject g in _bluestones)
                {
                    g.SetActive(true);
                }

                foreach (GameObject g in _greenstones)
                {
                    g.SetActive(true);
                }
            }
            
            else
        {
            foreach (GameObject g in _redstones)
            {
                g.SetActive(false);
            }

            foreach (GameObject g in _bluestones)
            {
                g.SetActive(false);
            }

            foreach (GameObject g in _greenstones)
            {
                g.SetActive(false);
            }
        }*/
        

           
    }
		
}


