using System.Collections;
using UnityEngine;

public class BulletInstance : MonoBehaviour
{
    [SerializeField] private Bullet data;
    [SerializeField] protected LayerMask destroyOnLayers;

    private Vector3 direction = Vector3.right;
    private Coroutine lifeCoroutine;

    public BulletPool Pool { get; set; }

    public float ExternalSpeedMultiplier { get; set; } = 1f;

    public void Initialize(Vector3 dir)
    {
        if (data == null)
        {
            SetDirection(dir);
            StartLifeTimer(3f);
            return;
        }

        data.Apply(this, dir);
    }

    private void Update()
    {
        float speed = data != null ? data.Speed : 20f;
        transform.Translate(direction * Time.deltaTime * speed * ExternalSpeedMultiplier, Space.World);
    }

    public void SetDirection(Vector3 dir)
    {
        direction = dir.sqrMagnitude > 0f ? dir.normalized : Vector3.right;
    }

    public void StartLifeTimer(float lifeTime)
    {
        if (lifeCoroutine != null)
            StopCoroutine(lifeCoroutine);

        lifeCoroutine = StartCoroutine(LifeTimer(lifeTime));
    }

    private IEnumerator LifeTimer(float lifeTime)
    {
        yield return new WaitForSeconds(lifeTime);
        Pool?.ReturnToPool(this);
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<EnemyController>(out var enemy))
        {
            enemy.TakeDamage(1);
            Pool?.ReturnToPool(this);
            return;
        }

        if ((destroyOnLayers.value & (1 << other.gameObject.layer)) != 0)
        {
            Pool?.ReturnToPool(this);
        }
    }
}