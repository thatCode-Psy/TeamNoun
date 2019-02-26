using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UpdatedSweepingLightScript))]
public class SweepLightEditor : Editor
{
    public override void OnInspectorGUI() {
        
        UpdatedSweepingLightScript script = target as UpdatedSweepingLightScript;

        DrawDefaultInspector();
        Quaternion xRot = Quaternion.Euler(90f, 0, 0);
        Quaternion yRot = Quaternion.Euler(0, -script.baseAngle, 0);
        script.transform.rotation = xRot * yRot;
    }

}
