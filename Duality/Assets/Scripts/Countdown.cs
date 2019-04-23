using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Countdown : MonoBehaviour
{
    public Text countdownText;

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
    }

     public IEnumerator StartCountdown(float countdownValue = 10)
    {
        currCountdownValue = countdownValue;
        
        while (currCountdownValue > 0)
        {
            int seconds = (int)currCountdownValue % 60;
            countdownText.text = ((int)currCountdownValue/60 +":" + seconds.ToString("D2"));
            Debug.Log("Countdown: " + currCountdownValue);
            yield return new WaitForSeconds(1.0f);
            currCountdownValue--;
        }
        SceneManager.LoadScene("ThirdLevel");
    }
}


