using System.Collections.Generic;
using UnityEngine;

public class BulletPool
{
    private readonly BulletInstance prefab;
    private readonly Queue<BulletInstance> pool = new Queue<BulletInstance>();
    private readonly Transform container;

    public BulletPool(BulletInstance prefab, int initialSize = 10, Transform container = null)
    {
        this.prefab = prefab;
        this.container = container;

        for (int i = 0; i < initialSize; i++)
        {
            var instance = CreateNew();
            instance.gameObject.SetActive(false);
            pool.Enqueue(instance);
        }
    }

    public BulletInstance GetFromPool(Vector3 position)
    {
        BulletInstance instance = pool.Count > 0 ? pool.Dequeue() : CreateNew();
        Transform t = instance.transform;
        t.position = position;
        instance.gameObject.SetActive(true);
        return instance;
    }

    public void ReturnToPool(BulletInstance instance)
    {
        if (instance == null) return;
        instance.gameObject.SetActive(false);
        pool.Enqueue(instance);
    }

    private BulletInstance CreateNew()
    {
        var instance = Object.Instantiate(prefab, container);
        instance.Pool = this;
        return instance;
    }
}