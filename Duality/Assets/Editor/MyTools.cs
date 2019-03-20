using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class MyTools : MonoBehaviour
{
    [MenuItem("MyTools/Create Level")]
    static void CreateLevel(){
        
        GenerateLevel level = GameObject.FindGameObjectWithTag("LevelGenerator").GetComponent<GenerateLevel>();
        level.Generate();   
    }

    [MenuItem("MyTools/Delete Level")]
    static void DeleteLevel(){
        GameObject level = GameObject.FindGameObjectWithTag("Level");
        DestroyImmediate(level);
    }
}
