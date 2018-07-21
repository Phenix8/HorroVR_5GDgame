using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPierres : MonoBehaviour {
    public List<GameObject> _redstones = new List<GameObject>();
    public List<GameObject> _bluestones = new List<GameObject>();
    public List<GameObject> _greenstones = new List<GameObject>();
    public int _redcount, _bluecount, _greencount;
    public GameObject _1red1, _2red1, _2red2, _3red1, _3red2, _3red3;
    public GameObject _1blue1, _2blue1, _2blue2, _3blue1, _3blue2, _3blue3;
    public GameObject _1gre1, _2gre1, _2gre2, _3gre1, _3gre2, _3gre3;
    public bool _display;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "redstone")
        {
            _redcount++;
        }

        if (collision.gameObject.tag == "bluestone")
        {
            _bluecount++;
        }

        if (collision.gameObject.tag == "greenstone")
        {
            _greencount++;
        }
    }
    // Use this for initialization

    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //_display = false;
        if (Input.GetKeyDown(KeyCode.Space))
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

        if (Input.GetKeyUp(KeyCode.Space))
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
        

            if(_redcount == 1)
            {
            _redstones.Clear();
            if (_redstones.Count == 0)

                {
                    _redstones.Add(_1red1);
                }
                
            }

            if (_redcount == 2)
            {
                _redstones.Clear();

                if (_redstones.Count < 2)
                {
                    _redstones.Add(_2red1);
                    _redstones.Add(_2red2);
                }
            }

            if (_redcount == 3)
            {
                _redstones.Clear();

                if (_redstones.Count < 3)
                {
                    _redstones.Add(_3red1);
                    _redstones.Add(_3red2);
                    _redstones.Add(_3red3);
                }
            }
        }
		
	}


