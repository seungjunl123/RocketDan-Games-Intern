using UnityEngine;

[CreateAssetMenu(fileName = "ZombieStat", menuName = "Scriptable Objects/ZombieStat")]
public class ZombieStat : ScriptableObject
{
    public  float moveSpeed = 2f;
    public float jumpForce = 5f;
    public float detectDistance = 0.05f;
    public float zombieWidth = 0.6f;
    public float jumpCooldownSec = 3f;
    public float zombieHeight = 1f;
    public float MaxHealth = 10f;
}
