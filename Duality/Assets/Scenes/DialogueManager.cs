using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.
using UnityEngine;

public class DialogueManager : MonoBehaviour {

    public string dialogueFileLocation;

    public ArrayList dialogueLines;

    public GameObject textbox;

	// Use this for initialization
	void Start () {
        FileInfo file = new FileInfo(dialogueFileLocation);
        StreamReader reader = file.OpenText();
        //Create an array of Strings based on an input file, 
        string line = reader.ReadLine();
        dialogueLines = new ArrayList(int.Parse(line));


        while()
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
