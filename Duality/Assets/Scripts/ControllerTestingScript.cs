using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControllerTestingScript : MonoBehaviour {
    [SerializeField]
    private InputManager.ControllerType controllerType;
    [SerializeField]
    private int controllerNumber = 0;
    [SerializeField]
    private GameObject scrollbarPrefab; // this is used to display the input
    [SerializeField]
    private Transform scrollViewParent;

    InputManager inputManager;
    private List<Scrollbar> scrollbars = new List<Scrollbar>();

    // Use this for initialization
    void Start () {
        inputManager = new InputManager(controllerNumber, controllerType);
        for (int i = 0; i < inputManager.NumberOfAxes(); i++)
        {
            GameObject go = Instantiate(scrollbarPrefab, scrollViewParent);
            go.GetComponentInChildren<Text>().text = inputManager.AxisName((InputManager.ControllerAxis)i);
            scrollbars.Add(go.GetComponentInChildren<Scrollbar>());
        }
        /*for (int i = 0; i < scrollbars.Count; i++)
        {
            scrollbars[i].GetComponent<Text>().text = inputManager.AxisName((InputManager.ControllerAxis)i);
        }*/
	}

    void UpdateInputManager()
    {
        inputManager = new InputManager(controllerNumber, controllerType);
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.R))
        {
            UpdateInputManager();
        }
        inputManager.Update();
        for (int i = 0; i < scrollbars.Count; i++)
        {
            scrollbars[i].value = (1 + inputManager.GetAxis((InputManager.ControllerAxis)i)) / 2;
        }
	}
}
