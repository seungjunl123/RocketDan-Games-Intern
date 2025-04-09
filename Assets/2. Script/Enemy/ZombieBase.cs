using Unity.Burst.Intrinsics;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(Rigidbody2D))]
public class ZombieBase : MonoBehaviour
{

    [SerializeField] private ZombieStat stats;
    public IObjectPool<ZombieBase> ObjectPool { get; set; }
    [SerializeField] private float HP;

    private Rigidbody2D rb;
    private bool isJumping = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        HP = stats.MaxHealth;
    }

    void Update()
    {
        transform.Translate(Vector2.left * stats.moveSpeed * Time.deltaTime);
        // 이동
        if (!isJumping)
        {

            Vector3 frontRayPos = transform.position - new Vector3(stats.zombieWidth, -0.1f ,0);
            Vector3 topRayPos = transform.position + new Vector3(0, stats.zombieHeight ,0);

            Debug.DrawRay(frontRayPos, Vector2.left * stats.detectDistance, Color.red);
            Debug.DrawRay(topRayPos, Vector2.up * stats.detectDistance, Color.green);
            // 앞에 Enemy가 있나 검사
            LayerMask myLayerMask = 1 << gameObject.layer;
            RaycastHit2D frontHit = Physics2D.Raycast(frontRayPos, Vector2.left, stats.detectDistance, myLayerMask);
            RaycastHit2D topHit  = Physics2D.Raycast(topRayPos, Vector2.up, stats.detectDistance, myLayerMask);

            if (frontHit.collider != null && frontHit.collider.gameObject != this.gameObject)
            {
                if (topHit.collider == null)
                {
                    // 부딪히기 직전이면 점프
                    Jump();
                }
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if(--HP == 0)
                {
                    Die();
                }
            }
        }
    }


    void Jump()
    {
        isJumping = true;
        rb.AddForce(Vector2.up * stats.jumpForce, ForceMode2D.Impulse);
        StartCoroutine(JumpCooldown());
    }

    IEnumerator JumpCooldown()
    {
        yield return new WaitForSeconds(stats.jumpCooldownSec);
        isJumping = false;
    }

    public void Die()
    {
        // 좀비가 죽거나 사라질 때 ObjectPool에 반환
        ObjectPool.Release(this);
    }

}
