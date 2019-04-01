using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextBoxBehavior : MonoBehaviour
{
    Camera mainCam;
    public GameObject textBox;
    public GameObject parentPlayer;
    //int rightOrLeft = 0; //0 is right of character, 1 is left of character (no longer used)
    Vector3 originalPos;
    // Start is called before the first frame update
    void Start()
    {
        //Set this canvas' event camera to the main camera
        mainCam = Camera.main;
        this.GetComponent<Canvas>().worldCamera = mainCam;

        originalPos = textBox.GetComponent<RectTransform>().localPosition;

    }

    // Update is called once per frame
    void Update()
    {
        //4.12
        Vector3 testPos = mainCam.WorldToViewportPoint(parentPlayer.transform.position);
        //Vector3 edgeDistanceTester = mainCam.WorldToViewportPoint(new Vector3(4.12f, 0, 0));
        //If textbox would be off screen, move it to the other side
        if (testPos.x > 0.75f)
        {
            textBox.GetComponent<RectTransform>().localPosition = new Vector3(originalPos.x - 3.06f, originalPos.y, originalPos.z);
        }
        else
        {
            textBox.GetComponent<RectTransform>().localPosition = originalPos;
        }
    }
}
