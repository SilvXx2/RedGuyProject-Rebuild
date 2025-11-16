using UnityEngine;

[CreateAssetMenu(menuName = "Enemies/Enemy Type", fileName = "EnemyType")]
public class EnemyType : ScriptableObject
{
    [Header("Movimiento")]
    public float patrolSpeed = 3f;
    public float pursueSpeed = 5f;
    [Range(-1, 1)] public int initialDirection = 1;

    [Header("Detección paredes")]
    public LayerMask wallMask;
    public float wallRayDistance = 1f;

    [Header("Visión jugador")]
    public LayerMask playerMask;
    public float viewDistance = 8f;

    [Header("Vida")]
    public int maxHealth = 3;
}