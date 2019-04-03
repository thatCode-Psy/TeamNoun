using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowScript : MonoBehaviour
{

    public float dampTime;
    // public float cameraLookAhead;
    GameObject[] playerList;

    public float minFOV;
    public float maxFOV;
    
    Vector3 velocity;

    public float expectedMaxDistanceBetweenPlayers = 30f;

    public Vector3 offset;
   
    // Start is called before the first frame update
    void Start()
    {
        playerList = GameObject.FindGameObjectsWithTag("Player");
        // minSizeY = GetComponent<Camera>().orthographicSize;
        // minSizeX = minSizeY * Screen.width / Screen.height;
        
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
        Vector3 currentPosition = Vector3.zero;
        currentPosition.x = averageX;
        currentPosition.y = averageY;
        currentPosition += offset;
        transform.position = Vector3.SmoothDamp(transform.position, currentPosition, ref velocity, dampTime);
        if(playerList.Length > 1){
            
            float distance = Vector3.Distance(playerList[0].transform.position, playerList[1].transform.position);
            // float currentDifference = Mathf.Abs(playerList[0].transform.position.x - playerList[1].transform.position.x) * 0.5f;
            // float currentDifferenceHeight = Mathf.Abs(playerList[0].transform.position.y - playerList[1].transform.position.y) * 0.5f; 
            // float newWidth = Mathf.Max(minSizeX, currentDifference * cameraLookAhead);
            
            float newFov = Mathf.Lerp(minFOV, maxFOV, distance/expectedMaxDistanceBetweenPlayers);
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, newFov, Time.deltaTime);
        }
        

    }

    
}
