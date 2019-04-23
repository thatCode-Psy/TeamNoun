using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TextBoxScaling : MonoBehaviour
{
    // Start is called before the first frame update
    Camera camera;
    public Text text;
    public float originalFOV;
    float originalTextSize;
    float originalScaleX;
    float originalScaleY;
    bool firstFrame = true;

    void Start()
    {
        //originalFOV = camera.fieldOfView;
        //originalTextSize = text.fontSize;
        originalScaleX = gameObject.GetComponent<RectTransform>().localScale.x;
        originalScaleY = gameObject.GetComponent<RectTransform>().localScale.y;
    }

    // Update is called once per frame
    void Update()
    {
        //if(firstFrame)
        //{
        //    originalFOV = camera.fieldOfView;
        //    firstFrame = false;
        //}
        //text.fontSize = (int)(originalTextSize * (camera.fieldOfView / originalFOV) * .2) + 96;
        gameObject.GetComponent<RectTransform>().localScale = new Vector3(originalScaleX * (camera.fieldOfView / originalFOV), originalScaleY * (camera.fieldOfView / originalFOV), 1);
        //text.gameObject.GetComponent<RectTransform>().localScale = new Vector3(.001f * (camera.fieldOfView / originalFOV), .001f * (camera.fieldOfView / originalFOV), 1);
    }

    public void GetFOV()
    {
        camera = Camera.main;
        originalFOV = camera.fieldOfView;
    }
}
