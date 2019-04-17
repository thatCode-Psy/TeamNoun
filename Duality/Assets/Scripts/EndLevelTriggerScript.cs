using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevelTriggerScript : MonoBehaviour
{

    //static List<EndLevelTriggerScript> endLevelTriggers = new List<EndLevelTriggerScript>();
    static bool runningCheckWin;
    public string nextLevel = "DemoVictory";
    public static int playerOnGoal = 0;

    bool triggering;
    // Start is called before the first frame update
    void Start()
    {
        playerOnGoal = 0;
        //endLevelTriggers.Add(this);
        triggering = false;
        runningCheckWin = false;
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collider){
        if(collider.tag == "Player"){
            playerOnGoal++;
            if(playerOnGoal == 2)
            {
                SceneManager.LoadScene(nextLevel);
            }
            //print("Players on goal "+playerOnGoal);
            //triggering = true;
            //CheckWin();

        }

    }

    void OnTriggerExit2D(Collider2D collider){
        if (collider.tag == "Player")
        {
            playerOnGoal--;
            
        }
    }

    /*void CheckWin(){
        if(!runningCheckWin){
            runningCheckWin = true;
            foreach(EndLevelTriggerScript script in endLevelTriggers){
                if(!script.triggering){
                    runningCheckWin = false;
                    return;
                }
            }
            SceneManager.LoadScene(nextLevel);
        }
    }*/

}
