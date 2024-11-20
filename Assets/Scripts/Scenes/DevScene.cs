using System.Collections;
using UnityEngine;

public class DevScene : SceneBase
{
    protected override void OnSceneLoad()
    {
        // 1. 씬 로드시 필요한 로직을 수행
        Managers.UI.Init();
        Managers.Sound.Init();
        Managers.Pool.Init();
        Managers.Coroutine.Init();
        
        Managers.UI.LoadSceneUI<UIGameScene>();

        LoadPlayer();
    }

    private void LoadPlayer()
    {
        JobEntity job = Managers.DB.Get<JobEntity>(11);
        
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
        ItemEntity comboWeapon = Managers.DB.Get<ItemEntity>(1503);
        ItemEntity skillWeapon = Managers.DB.Get<ItemEntity>(1505);
        ItemEntity armor = Managers.DB.Get<ItemEntity>(1601);
        ItemEntity accessory = Managers.DB.Get<ItemEntity>(1704);
        player.Equipment.Equip(comboWeapon);
        player.Equipment.Equip(skillWeapon);
        player.Equipment.Equip(armor);
        player.Equipment.Equip(accessory);
    }


    protected override void OnSceneLoaded()
    {
        // 2. 씬 로드가 완료된 후 필요한 로직을 수행
        Managers.Sound.PlayBGM("BGM");
        Managers.Sound.SetMasterVolume(0.3f);
        
        Managers.User.Earn(10_000_000_000);
    }
    protected override void OnSceneUnload()
    {
        // 3. 씬 언로드시 필요한 로직을 수행

    }
}