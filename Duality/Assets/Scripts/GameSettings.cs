﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour {

    public static GameSettings instance; // this is a singleton to store stuff between the menu and the gameplay
    public bool playMusic = true;
    // so sorry I'm not making this nice and using private variables but who's got the time?
    public InputManager p1InputManager = new InputManager(1, InputManager.ControllerType.Keyboard);
    public InputManager p2InputManager = new InputManager(1, InputManager.ControllerType.KeyboardTwo);

	// Use this for initialization
	void Awake () {
		if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
	}
}
