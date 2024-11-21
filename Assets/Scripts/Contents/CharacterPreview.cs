using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPreview : MonoBehaviour
{



    public void SetJob(JobEntity job)
    {
     
        GameObject jobGo = Managers.Resource.Instantiate(job.prefabPath, transform);
        jobGo.transform.localPosition = Vector3.zero;
  
        
    }

}
