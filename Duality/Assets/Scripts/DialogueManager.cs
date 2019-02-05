using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

    public string dialogueFileLocation;

    public ArrayList dialogueLines;

    public GameObject textbox;

	// Use this for initialization
	void Start () {
        FileInfo file = new FileInfo(dialogueFileLocation);
        StreamReader reader = file.OpenText();
        //Create an array of Strings based on an input file, where the first line is the number of dialogue lines
        string line = reader.ReadLine();
        dialogueLines = new ArrayList(int.Parse(line));

        /* Dialogue lines are of the format: {(integer defining speaker)(ActualLine)[(integer for delay in seconds)*/
        //string line;
        while(!reader.EndOfStream)
        {
            line = reader.ReadLine();
            dialogueLines.Add(line);
        }

        textbox.SetActive(false);
		for(int x = 0; x < dialogueLines.Count; x++)
        {
            Debug.Log(dialogueLines[x]);
        }
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
        Debug.Log(lineActual);
        
        //Figure out speaker later
       
        int delay = int.Parse(lineToPrint.Substring(locOfOpenBracket + 1, lineToPrint.Length - locOfOpenBracket - (lineToPrint.Length - locOfCloseBracket) - 1));
        Debug.Log(delay);
        StartCoroutine(Say(lineActual, 1, delay));

    }

    IEnumerator Say(string line, int speed, int delay)
    {
        textbox.SetActive(true);
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
        textbox.SetActive(false);
    }
}
