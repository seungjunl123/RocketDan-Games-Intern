using UnityEngine;
using System.Collections;

public class BulletSpawner : ObjectPooler<Bullet>
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(FireBullet());
    }


    IEnumerator FireBullet()
    {
        while(true)
        {
            yield return new WaitForSeconds(1f);

            pool.Get();
        }
    }
    protected override Bullet CreateObject()
    {
        GameObject obj = Instantiate(setting.prefab, transform);
        Bullet bullet = obj.GetComponent<Bullet>();
        bullet.ObjectPool = pool;

        // 총알에 움직임 부여
        return bullet;
    }

    protected override void OnGetFromPool(Bullet bullet)
    {
        base.OnGetFromPool(bullet);
        // 위치 초기화 등
        bullet.transform.position = GetSpawnPosition();
        bullet.transform.rotation = transform.rotation;
    }
}
