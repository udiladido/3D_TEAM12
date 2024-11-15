using UnityEngine;

public class MoveController : BaseController
{
    private Vector3 moveDirection;
    [SerializeField] private float defaultSpeed = 2f;
    private Condition condition;
    public float MoveSpeed => condition == null ? defaultSpeed : condition.CurrentStat.moveSpeed;

    private void Update()
    {
        transform.position += moveDirection * MoveSpeed * Time.deltaTime;
    }
    
    public void SetMoveDirection(Vector3 direction)
    {
        moveDirection = direction;
    }
}