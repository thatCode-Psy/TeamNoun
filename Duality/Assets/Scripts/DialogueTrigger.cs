﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{

    public GameObject dialogueManager;
    public int dialogueNum;
    bool beenTriggered = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && !beenTriggered)
        {
            dialogueManager.SendMessage("PlayDialogue", dialogueNum);
            beenTriggered = true;
        }
    }
}
