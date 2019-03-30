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
        float averageY = 0;
        foreach(GameObject player in playerList){
            averageX += player.transform.position.x;
            averageY += player.transform.position.y;
        }
        averageX /= playerList.Length;
        averageY /= playerList.Length;
        Vector3 currentPosition = transform.position;
        currentPosition.x = averageX;
        currentPosition.y = averageY;
        transform.position = Vector3.Lerp(transform.position, currentPosition, lerpThreshold);
        if(playerList.Length > 1){
            float currentSize = GetComponent<Camera>().orthographicSize;
            float currentDifference = Mathf.Abs(playerList[0].transform.position.x - playerList[1].transform.position.x) * 0.5f;
            float currentDifferenceHeight = Mathf.Abs(playerList[0].transform.position.y - playerList[1].transform.position.y) * 0.5f; 
            float newWidth = Mathf.Max(minSizeX, currentDifference * cameraLookAhead);
            Camera.main.fieldOfView = Mathf.Lerp(currentSize, Mathf.Max(currentDifferenceHeight * cameraLookAhead, newWidth * Screen.height/Screen.width, minSizeY), lerpThreshold);
        }
        

    }
}
