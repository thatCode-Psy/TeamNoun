using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowScript : MonoBehaviour
{

    public float lerpThreshold;
    public float cameraLookAhead;
    GameObject[] playerList;

    float minSizeY;
    float minSizeX;
   
    // Start is called before the first frame update
    void Start()
    {
        playerList = GameObject.FindGameObjectsWithTag("Player");
        minSizeY = GetComponent<Camera>().orthographicSize;
        minSizeX = minSizeY * Screen.width / Screen.height;
        
    }

    // Update is called once per frame
    void Update()
    {
        float averageX = 0;
        foreach(GameObject player in playerList){
            averageX += player.transform.position.x;
        }
        averageX /= playerList.Length;
        Vector3 currentPosition = transform.position;
        currentPosition.x = averageX;
        transform.position = Vector3.Lerp(transform.position, currentPosition, lerpThreshold);
        if(playerList.Length > 1){
            float currentSize = GetComponent<Camera>().orthographicSize;
            float currentDifference = Mathf.Abs(playerList[0].transform.position.x - playerList[1].transform.position.x) * 0.5f; 
            float newWidth = Mathf.Max(minSizeX, currentDifference + cameraLookAhead);
            Camera.main.orthographicSize = Mathf.Lerp(currentSize, Mathf.Max(newWidth * Screen.height/Screen.width, minSizeY), lerpThreshold);
        }
        

    }
}
