using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowScript : MonoBehaviour
{

    public float lerpThreshold;
    GameObject[] playerList;
    // Start is called before the first frame update
    void Start()
    {
        playerList = GameObject.FindGameObjectsWithTag("Player");
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
    }
}
