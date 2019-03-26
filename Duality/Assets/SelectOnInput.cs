using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


//only P1 can do menu stuff; can be easily changed if we don't want that
public class SelectOnInput : MonoBehaviour
{
    public EventSystem eventSystem;
    public GameObject selectedObject;
    private bool selected;

    InputManager inputManager;

    //since this is in the main menu we know gameSettings is active so we just do this
    void Start() {
        inputManager = GameSettings.instance.p1InputManager;
    }

    // Update is called once per frame
    void Update()
    {
        inputManager.Update();
        float moveHorizontal = inputManager.GetAxis(InputManager.ControllerAxis.HorizontalMovement);
        float moveVertical = inputManager.GetAxis(InputManager.ControllerAxis.VerticalMovement);
        print(moveHorizontal + " " + moveVertical);
        if( (moveHorizontal != 0 || moveVertical != 0) && selected == false ) {
            eventSystem.SetSelectedGameObject(selectedObject);
            selected = true;
        }
    }

    private void onDisable() {
        selected = false;
    }
}
