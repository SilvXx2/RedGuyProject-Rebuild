using UnityEngine;

[CreateAssetMenu(menuName = "Flyweights/Bullet", fileName = "Bullet")]
public class Bullet : ScriptableObject
{
    [Header("Datos intrínsecos (compartidos)")]
    [SerializeField] private float speed = 20f;
    [SerializeField] private float lifeTime = 3f;

    public float Speed => speed;
    public float LifeTime => lifeTime;

    // Aplica los datos del flyweight a una instancia
    public void Apply(BulletInstance instance, Vector3 dir)
    {
        if (instance == null) return;
        instance.SetDirection(dir.normalized);
        instance.StartLifeTimer(lifeTime);
    }
}