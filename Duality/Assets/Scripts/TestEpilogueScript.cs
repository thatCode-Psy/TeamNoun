using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEpilogueScript : MonoBehaviour
{
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
    public GameObject white_char;
    public GameObject moon;
    public GameObject sky;
    public GameObject tree;
    public List<Vector3> array;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main.gameObject;
        moon.transform.position = new Vector3(4.65f, 7.88f, -8.61f);
    }

    // Update is called once per frame
    void Update()
    {
        elapsed += Time.deltaTime;

        if (elapsed < speed1)
        {
            float lerp = elapsed / speed1;
            mainCamera.transform.position = (lerp * endPosition1) + ((1.0f - lerp) * startPosition1);
            mainCamera.transform.rotation = Quaternion.Euler(rotation1);
        }
        else if (elapsed < speed1 + speed2)
        {
            float lerp = (elapsed - speed1) / speed2;
            mainCamera.transform.position = (lerp * endPosition2) + ((1.0f - lerp) * startPosition2);
            mainCamera.transform.rotation = Quaternion.Euler(rotation2);
        }
        else if (elapsed < speed1 + speed2 + speed3)
        {
            float lerp = (elapsed - speed1 - speed2) / speed3;
            mainCamera.transform.position = (lerp * endPosition3) + ((1.0f - lerp) * startPosition3);
            mainCamera.transform.rotation = Quaternion.Euler(rotation3);
            moon.transform.position = new Vector3(3.35f, 5.06f, -8.61f);
        }
        else if (elapsed < speed1 + speed2 + speed3 + speed4)
        {
            float lerp = (elapsed - speed1 - speed2 - speed3) / speed4;
            mainCamera.transform.position = (lerp * endPosition4) + ((1.0f - lerp) * startPosition4);
            mainCamera.transform.rotation = Quaternion.Euler(rotation4);
            moon.transform.position = new Vector3(4.65f, 7.88f, -8.61f);

        }
        else
        {
            mainCamera.transform.position = new Vector3(0.0f, 5.0f, -18.0f);
            mainCamera.transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
        }

    }
}
