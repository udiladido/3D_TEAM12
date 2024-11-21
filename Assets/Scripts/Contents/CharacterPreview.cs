using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPreview : MonoBehaviour
{

    private GameObject currentCharacter;  // 현재 캐릭터 저장용 변수

    public void SetJob(JobEntity job)
    {

        if (currentCharacter != null)
            Managers.Resource.Destroy(currentCharacter);

        currentCharacter = Managers.Resource.Instantiate(job.prefabPath, transform);
        currentCharacter.transform.localPosition = Vector3.zero;
        currentCharacter.transform.localRotation = Quaternion.Euler(0, 180, 0);  // y축 180도 회전
    }

}
