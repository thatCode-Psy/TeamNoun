using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriorityDialogueTrigger : MonoBehaviour
{
    public GameObject dialogueManager;
    public float dialogueNum;
    public float cameraSetting;
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
        float[] args = new float[2];
        args[0] = dialogueNum;
        args[1] = cameraSetting;
        if (collision.gameObject.tag == "Player" && !beenTriggered)
        {
            dialogueManager.SendMessage("PlayDialoguePriority", args);
            beenTriggered = true;
        }
    }
}
