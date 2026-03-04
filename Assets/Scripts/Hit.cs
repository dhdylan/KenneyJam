using UnityEngine;

[CreateAssetMenu(fileName="Hit", menuName ="Combat/Hit", order = 0)]
public class Hit : ScriptableObject
{
    public int damage = 1;
    public float knockbackAmount = 5f;

    public GameObject instigator { get; set; }
}