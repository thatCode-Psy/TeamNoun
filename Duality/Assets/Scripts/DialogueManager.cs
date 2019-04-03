using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{

    public string dialogueFileLocation;

    public ArrayList dialogueLines;

    //This stuff is now deprecated
    //public GameObject textbox;
    //public GameObject spritebox;
    //public GameObject blackSpeaker;
    //public GameObject whiteSpeaker;
    //public Sprite whiteBackround;
    //public Sprite blackBackground;

    public GameObject blackTextBox;
    public GameObject whiteTextBox;
    public GameObject otherTextBox;


    public AudioSource audio;
    private AudioClip blackText;
    private AudioClip whiteText;

    float defaultCameraSetting;
    int currentlySpeaking;
    int bufferedLines;
    // Use this for initialization
    void Start()
    {
        FileInfo file = new FileInfo(dialogueFileLocation);
        StreamReader reader = file.OpenText();
        //Create an array of Strings based on an input file, where the first line is the number of dialogue lines (DEPRECATED AGAIN)
        //string line = reader.ReadLine();
        dialogueLines = new ArrayList();
        audio = GetComponent<AudioSource>();
        blackText = Resources.Load("blackText") as AudioClip;
        whiteText = Resources.Load("whiteText") as AudioClip;

        /* Dialogue lines are of the format: {(integer defining speaker)}(ActualLine)[(integer for delay in seconds)](number of next line or -1 if that's the end of the dialogue)*/
        //1 is black
        //2 is white
        //3 is other
        string line;
        while (!reader.EndOfStream)
        {
            line = reader.ReadLine();
            if (!line.Equals(""))
            {
                dialogueLines.Add(line);
            }
        }

        //textbox.SetActive(false);
        //spritebox.SetActive(false);
        //whiteSpeaker.SetActive(false);
        //blackSpeaker.SetActive(false);
        //for (int x = 0; x < dialogueLines.Count; x++)
        //{
        //    Debug.Log(dialogueLines[x]);
        //}

        AquireTextBoxes();
        blackTextBox.SetActive(false);
        whiteTextBox.SetActive(false);
        otherTextBox.SetActive(false);


        currentlySpeaking = -1;
        bufferedLines = -1;
        defaultCameraSetting = 30;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayDialogueTesting(string input)
    {
        PlayDialogue(int.Parse(input));
    }

    private void SetPlayerMovement(bool canMove)
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject x in players)
        {
            x.GetComponent<PlayerScript>().canMove = canMove;
            x.GetComponent<Rigidbody2D>().velocity = new Vector3(0f,0f,0f);
            x.GetComponent<AnimCycle>().leftMove = false;
            x.GetComponent<AnimCycle>().rightMove = false;
        }
    }

    public void PlayDialoguePriority(float[] arguments)
    {
        SetPlayerMovement(false);
        if (arguments[1] != -1)
        {
            //Camera.main.GetComponent<CameraFollowScript>().cameraLookAhead = arguments[1];
        }
        PlayDialogue((int)arguments[0]);
        
    }

    public void PlayDialogue(int input)
    {
        string lineToPrint = (string)dialogueLines[input];

        int locOfOpenBracket = lineToPrint.IndexOf("[");
        int locOfCloseBracket = lineToPrint.IndexOf("]");
        int locOfOpenCurl = lineToPrint.IndexOf("{");
        int locOfCloseCurl = lineToPrint.IndexOf("}");

        string lineActual = lineToPrint.Substring(locOfCloseCurl + 1, lineToPrint.Length - locOfCloseCurl - (lineToPrint.Length - locOfOpenBracket) - 1);
        //Debug.Log(lineActual);

        int speaker = int.Parse(lineToPrint.Substring(locOfOpenCurl + 1, lineToPrint.Length - locOfOpenCurl - (lineToPrint.Length - locOfCloseCurl) - 1));

        float delay = float.Parse(lineToPrint.Substring(locOfOpenBracket + 1, lineToPrint.Length - locOfOpenBracket - (lineToPrint.Length - locOfCloseBracket) - 1));
        //Debug.Log(delay);

        //Debug.Log(lineToPrint.Substring(locOfCloseBracket + 1));
        int nextLineVar = int.Parse(lineToPrint.Substring(locOfCloseBracket + 1));
        //if (currentlySpeaking == )
        //{
        //currentlySpeaking = true;
        StartCoroutine(Say(lineActual, speaker, delay, nextLineVar));
        //}
        //else
        //{
        //    bufferedLines = input;
        //}


    }

    IEnumerator Say(string line, int speaker, float delay, int nextLineVar)
    {

        //textbox.SetActive(true);
        //spritebox.SetActive(true);
        Text textbox = null;
        int speed = 1;
        if (speaker == 1)
        {
            // audio.AudioSource
            //textbox.GetComponent<Text>().color = Color.black;
            //spritebox.GetComponent<Image>().sprite = whiteBackround;
            //blackSpeaker.SetActive(true);
            //if (blackTextBox == null)
            //{
            //    AquireTextBoxes();
            //}
            try
            {
                blackTextBox.SetActive(true);
            }
            catch(Exception e)
            {
                print(e.Message);
                AquireTextBoxes();
                blackTextBox.SetActive(true);
            }
            
            textbox = blackTextBox.GetComponentInChildren<Text>();
            Debug.Log(textbox.text);
            audio.clip = blackText;
            audio.Play();
            speed = 2;

        }
        else if (speaker == 2)
        {
            //textbox.GetComponent<Text>().color = Color.white;
            //spritebox.GetComponent<Image>().sprite = blackBackground;
            //whiteSpeaker.SetActive(true);
            // if (whiteTextBox == null)
            //{
            //    AquireTextBoxes();
            //}
            //whiteTextBox.SetActive(true);
            try
            {
                whiteTextBox.SetActive(true);
            }
            catch (Exception e)
            {
                AquireTextBoxes();
                whiteTextBox.SetActive(true);
            }
            textbox = whiteTextBox.GetComponentInChildren<Text>();
            audio.clip = whiteText;
            audio.Play();
            speed = 2;
        }
        else if (speaker == 3)
        {
            //if (otherTextBox == null)
            //{
            //    AquireTextBoxes();
            //}
            //otherTextBox.SetActive(true);
            try
            {
                otherTextBox.SetActive(true);
            }
            catch (Exception e)
            {
                AquireTextBoxes();
                otherTextBox.SetActive(true);
            }
            textbox = otherTextBox.GetComponentInChildren<Text>();
            audio.clip = whiteText;
            audio.Play();
            speed = 2;
        }
        //textbox.gameObject.SetActive(true);
        //int speed = 1;
        int x = 0;
        while (true)
        {
            if (speaker == 1)
            {
                if (blackTextBox == null)
                {
                    Debug.Log("its null");
                    AquireTextBoxes();
                    textbox = blackTextBox.GetComponentInChildren<Text>();
                }

            }
            else if (speaker == 2)
            {
                if (whiteTextBox == null)
                {
                    AquireTextBoxes();
                    textbox = whiteTextBox.GetComponentInChildren<Text>();
                }

            }

            if (x >= line.Length)
            {
                textbox.text = line;
                break;
            }
            else
            {
                textbox.text = line.Substring(0, x);
            }
            x += speed;
            yield return null;
        }
        audio.Stop();
        float time = Time.time;
        while (Time.time - time < delay)
        {
            yield return null;
        }
        //audio.Stop();
        blackTextBox.SetActive(false);
        whiteTextBox.SetActive(false);
        otherTextBox.SetActive(false);
        //spritebox.SetActive(false);
        //whiteSpeaker.SetActive(false);
        //blackSpeaker.SetActive(false);
        if (nextLineVar != -1)
        {
            PlayDialogue(nextLineVar);
        }
        else
        {
            SetPlayerMovement(true);
//            Camera.main.GetComponent<CameraFollowScript>().cameraLookAhead = defaultCameraSetting;
        }
        //else if(bufferedLines != -1)
        //{
        //    PlayDialogue(bufferedLines);
        //}
        //else
        //{
        //    currentlySpeaking = false;
        //}
        //Debug.Log("Finished execution");
    }

    void AquireTextBoxes()
    {
        blackTextBox = GameObject.FindGameObjectWithTag("PlayerBlack_textbox");
        whiteTextBox = GameObject.FindGameObjectWithTag("PlayerWhite_textbox");
        otherTextBox = GameObject.FindGameObjectWithTag("Other_textbox");
    }
}
