using System.Linq;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public float gunSpriteRot;
    [SerializeField] private Transform muzzle;
    [SerializeField] private Transform targetZombie;
    void OnEnable()
    {
        ZombieSpanwer.OnMonsterSpawned += HandleMonsterSpawned;
        ZombieBase.OnMonsterDied += HandleMonsterDied;
    }

    void OnDisable()
    {
        ZombieSpanwer.OnMonsterSpawned -= HandleMonsterSpawned;
        ZombieBase.OnMonsterDied -= HandleMonsterDied;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if(targetZombie != null)
        {
            Vector3 dir = targetZombie.position - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle-gunSpriteRot);
        }

    }

    public Transform Target()
    {
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Zombie");

        if(monsters.Count() == 0) return null;

        Transform closest = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject monster in monsters)
        {
            float dist = Vector2.Distance(transform.position, monster.transform.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                closest = monster.transform;
            }
        }

        return closest;
    }

    // Delegate
    private void HandleMonsterSpawned()
    {
        targetZombie = Target();
    }

    private void HandleMonsterDied(ZombieBase deadMonster)
    {

        targetZombie = Target();
        Debug.Log($"죽음으로 인한 Target 변경 {targetZombie}");
        
    }
}
