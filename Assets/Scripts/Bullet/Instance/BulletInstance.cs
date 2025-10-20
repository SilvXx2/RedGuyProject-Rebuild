using System.Collections;
using UnityEngine;

public class BulletInstance : MonoBehaviour
{
    [SerializeField] private Bullet data; // Flyweight

    private Vector3 direction = Vector3.right;
    private Coroutine lifeCoroutine;

    public BulletPool Pool { get; set; }

    // API similar al script base
    public void Initialize(Vector3 dir)
    {
        if (data == null)
        {
            Debug.LogWarning("BulletInstance no tiene 'data' asignado. Usando valores por defecto.");
            SetDirection(dir);
            StartLifeTimer(3f);
            return;
        }

        data.Apply(this, dir);
    }

    private void Update()
    {
        float speed = data != null ? data.Speed : 20f;
        transform.Translate(direction * Time.deltaTime * speed, Space.World);
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

    private void OnCollisionEnter(Collision _)
    {
        Pool?.ReturnToPool(this);
    }
}