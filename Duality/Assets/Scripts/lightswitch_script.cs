using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightswitch_script : MonoBehaviour
{
    public bool light_on = false;
    public List<GameObject> lights = new List<GameObject>();

    //Dialogue variables
    public GameObject dialogueManager;
    public int lineToTrigger = -1;//Keep this at -1 to have the light not trigger dialogue!
    bool hasTriggered = false;

    //contains the components of all the non-null lights attached to the lightswitch.
    private List<UpdatedLightSourceScript> lightComponents = new List<UpdatedLightSourceScript>();

    public AudioSource audio;

    public bool switchTriggering;
    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
        switchTriggering = false;
        for (int x = 0; x < lights.Count; x++)
        {
            if (lights[x] != null)
            {
                UpdatedLightSourceScript lsc = lights[x].GetComponent<UpdatedLightSourceScript>();
                lightComponents.Add(lsc);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //switchLight();
        if(switchTriggering){
            switchLight();
            switchTriggering = false;
        }
    }

    public void switchLight()
    {
        //flips the light model upside down so it looks like it gets switched on/off.
        transform.GetChild(0).RotateAround(transform.position, new Vector3(1.0f, 0.0f, 0.0f), 180);
        light_on = !light_on;
        audio.Play();
        
        print("triggerSwitch");
        //toggle lights
        for (int x = 0; x < lightComponents.Count; x++)
        {
            lightComponents[x].isOn = !lightComponents[x].isOn;
        }
        //Dialogue stuff
        if(lineToTrigger != -1 && !hasTriggered)
        {
            dialogueManager.SendMessage("PlayDialogue", lineToTrigger);
            hasTriggered = true;
        }
    }
}
