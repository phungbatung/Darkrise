using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static Dictionary<GameObject, Pool> currentPools = new();

    private void Awake()
    {
        Pool[] pools = GetComponentsInChildren<Pool>();
        foreach (Pool pool in pools)
        {
            currentPools.Add(pool.prefab, pool);
        }
    }

    public static GameObject Spawn(GameObject prefab,Vector3 position, Transform parent = null)
    {
        if (currentPools.TryGetValue(prefab, out Pool pool))
        {
            return pool.Spawn(position, parent);
        }
        return SpawnNonPooledObject(prefab, position, parent);
    }

    public static GameObject SpawnNonPooledObject(GameObject prefab, Vector3 position, Transform parent = null)
    {
        GameObject instance = Instantiate(prefab);
        prefab.transform.position = position;
        prefab.transform.SetParent(parent);
        return instance;
    }

    public static void Despawn(GameObject prefab)
    {
        if (currentPools.TryGetValue(prefab, out Pool pool))
        {
            pool.Despawn(prefab);
            return;
        }
        Destroy(prefab);
    }

}
