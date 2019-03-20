using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(GenerateLevel))]
public class LevelEditor : Editor
{
    void OnSceneGUI() {
        GenerateLevel levelGenerator = target as GenerateLevel;    
    }
}
