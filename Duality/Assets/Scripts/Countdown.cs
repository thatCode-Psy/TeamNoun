using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Countdown : MonoBehaviour
{
    public Text countdownText;
    public float percentTimeLeftMaxRed;

    [SerializeField]
    private float countdownTimeMinutes;
    [SerializeField]
    private float countdownTimeSeconds;

    private float totalCountdownTime;
    private float currCountdownValue;
    
    // Start is called before the first frame update
    void Start()
    {
        totalCountdownTime = countdownTimeMinutes * 60 + countdownTimeSeconds;
        StartCoroutine(StartCountdown(totalCountdownTime));
        // print(countdownText.color);
    }

     public IEnumerator StartCountdown(float countdownValue = 10)
    {
        currCountdownValue = countdownValue;
        print("total Countdown time: " + totalCountdownTime);
        
        
        while (currCountdownValue > 0)
        {
            //lets say our total time is 100 and we want it to be red at 20% time remaining.
            // at the start, TCT == 100 and CCT == 100.
            // TCT * (1 - 0.2) == TCT * (0.8) == 100 * 0.8 == 80
            //at 20 seconds left, (TCT - CCT) == (100 - 20) == 80 and so:
            // (TCT - CCT) / (TCT * .8) == 80 / 80!!
            float redPercent = (totalCountdownTime - currCountdownValue) / (totalCountdownTime * (1 - percentTimeLeftMaxRed));
            if(redPercent > 1)
                redPercent = 1;
            print("red percent:" + redPercent);
            int seconds = (int)currCountdownValue % 60;
            countdownText.text = ((int)currCountdownValue/60 +":" + seconds.ToString("D2"));
            countdownText.color = new Color(1, 1 - redPercent, 1 - redPercent, 1);
            Debug.Log("Countdown: " + currCountdownValue);
            yield return new WaitForSeconds(1.0f);
            currCountdownValue--;
        }
        SceneManager.LoadScene("ThirdLevel");
    }
}


