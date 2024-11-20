using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private void Start()
    {
        GameObject go = Managers.Pool.Spawn("/Monster");
        if (go == null)
        {
            return;
        }

        Monster monster = go.GetComponent<Monster>();
        if (monster == null)
        {
            Managers.Pool.Despawn(go);
            return;
        }

        if (monster.Initialize(1, 10001) == false)
        {
            Managers.Pool.Despawn(go);
            return;
        }
    }
}
