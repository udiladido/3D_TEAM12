using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "ItemDataList", menuName = "SO/ItemDataList", order = 0)]
public class JobDataList : ScriptableObject
{
    public List<JobEntity> JobList;

    public static ScriptableObject Convert(Sheet sheet)
    {
        JobDataList jobDataList = ScriptableObject.CreateInstance<JobDataList>();

        if (sheet is JobSheets jobSheets)
        {
            jobDataList.JobList = jobSheets.JobList;
            foreach (var job in jobDataList.JobList)
            {
                foreach (var stat in jobSheets.StatList)
                    if (stat.jobId == job.id)
                        job.jobStatEntity = stat;
            }
        }

        return jobDataList;
    }
}