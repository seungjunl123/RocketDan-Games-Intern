using UnityEngine;
using UnityEngine.Pool;

public abstract class PoolableObject<T> : MonoBehaviour where T : MonoBehaviour
{
    public IObjectPool<T> ObjectPool { get; set; }

    public virtual void Release()
    {
        ObjectPool.Release(this as T);
    }
}
