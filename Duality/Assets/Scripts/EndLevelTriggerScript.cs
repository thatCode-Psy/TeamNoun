using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevelTriggerScript : MonoBehaviour
{

    static List<EndLevelTriggerScript> endLevelTriggers = new List<EndLevelTriggerScript>();

    bool triggering;
    // Start is called before the first frame update
    void Start()
    {
        endLevelTriggers.Add(this);
        triggering = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collider){
        if(collider.tag == "Player"){
            triggering = true;
            CheckWin();
        }

    }

    void OnTriggerExit2D(Collider2D collider){
        if(collider.tag == "Player")
            triggering = false;
    }

    void CheckWin(){
        foreach(EndLevelTriggerScript script in endLevelTriggers){
            if(!script.triggering){
                return;
            }
        }
        SceneManager.LoadScene("DemoVictory");
    }

}
