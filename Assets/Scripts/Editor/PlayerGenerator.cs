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
            Player player = GameObject.FindObjectOfType<Player>();
            if (player == null)
                player = Managers.Resource.Instantiate("Player")?.GetComponent<Player>();

            if (player == null)
            {
                Debug.LogWarning("Player 프리팹이 없습니다.");
                return;
            }
            player.gameObject.name = nameof(Player);
            player.SetJob(job);
        }
    }
}