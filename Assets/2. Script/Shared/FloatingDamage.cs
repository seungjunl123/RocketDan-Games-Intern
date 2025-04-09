using UnityEngine;
using TMPro;
public class FloatingDamage : MonoBehaviour
{
    [SerializeField] private TMP_Text damageText;
    [SerializeField] private float lifetime = 0.5f;
    [SerializeField] private float floatSpeed = 50f;

    public void Initialize(float damage)
    {
        damageText.text = damage.ToString();
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.position += Vector3.up * floatSpeed * Time.deltaTime;
    }
}