#if (UNITY_EDITOR)
using UnityEditor;
using UnityEngine;

public class EditorHelper : EditorWindow
{
    private Vector3 moveOffset;

    [MenuItem("Tools/Move Selected Objects")]
    public static void ShowWindow()
    {
        GetWindow<EditorHelper>("Move Selected Objects");
    }

    private void OnGUI()
    {
        GUILayout.Label("Move Offset", EditorStyles.boldLabel);

        moveOffset = EditorGUILayout.Vector3Field("Offset", moveOffset);

        if (GUILayout.Button("Move Selected"))
        {
            MoveObjects();
        }
    }

    private void MoveObjects()
    {
        foreach (GameObject obj in Selection.gameObjects)
        {
            Undo.RecordObject(obj.transform, "Move Objects");
            obj.transform.position += moveOffset;
        }

        Debug.Log($"{Selection.gameObjects.Length} objects moved by {moveOffset}");
    }
}
#endif