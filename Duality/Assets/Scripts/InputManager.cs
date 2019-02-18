using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * For this to work for a controller #, you need to set up the controller Axis as "joystick n axis 1" for axis 1, 2, 3, 4, 5, 6, 10
 * For this to work for the keyboard you need to set up Horizontal and Vertical for movement (wasd likely), and have "Mouse X" and "Mouse Y" for mouse controls
 * 
 * You have to update it each frame so it can keep track of things being pressed and released (the .Update() function)
 * You can query raw values of axes like horizontal or vertical using the GetAxis(InputManager.ControllerAxis axis) function, they'll be -1 to 1 except for the mouse, which is dx and dy (we may need to scale that)
 * You can query if a button is being held down with GetAxisPressed(InputManager.ControllerAxis axis). This only works for buttons like shoot, jump, back, or pause, not HorizontalMove
 * You can query if a button was just pressed or just released with GetAxisDown and GetAxisUp
 */

public class InputManager {

    private int controllerNumber;
    private ControllerType controllerType;
    private float[] inputValues;
    private int[] inputDown;
    private bool[] inputUp;

    private int i = (int)ControllerAxis.Back;

    [SerializeField]
    private string[] controllerInputStrings;

    private int numInputAxes;
	
	public InputManager(int controllerNum, ControllerType controllerType)
    {
        InitializeInputManager(controllerNum, controllerType);
    }

    public void InitializeInputManager(int controllerNum, ControllerType controller)
    {
        controllerNumber = controllerNum;
        controllerType = controller;

        numInputAxes = System.Enum.GetNames(typeof(ControllerAxis)).Length;

        inputValues = new float[numInputAxes];
        inputDown = new int[numInputAxes];

        // HorizontalMovement, VerticalMovement, HorizontalLook, VerticalLook, Shoot, Jump, Back, Pause
        if (controllerType == ControllerType.Xbox)
        {
            controllerInputStrings = new string[] { " axis 1", " axis 2", " axis 4", " axis 5", " axis 10", " button 0", " button 6", " button 7" };
            for (int i = 0; i < controllerInputStrings.Length; i++)
            {
                controllerInputStrings[i] = "joystick " + controllerNumber + controllerInputStrings[i]; // so that the correct controller is used
            }
        }
        else if (controllerType == ControllerType.Dualshock4)
        {
            controllerInputStrings = new string[] { " axis 1", " axis 2", " axis 3", " axis 6", " axis 5", " button 1", " button 8", " button 9" };
            for (int i = 0; i < controllerInputStrings.Length; i++)
            {
                controllerInputStrings[i] = "joystick " + controllerNumber + controllerInputStrings[i]; // so that the correct controller is used
            }
        }
    }

	// Update is called once per frame to deal with gathering the input and setting
	public void Update () {
        if (controllerType == ControllerType.Keyboard)
        {
            UpdateControllerValue(ControllerAxis.HorizontalMovement, Input.GetAxisRaw("Horizontal"));
            UpdateControllerValue(ControllerAxis.VerticalMovement, Input.GetAxisRaw("Vertical"));
            UpdateControllerValue(ControllerAxis.HorizontalLook, Input.GetAxisRaw("Mouse X"));
            UpdateControllerValue(ControllerAxis.VerticalLook, Input.GetAxisRaw("Mouse Y"));
            UpdateControllerValue(ControllerAxis.Shoot, Input.GetMouseButton(0) ? 1 : 0);
            UpdateControllerValue(ControllerAxis.Jump, Input.GetKey(KeyCode.Space) ? 1 : 0);
            UpdateControllerValue(ControllerAxis.Back, Input.GetKey(KeyCode.Escape) ? 1 : 0);
            UpdateControllerValue(ControllerAxis.Pause, Input.GetKey(KeyCode.P) ? 1 : 0);
        }
        else if (controllerType == ControllerType.Xbox)
        {
            UpdateControllerValue(ControllerAxis.HorizontalMovement, Input.GetAxis(controllerInputStrings[0]));
            UpdateControllerValue(ControllerAxis.VerticalMovement, -Input.GetAxis(controllerInputStrings[1]));
            UpdateControllerValue(ControllerAxis.HorizontalLook, Input.GetAxis(controllerInputStrings[2]));
            UpdateControllerValue(ControllerAxis.VerticalLook, -Input.GetAxis(controllerInputStrings[3]));
            UpdateControllerValue(ControllerAxis.Shoot, Input.GetAxisRaw(controllerInputStrings[4]) > 0.05f ? 1 : 0);
            UpdateControllerValue(ControllerAxis.Jump, Input.GetKey(controllerInputStrings[5]) ? 1 : 0);
            UpdateControllerValue(ControllerAxis.Back, Input.GetKey(controllerInputStrings[6]) ? 1 : 0);
            UpdateControllerValue(ControllerAxis.Pause, Input.GetKey(controllerInputStrings[7]) ? 1 : 0);
        }
        else if (controllerType == ControllerType.Dualshock4)
        {
            Debug.Log(Input.GetAxisRaw(controllerInputStrings[4]));
            UpdateControllerValue(ControllerAxis.HorizontalMovement, Input.GetAxisRaw(controllerInputStrings[0]));
            UpdateControllerValue(ControllerAxis.VerticalMovement, -Input.GetAxisRaw(controllerInputStrings[1])); // inverted this so that it's correct
            UpdateControllerValue(ControllerAxis.HorizontalLook, Input.GetAxisRaw(controllerInputStrings[2]));
            UpdateControllerValue(ControllerAxis.VerticalLook, -Input.GetAxisRaw(controllerInputStrings[3])); // inverted this so that it's "not inverted"
            UpdateControllerValue(ControllerAxis.Shoot, Input.GetAxisRaw(controllerInputStrings[4]) > -.95f ? 1 : 0); // this is needed to scale the trigger axis correctly for dualshocks
            UpdateControllerValue(ControllerAxis.Jump, Input.GetKey(controllerInputStrings[5]) ? 1 : 0);
            UpdateControllerValue(ControllerAxis.Back, Input.GetKey(controllerInputStrings[6]) ? 1 : 0);
            UpdateControllerValue(ControllerAxis.Pause, Input.GetKey(controllerInputStrings[7]) ? 1 : 0);
        }
    }

    void UpdateControllerValue(ControllerAxis axis, float newValue)
    {
        int i = (int)axis;
        float prev = inputValues[i];
        if (prev == 0 && newValue == 1)
        {
            inputDown[i] = 2; // pressed this frame
        }
        else if (prev == 1 && newValue == 0)
        {
            inputDown[i] = -2; // released this frame
        }
        else if (prev == 1 && newValue == 1)
        {
            inputDown[i] = 1; // pressed
        }
        else if (prev == 0 && newValue == 0)
        {
            inputDown[i] = -1; // not pressed
        }
        else
        {
            inputDown[i] = 0;
        }
        inputValues[i] = newValue;
    }

    public float GetAxis(ControllerAxis axis)
    {
        // this is used for full axes like horizontal and vertical movement and camera controls.
        return inputValues[(int)axis];
    }

    public bool GetAxisDown(ControllerAxis axis)
    {
        // NOTE THAT CURRENTLY THIS WILL ONLY WORK CORRECTLY WITH BUTTONS OR KEYS, NOT WITH JOYSTICK AXES LIKE SHOOT OR MOVE.
        return inputDown[(int)axis] == 2;
    }

    public bool GetAxisUp(ControllerAxis axis)
    {
        // NOTE THAT CURRENTLY THIS WILL ONLY WORK CORRECTLY WITH BUTTONS OR KEYS, NOT WITH JOYSTICK AXES LIKE SHOOT OR MOVE.
        return inputDown[(int)axis] == -2;
    }

    public bool GetAxisPressed(ControllerAxis axis)
    {
        // NOTE THAT CURRENTLY THIS WILL ONLY WORK CORRECTLY WITH BUTTONS OR KEYS, NOT WITH JOYSTICK AXES LIKE SHOOT OR MOVE.
        return inputDown[(int)axis] > 0;
    }

    public int NumberOfAxes()
    {
        return numInputAxes;
    }

    public int ControllerNumber()
    {
        return controllerNumber;
    }

    public string ControllerTypeName()
    {
        switch (controllerType) {
            case ControllerType.Keyboard:
                return "Keyboard";
            case ControllerType.Xbox:
                return "XBox Controller";
            case ControllerType.Dualshock4:
                return "Dualshock 4 Controller";
            default:
                return "This is not a valid controller type. Congratulations you broke the game";
        }
    }

    public ControllerType GetControllerType()
    {
        return controllerType;
    }

    public string AxisName(ControllerAxis axis)
    {
        switch (axis)
        {
            case ControllerAxis.HorizontalMovement:
                return "Horizontal Movement";
            case ControllerAxis.VerticalMovement:
                return "Vertical Movement";
            case ControllerAxis.HorizontalLook:
                return "Horizontal Look";
            case ControllerAxis.VerticalLook:
                return "Vertical Look";
            case ControllerAxis.Shoot:
                return "Shoot";
            case ControllerAxis.Jump:
                return "Jump";
            case ControllerAxis.Pause:
                return "Pause";
            case ControllerAxis.Back:
                return "Back";
            default:
                return "Not a valid axis oops";
        }
    }

    public enum ControllerType
    {
        Keyboard, Xbox, Dualshock4
    }

    public enum ControllerAxis
    {
        HorizontalMovement, VerticalMovement, HorizontalLook, VerticalLook, Shoot, Jump, Back, Pause
    }
}