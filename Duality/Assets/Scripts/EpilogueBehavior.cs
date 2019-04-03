using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EpilogueBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject camera;
    public GameObject button;
    public Text text;
    public GameObject lampShade;
    public float endCamPos;
    public float lerpThresh;
    //float t = 0;
    bool done = false;
    void Start()
    {
        camera = Camera.main.gameObject;
        button.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        button.SetActive(false);
        lampShade.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        lampShade.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        //printf()
        if(Mathf.Abs(endCamPos - camera.transform.position.y) > lerpThresh)
            camera.transform.position = new Vector3(camera.transform.position.x, Mathf.Lerp(camera.transform.position.y, endCamPos, Time.deltaTime), camera.transform.position.z);
        else if(!done)
        {
            lampShade.SetActive(true);
            done = true;
            StartCoroutine(fadeIn());
        }
    }

    IEnumerator fadeIn()
    {
        for (float i = 0; i <= 1; i += Time.deltaTime)
        {
            lampShade.GetComponent<Image>().color = new Color(0, 0, 0, i);
            yield return null;
        }
        button.SetActive(true);
        for (float i = 0; i <= 1; i += Time.deltaTime)
        {
            button.GetComponent<Image>().color = new Color(1, 1, 1, i);
            text.color = new Color(0, 0, 0, i);
            yield return null;
        }
    }

    public void Pressed()
    {
        SceneManager.LoadScene("Menu");
    }
}
