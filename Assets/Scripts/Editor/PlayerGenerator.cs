using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DevScene))]
public class PlayerGenerator : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (Application.isPlaying)
        {
            List<JobEntity> jobs = Managers.DB.GetAll<JobEntity>();

            foreach (var job in jobs)
            {
                DrawGenerateButton(job);
            }
        }
    }

    private void DrawGenerateButton(JobEntity job)
    {
        if (GUILayout.Button($"{job.displayTitle} 생성"))
        {
            Managers.Game.SetPlayerJobId(job.id);
            Managers.Game.CreatePlayer();
        }
    }
}