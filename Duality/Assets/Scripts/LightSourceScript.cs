using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSourceScript : MonoBehaviour {
    public float startAngle;
    public float endAngle;
    public int amountOfRayCasts;

    public bool isOn;
    GameObject litArea;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (isOn) {
            GenerateLightArea();
        }
	}

    void GenerateLightArea() {
        Vector3 raycastStart = transform.position;
        Vector3[] vertices = new Vector3[amountOfRayCasts + 2];
        vertices[0] = raycastStart;
        Vector2[] uvs = new Vector2[amountOfRayCasts + 2];
        
    }
}
