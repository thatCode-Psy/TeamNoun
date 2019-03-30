using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropScript : MonoBehaviour
{

    public float spinSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 currentRot = transform.localEulerAngles;
        currentRot.z += spinSpeed;
        
        transform.localEulerAngles = currentRot;
    }
}
