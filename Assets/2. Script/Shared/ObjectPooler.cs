using UnityEngine;
using UnityEngine.Pool;

public abstract class ObjectPooler<T> : MonoBehaviour where T : PoolableObject<T>
{
    [SerializeField] protected ObjectPoolSetting setting;
    protected IObjectPool<T> pool;

    protected virtual void Awake()
    {
        pool = new ObjectPool<T>(
            CreateObject,
            OnGetFromPool,
            OnReleaseToPool,
            OnDestroyObject,
            setting.collectionCheck,
            setting.defaultCapacity,
            setting.maxSize
        );
    }
    protected virtual T CreateObject()
    {
        GameObject obj = Instantiate(setting.prefab, transform);
        T component = obj.GetComponent<T>();
        component.ObjectPool = pool;
        return component;
    }
    protected virtual void OnGetFromPool(T obj)
    {
        obj.gameObject.SetActive(true);
    }

    protected virtual void OnReleaseToPool(T obj)
    {
        obj.gameObject.SetActive(false);
    }

    protected virtual void OnDestroyObject(T obj)
    {
        Destroy(obj.gameObject);
    }

    protected virtual T Get(Vector3 position)
    {
        T obj = pool.Get();
        obj.transform.position = position;
        return obj;
    }

    protected Vector3 GetSpawnPosition()
    {
        // 원하는 위치 반환
        return transform.position;
    }
}
