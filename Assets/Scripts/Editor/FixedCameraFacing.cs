using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Player))]
public class FixedCameraFacing : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Player player = (Player)target;
        if (GUILayout.Button("Fix Camera Facing"))
        {
            player.FixCameraFacing();
        }
    }
}