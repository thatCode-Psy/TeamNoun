using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UpdatedLightSourceScript))]
public class LightEditor : Editor {
    public override void OnInspectorGUI() {
        
        UpdatedLightSourceScript script = target as UpdatedLightSourceScript;

        DrawDefaultInspector();
        Quaternion xRot = Quaternion.Euler(90f, 0, 0);
        Quaternion yRot = Quaternion.Euler(0, -script.baseAngle, 0);
        script.transform.parent.rotation = xRot * yRot;
    }
}