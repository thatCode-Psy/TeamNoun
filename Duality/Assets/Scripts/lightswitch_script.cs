using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightswitch_script : MonoBehaviour
{
    public bool light_on = false;
    // Start is called before the first frame update
    void Start()
    {
        if (light_on)
        {
            transform.RotateAround(transform.position, new Vector3(1.0f, 0.0f, 0.0f), 180);
            //transform.up *= -1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        switchLight();
    }

    void switchLight()
    {
        transform.RotateAround(transform.position, new Vector3(1.0f, 0.0f, 0.0f), 180);
        //transform.up *= -1;
        light_on = !light_on;
    }
}
