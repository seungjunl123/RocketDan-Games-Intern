using UnityEngine;
using System.Collections;
public class Bullet : PoolableObject<Bullet>
{
    
    [SerializeField] private float speed = 4f;
    [SerializeField] private float damage = 5f;
    private bool isHitted = false;
    private Rigidbody2D  rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(ReleaseFlyingBullet());
    }
    void FixedUpdate()
    {
        rb.linearVelocity = transform.right * speed;
    }
    void OnEnable()
    {
        isHitted = false;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Zombie"))
        {
            if (isHitted) return;
            isHitted = true;

            ZombieBase zombie = other.GetComponent<ZombieBase>();
            if (zombie != null)
            {
                zombie.TakeDamage(damage);
            }

            // 총알 제거 (Pool 반환)
            Release();
        }
    }

    IEnumerator ReleaseFlyingBullet()
    {
        yield return new WaitForSeconds(10f);
        Release();        
    }

}
