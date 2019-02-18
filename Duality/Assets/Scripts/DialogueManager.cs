using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

    public string dialogueFileLocation;

    public ArrayList dialogueLines;

    public GameObject textbox;
    public GameObject spritebox;

    public GameObject blackSpeaker;
    public GameObject whiteSpeaker;
    public AudioSource audio;

    public Sprite whiteBackround;
    public Sprite blackBackground;

    private AudioClip blackText;
    private AudioClip whiteText;

    int currentlySpeaking;
    int bufferedLines;
	// Use this for initialization
	void Start() {
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
        string line;
        while(!reader.EndOfStream)
        {
            line = reader.ReadLine();
            if (!line.Equals(""))
            {
                dialogueLines.Add(line);
            }
        }

        textbox.SetActive(false);
        spritebox.SetActive(false);
        whiteSpeaker.SetActive(false);
        blackSpeaker.SetActive(false);
        for (int x = 0; x < dialogueLines.Count; x++)
        {
            Debug.Log(dialogueLines[x]);
        }

        currentlySpeaking = -1;
        bufferedLines = -1;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlayDialogueTesting(string input)
    {
        PlayDialogue(int.Parse(input));
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
       
        int delay = int.Parse(lineToPrint.Substring(locOfOpenBracket + 1, lineToPrint.Length - locOfOpenBracket - (lineToPrint.Length - locOfCloseBracket) - 1));
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

    IEnumerator Say(string line, int speaker, int delay, int nextLineVar)
    {
        
        textbox.SetActive(true);
        spritebox.SetActive(true);
        int speed = 1;
        if(speaker == 1)
        {
            // audio.AudioSource
            textbox.GetComponent<Text>().color = Color.black;
            spritebox.GetComponent<Image>().sprite = whiteBackround;
            blackSpeaker.SetActive(true);
            audio.clip = blackText;
            audio.Play();
            speed = 1;

        }
        else if(speaker == 2)
        {
            textbox.GetComponent<Text>().color = Color.white;
            spritebox.GetComponent<Image>().sprite = blackBackground;
            whiteSpeaker.SetActive(true);
            audio.clip = whiteText;
            audio.Play();
            speed = 2;
        }
        //int speed = 1;
        int x = 0;
        while(true)
        {
            if (x >= line.Length)
            {
                textbox.GetComponent<Text>().text = line;
                break;
            }
            else
            {
                textbox.GetComponent<Text>().text = line.Substring(0, x);
            }
            x += speed;
            yield return null;
        }
        float time = Time.time;
        while(Time.time - time < delay)
        {
            yield return null;
        }
        audio.Stop();
        textbox.SetActive(false);
        spritebox.SetActive(false);
        whiteSpeaker.SetActive(false);
        blackSpeaker.SetActive(false);
        if (nextLineVar != -1)
        {
            PlayDialogue(nextLineVar);
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
}
