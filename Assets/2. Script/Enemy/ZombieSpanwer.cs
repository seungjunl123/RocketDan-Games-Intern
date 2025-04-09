using UnityEngine;
using UnityEngine.Pool;

public class ZombieSpanwer : MonoBehaviour
{
    [SerializeField] private ZombieBase zombiePrefab;
    [SerializeField] private bool collectionCheck = true;
    [SerializeField] private int defaultCapacity = 10;
    [SerializeField] private int maxSize = 50;
    [SerializeField] private int LayerIndex;
    private IObjectPool<ZombieBase> zombiePool;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        zombiePool = new ObjectPool<ZombieBase>(
            CreateZombie,
            OnGetFromPool,
            OnReleaseToPool,
            OnDestroyZombie,
            collectionCheck,
            defaultCapacity,
            maxSize
        );
    }

    private ZombieBase CreateZombie()
    {
        ZombieBase zombie = Instantiate(zombiePrefab, transform);
        zombie.ObjectPool = zombiePool;
        zombie.gameObject.layer = LayerMask.NameToLayer($"Ground_Line_{LayerIndex}");
        SpriteRenderer[] sr = zombie.gameObject.GetComponentsInChildren<SpriteRenderer>();
        foreach(SpriteRenderer renderer in sr)
        {
            renderer.sortingLayerName = $"Ground_Line_{LayerIndex}";
        }
        return zombie;
    }

    private void OnGetFromPool(ZombieBase zombie)
    {
        zombie.gameObject.SetActive(true);
        // 위치 초기화 등
        zombie.transform.position = GetSpawnPosition();
    }

    private void OnReleaseToPool(ZombieBase zombie)
    {
        zombie.gameObject.SetActive(false);
    }

    private void OnDestroyZombie(ZombieBase zombie)
    {
        Destroy(zombie.gameObject);
    }

    // 예시용 좀비 소환
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            zombiePool.Get(); // Get만 해도 자동으로 OnGetFromPool 호출
        }
    }

    private Vector3 GetSpawnPosition()
    {
        // 원하는 위치 반환
        return transform.position;
    }
}
