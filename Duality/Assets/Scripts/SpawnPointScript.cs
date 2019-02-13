using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointScript : MonoBehaviour
{

    public bool isInitialSpawn;
    public PlayerColor color;

    public GameObject playerPrefab;

    public int spawnNumber;

    // Start is called before the first frame update
    void Awake()
    {
        if(isInitialSpawn){
            GameObject player = Instantiate(playerPrefab);
            player.transform.position = transform.position;
            PlayerScript script = player.GetComponent<PlayerScript>();
            script.setCurrentSpawn(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
