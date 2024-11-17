using UnityEngine;

public class ModelPivot : MonoBehaviour
{
    [field: SerializeField] public Transform LeftHand { get; private set; }
    [field: SerializeField] public Transform RightHand { get; private set; }
    
    private Transform leftHandItem;
    private Transform rightHandItem;
    
    public void EquipLeftHand(ItemEquipableEntity item)
    {
        if (item == null) return;
        GameObject go = Managers.Resource.Instantiate(item.equipedPrefabPath, LeftHand);
        leftHandItem = go?.transform;
    }
    
    public void EquipRightHand(ItemEquipableEntity item)
    {
        if (item == null) return;
        GameObject go = Managers.Resource.Instantiate(item.equipedPrefabPath, RightHand);
        rightHandItem = go?.transform;
    }
    
    public void UnEquipLeftHand()
    {
        if (leftHandItem == null) return;
        Managers.Resource.Destroy(leftHandItem.gameObject);
        leftHandItem = null;
    }
    
    public void UnEquipRightHand()
    {
        if (rightHandItem == null) return;
        Managers.Resource.Destroy(rightHandItem.gameObject);
        rightHandItem = null;
    }
}