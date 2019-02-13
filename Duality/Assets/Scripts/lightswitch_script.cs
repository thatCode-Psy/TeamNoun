﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightswitch_script : MonoBehaviour
{
    public bool light_on = false;
    public List<GameObject> lights = new List<GameObject>();

    //contains the components of all the non-null lights attached to the lightswitch.
    private List<LightSourceScript> lightComponents = new List<LightSourceScript>();

    // Start is called before the first frame update
    void Start()
    {
        for (int x = 0; x < lights.Count; x++)
        {
            if (lights[x] != null)
            {
                LightSourceScript lsc = lights[x].GetComponent<LightSourceScript>();
                lightComponents.Add(lsc);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //switchLight();
    }

    public void switchLight()
    {
        //flips the light model upside down so it looks like it gets switched on/off.
        transform.GetChild(0).RotateAround(transform.position, new Vector3(1.0f, 0.0f, 0.0f), 180);
        light_on = !light_on;

        //toggle lights
        for (int x = 0; x < lightComponents.Count; x++)
        {
            lightComponents[x].isOn = !lightComponents[x].isOn;
        }
    }
}