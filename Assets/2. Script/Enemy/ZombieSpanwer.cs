using UnityEngine;
using System;

public class ZombieSpanwer : ObjectPooler<ZombieBase>
{
    [SerializeField] private int LayerIndex;
    public static event Action OnMonsterSpawned;

    // Start is called once before the first execution of Update after the MonoBehaviour is created


    protected override ZombieBase CreateObject()
    {
        GameObject obj = Instantiate(setting.prefab, transform);
        ZombieBase zombie = obj.GetComponent<ZombieBase>();
        zombie.ObjectPool = pool;
        zombie.gameObject.layer = LayerMask.NameToLayer($"Ground_Line_{LayerIndex}");
        SpriteRenderer[] sr = zombie.gameObject.GetComponentsInChildren<SpriteRenderer>();
        foreach(SpriteRenderer renderer in sr)
        {
            renderer.sortingLayerName = $"Ground_Line_{LayerIndex}";
        }
        OnMonsterSpawned();
        return zombie;
    }

    protected override void OnGetFromPool(ZombieBase zombie)
    {
        base.OnGetFromPool(zombie);
        // 위치 초기화 등
        zombie.transform.position = GetSpawnPosition();
    }

    // 예시용 좀비 소환
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            pool.Get(); // Get만 해도 자동으로 OnGetFromPool 호출
        }
    }
}
