using UnityEngine;

[CreateAssetMenu(fileName = "ObjectPoolSetting", menuName = "Scriptable Objects/ObjectPoolSetting")]
public class ObjectPoolSetting : ScriptableObject
{
    public GameObject prefab;
    public bool collectionCheck = true;
    public int defaultCapacity = 10;
    public int maxSize = 50;
}
