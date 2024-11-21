using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterState_Dead : MonsterBaseState
{
    public MonsterState_Dead(MonsterStateMachine stateMachine) : base(stateMachine) { }

    private float beforeDissolveTime = 2.0f;
    private float dissolveRate = 0.0125f;
    private float dessolveInterval = 0.025f;

    private float beforeDissolveRemainTime;

    public override void Enter()
    {
        stateMachine.Monster.RigidBody.isKinematic = true;
        stateMachine.Monster.HitCollider.enabled = false;

        if (stateMachine.Monster.ValidAnimator)
        {
            stateMachine.Monster.AnimationController.TriggerDeath();
        }

        beforeDissolveRemainTime = beforeDissolveTime;
    }

    public override void Update()
    {
        if (beforeDissolveRemainTime >= 0)
        {
            beforeDissolveRemainTime -= Time.deltaTime;
            if (beforeDissolveRemainTime < 0)
            {
                Managers.Coroutine.StartCoroutine($"DissolveMonster_{stateMachine.Monster.Identifier}", DissolveCoroutine(stateMachine.Monster.SetDisable));
            }
        }
    }


    private IEnumerator DissolveCoroutine(Action callback)
    {
        if (stateMachine.Monster.ValidAnimator == false)
        {
            yield return null;
            callback?.Invoke();
        }

        List<Material[]> skinnedMaterials = new(); 
        SkinnedMeshRenderer[] skinnedMeshRenderers = stateMachine.Monster.AnimationController.transform.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (SkinnedMeshRenderer skinnedMeshRenderer in skinnedMeshRenderers)
        {
            skinnedMaterials.Add(skinnedMeshRenderer.materials);
        }

        WaitForSecondsRealtime waitForSecondsRealtime = new WaitForSecondsRealtime(dessolveInterval);
        float dissolve = 0;
        while (dissolve < 1)
        {
            dissolve += dissolveRate;

            foreach (Material[] materials in skinnedMaterials)
            {
                foreach (Material material in materials)
                {
                    material.SetFloat("_DissolveAmount", dissolve);
                }
            }

            yield return waitForSecondsRealtime;
        }

        callback?.Invoke();
    }
}
