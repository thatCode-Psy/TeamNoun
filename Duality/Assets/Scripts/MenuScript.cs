using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour {
    private int p1ControllerNumber = 1;
    private int p1ControllerType = (int)InputManager.ControllerType.Xbox; // keyboard
    private string p1ControllerName = "XBox Controller";
    private int p2ControllerNumber = 1;
    private int p2ControllerType = (int)InputManager.ControllerType.Keyboard; // Xbox
    private string p2ControllerName = "Keyboard";


    public Text p1NameText;
    public Text p2NameText;
    public Text p1NumberText;
    public Text p2NumberText;

    public void Start()
    {
        // show the mouse and unlock it so you can click on things
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (GameSettings.instance != null)
        {
            p1ControllerNumber = GameSettings.instance.p1InputManager.ControllerNumber();
            p2ControllerNumber = GameSettings.instance.p2InputManager.ControllerNumber();
            p1ControllerType = (int)GameSettings.instance.p1InputManager.GetControllerType();
            p2ControllerType = (int)GameSettings.instance.p2InputManager.GetControllerType();
            p1ControllerName = GameSettings.instance.p1InputManager.ControllerTypeName();
            p2ControllerName = GameSettings.instance.p2InputManager.ControllerTypeName();
        }
        SetButtonText();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitGame();
        }
    }

    public void ToggleControllerNumber(int playerNum)
    {
        switch (playerNum)
        {
            case 1:
                p1ControllerNumber += 1;
                if (p1ControllerNumber == 3)
                    p1ControllerNumber = 1;
                GameSettings.instance.p1InputManager.InitializeInputManager(p1ControllerNumber, (InputManager.ControllerType)p1ControllerType);
                p1ControllerName = GameSettings.instance.p1InputManager.ControllerTypeName();
                break;
            case 2:
                p2ControllerNumber += 1;
                if (p2ControllerNumber == 3)
                    p2ControllerNumber = 1;
                GameSettings.instance.p2InputManager.InitializeInputManager(p2ControllerNumber, (InputManager.ControllerType)p2ControllerType);
                p2ControllerName = GameSettings.instance.p2InputManager.ControllerTypeName();
                break;
            default:
                Debug.Log("Invalid controller number entered for toggle contrller number");
                break;
        }
        SetButtonText();
    }

    public void ToggleControllerType(int playerNum)
    {
        switch (playerNum)
        {
            case 1:
                p1ControllerType += 1;
                p1ControllerType %= 3;
                GameSettings.instance.p1InputManager.InitializeInputManager(p1ControllerNumber, (InputManager.ControllerType)p1ControllerType);
                p1ControllerName = GameSettings.instance.p1InputManager.ControllerTypeName();
                break;
            case 2:
                p2ControllerType += 1;
                p2ControllerType %= 3;
                GameSettings.instance.p2InputManager.InitializeInputManager(p2ControllerNumber, (InputManager.ControllerType)p2ControllerType);
                p2ControllerName = GameSettings.instance.p2InputManager.ControllerTypeName();
                break;
            default:
                Debug.Log("Invalid controller number entered for toggle contrller type");
                break;
        }
        SetButtonText();
    }

    private void SetButtonText()
    {
        p1NameText.text = p1ControllerName;
        p2NameText.text = p2ControllerName;
        p1NumberText.text = "Controller #" + p1ControllerNumber;
        p2NumberText.text = "Controller #" + p2ControllerNumber;
    }

    public void LoadLevel()
    {
        GameSettings.instance.p1InputManager.InitializeInputManager(p1ControllerNumber, (InputManager.ControllerType)p1ControllerType);
        GameSettings.instance.p2InputManager.InitializeInputManager(p2ControllerNumber, (InputManager.ControllerType)p2ControllerType);
        SceneManager.LoadScene("Level");
    }
}
