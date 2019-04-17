using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class OpeningManager : MonoBehaviour
{
    public AudioSource player;
    // Start is called before the first frame update
    void Start()
    {
        player.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if(!player.isPlaying)
        {
            SceneManager.LoadScene("LevelGeneratorTest");
        }
    }
}
