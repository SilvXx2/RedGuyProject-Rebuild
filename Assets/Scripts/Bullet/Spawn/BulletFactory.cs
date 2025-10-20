using UnityEngine;

public class BulletFactory : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private BulletInstance prefab;      // Prefab con BulletInstance y su 'data' (ScriptableObject) asignado
    [SerializeField] private int initialSize = 20;
    [SerializeField] private Transform container;

    public BulletPool Pool { get; private set; }

    private void Awake()
    {
        if (prefab != null)
            Initialize(prefab, initialSize, container);
    }

    public void Initialize(BulletInstance bulletPrefab, int initialSize = 20, Transform container = null)
    {
        prefab = bulletPrefab;
        this.initialSize = initialSize;
        this.container = container;
        Pool = new BulletPool(bulletPrefab, initialSize, container);
    }

    // Usa el pool interno del Factory
    public GameObject Create(Vector3 position)
    {
        return Create(position, Vector3.right);
    }

    public GameObject Create(Vector3 position, Vector3 direction)
    {
        if (Pool == null)
        {
            Debug.LogError("BulletFactory no inicializado. Llama a Initialize() o asigna el prefab en el inspector.");
            return null;
        }

        var instance = Pool.GetFromPool(position);
        instance.Initialize(direction);
        return instance.gameObject;
    }

    // Variante estática similar a tu ejemplo, usando un pool externo
    public static GameObject Create(Vector3 position, Vector3 direction, BulletPool bulletPool)
    {
        var instance = bulletPool.GetFromPool(position);
        instance.Initialize(direction);
        return instance.gameObject;
    }

    public void ReturnToPool(GameObject obj)
    {
        if (obj == null || Pool == null) return;
        var instance = obj.GetComponent<BulletInstance>();
        if (instance != null)
            Pool.ReturnToPool(instance);
    }
}