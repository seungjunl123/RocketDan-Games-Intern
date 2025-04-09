using Unity.Burst.Intrinsics;
using System.Collections;
using System;
using UnityEngine;
using UnityEngine.Pool;
using TMPro;

[RequireComponent(typeof(Rigidbody2D))]
public class ZombieBase : PoolableObject<ZombieBase>
{
    [SerializeField] private ZombieStat stats;
    [SerializeField] private float HP;
    [SerializeField] private GameObject floatingDamageTextPrefab;
    [SerializeField] private Transform damageTextSpawnPoint;
    public bool isDead;
    public static event Action<ZombieBase> OnMonsterDied;

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
        isDead = true;
        // 총구 방향 에러 방지를 위해 화면 밖으로 이동 후 비활성화
        this.transform.position = new Vector3(20f, 0f, 0f); 
        OnMonsterDied(this);
        Release();
    }

    public void TakeDamage(float InDamage)
    {
        HP -= InDamage;
        
            // 데미지 텍스트 생성
        GameObject dmgTextObj = Instantiate(floatingDamageTextPrefab, gameObject.transform.position, Quaternion.identity);
        FloatingDamage dmgText = dmgTextObj.GetComponent<FloatingDamage>();
        dmgText.Initialize(InDamage);
        if(HP <= 0) Die();
    }
}
