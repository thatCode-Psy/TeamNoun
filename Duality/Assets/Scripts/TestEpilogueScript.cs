using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TestEpilogueScript : MonoBehaviour
{
    public GameObject dialogueManager;
    bool triggered1 = false;
    bool triggered2 = false;
    bool triggered3 = false;

    GameObject mainCamera;
    public Vector3 startPosition1;
    public Vector3 endPosition1;
    public Vector3 rotation1;
    public float speed1;
    public Vector3 startPosition2;
    public Vector3 endPosition2;
    public Vector3 rotation2;
    public float speed2;
    public Vector3 startPosition3;
    public Vector3 endPosition3;
    public Vector3 rotation3;
    public float speed3;
    public Vector3 startPosition4;
    public Vector3 endPosition4;
    public Vector3 rotation4;
    public float speed4;
    float elapsed = 0;
    public GameObject black_char;
    public GameObject black_char2;
    public GameObject white_char;
    public GameObject moon;
    public GameObject sky;
    public GameObject tree;
    public List<Vector3> array;

    public float speed5;
    DialogueManager manager;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        //QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main.gameObject;
        moon.transform.position = new Vector3(4.65f, 7.88f, -8.61f);
        manager = dialogueManager.GetComponent<DialogueManager>();
    }

    // Update is called once per frame
    void Update()
    {
        elapsed += Time.deltaTime;


        black_char.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
        black_char2.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);

        if (elapsed < speed1)
        {
            float lerp = elapsed / speed1;
            mainCamera.transform.position = (lerp * endPosition1) + ((1.0f - lerp) * startPosition1);
            mainCamera.transform.rotation = Quaternion.Euler(rotation1);
            black_char.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
            black_char2.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
        }
        else if (elapsed < speed1 + speed2)
        {
            if(!triggered1)
            {
                Debug.Log("got here 1 " + Time.time);
                int start = 0;

                manager.PlayDialogue(start);
                triggered1 = true;
            }
            float lerp = (elapsed - speed1) / speed2;
            mainCamera.transform.position = (lerp * endPosition2) + ((1.0f - lerp) * startPosition2);
            mainCamera.transform.rotation = Quaternion.Euler(rotation2);
            moon.transform.position = new Vector3(2.8f, 7.1f, -8.61f);

        }
        else if (elapsed < speed1 + speed2 + speed3)
        {
            if (!triggered2)
            {
                Debug.Log("got here 2 " + Time.time);
                int start = 1;
                manager.PlayDialogue(start);
                //dialogueManager.SendMessage("PlayDialogue", start);
                triggered2 = true;
            }
            float lerp = (elapsed - speed1 - speed2) / speed3;
            mainCamera.transform.position = (lerp * endPosition3) + ((1.0f - lerp) * startPosition3);
            mainCamera.transform.rotation = Quaternion.Euler(rotation3);
            moon.transform.position = new Vector3(3.35f, 5.06f, -8.61f);
        }
        else if (elapsed < speed1 + speed2 + speed3 + speed4)
        {
            if (!triggered3)
            {
                Debug.Log("got here 3 " + Time.time);
                int start = 2;
                manager.PlayDialogue(start);
                //dialogueManager.SendMessage("PlayDialogue", start);
                triggered3 = true;
            }
            float lerp = (elapsed - speed1 - speed2 - speed3) / speed4;
            mainCamera.transform.position = (lerp * endPosition4) + ((1.0f - lerp) * startPosition4);
            mainCamera.transform.rotation = Quaternion.Euler(rotation4);
            moon.transform.position = new Vector3(4.65f, 7.88f, -8.61f);
            black_char.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
            black_char2.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);

        }
        else if (elapsed < speed1 + speed2 + speed3 + speed4 + speed5)
        {
            mainCamera.transform.position = new Vector3(0.0f, 5.0f, -18.0f);
            mainCamera.transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
        }
        else
        {
            SceneManager.LoadScene("Menu");
        }

    }
}
