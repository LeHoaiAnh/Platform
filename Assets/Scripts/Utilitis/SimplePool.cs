using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public static class SimplePool
{
    private const int DEFAULT_POOL_SIZE = 30;

    private class Pool
    {
        private readonly Queue<GameObject> inactive;
        private readonly GameObject prefab;
        public int StackCount
        {
            get
            {
                return inactive.Count;
            }
        }

        public Pool(GameObject prefab, int initialQty)
        {
            this.prefab = prefab;
            inactive = new Queue<GameObject>(initialQty);
        }

        GameObject _GetOrSpawn(Transform parent)
        {
            while (true)
            {
                GameObject obj;
                if (inactive.Count == 0)
                {
                    if (parent != null)
                    {
                        obj = GameObject.Instantiate(prefab, parent);
                    }
                    else
                    {
                        obj = GameObject.Instantiate(prefab);
                    }
                }
                else
                {
                    obj = inactive.Peek();

                    if (obj == null)
                    {
                        inactive.Dequeue();
                        continue;
                    }
                    else
                    {
                        var poolMem = obj.GetComponent<PoolMember>();
                        if (poolMem && poolMem.isLockForActivated)
                        {
                            if (parent != null)
                            {
                                obj = GameObject.Instantiate(prefab, parent);
                            }
                            else
                            {
                                obj = GameObject.Instantiate(prefab);
                            }
                        }
                        else
                        {
                            obj = inactive.Dequeue();
                        }
                    }

                    if (parent != null)
                    {
                        obj.transform.SetParent(parent);
                    }
                }
                return obj;
            }
        }

        GameObject _Spawn(Transform parent)
        {
            var obj = _GetOrSpawn(parent);
            if (obj != null)
            {
                var poolMem = obj.GetComponent<PoolMember>();
                if (poolMem == null)
                {
                    poolMem = obj.AddComponent<PoolMember>();
                }
                poolMem.myPool = this;
                poolMem.isActivate = true;
            }
            return obj;
        }

        public GameObject Spawn(Vector3 pos, Quaternion rot, bool isActive = true)
        {
            var obj = _Spawn(null);
            if (obj)
            {
                obj.transform.position = pos;
                obj.transform.rotation = rot;

                obj.gameObject.SetActive(isActive);
            }
            return obj;
        }

        public GameObject Spawn(Transform content, bool isActive = true)
        {
            var obj = _Spawn(content);
            if (obj)
            {
                obj.gameObject.SetActive(isActive);
            }
            return obj;
        }

        public T Spawn<T>(Vector3 pos, Quaternion rot)
        {
            return Spawn(pos, rot).GetComponent<T>();
        }
        public void Despawn(GameObject obj)
        {
            var poolMem = obj.GetComponent<PoolMember>();
            if (poolMem)
            {
                if (poolMem.isActivate == false)
                    return;

                poolMem.isActivate = false;
            }
            else
            if (!obj.activeSelf)
                return;

            obj.SetActive(false);
            inactive.Enqueue(obj);

            // khong the set parent OnDisable nen phai call coroutine de set parent muon hon
            //if (Hiker.GUI.GUIManager.Instance)
            //{
            //    Hiker.GUI.GUIManager.Instance.LateSetParent(obj.transform, null);
            //}
            //obj.transform.SetParent(null);
            poolMem.isLockForActivated = true;

            DOVirtual.DelayedCall(0.1f, () =>
            {
                if (poolMem && poolMem.isLockForActivated)
                {
                    poolMem.isLockForActivated = false;
                    poolMem.gameObject.SetActive(false);
                    poolMem.transform.SetParent(null);
                }
            },
            true);
            //Hiker.Utils.DoAction(Hiker.GUI.GUIManager.Instance, () =>
            //{
            //    if (poolMem && poolMem.isLockForActivated)
            //    {
            //        poolMem.isLockForActivated = false;
            //        poolMem.gameObject.SetActive(false);
            //        poolMem.transform.SetParent(null);
            //    }
            //}, 0.1f, true);
        }

        public void Clear()
        {
            while (inactive.Count > 0)
            {
                GameObject.Destroy(inactive.Dequeue());
            }
            inactive.Clear();
        }

    }
    private class PoolMember : MonoBehaviour
    {
        public Pool myPool;
        public bool isActivate { get; set; }
        public bool isLockForActivated { get; set; }
    }

    private static Dictionary<GameObject, Pool> pools;

    private static void Init(GameObject prefab = null, int qty = DEFAULT_POOL_SIZE)
    {
        if (pools == null)
        {
            pools = new Dictionary<GameObject, Pool>();
        }
        if (prefab != null && pools.ContainsKey(prefab) == false)
        {
            pools[prefab] = new Pool(prefab, qty);
        }
    }
    public static GameObject[] Preload(GameObject prefab, int qty = 1)
    {
        Init(prefab, qty);

        GameObject[] obs = new GameObject[qty];
        for (int i = 0; i < qty; i++)
        {
            obs[i] = Spawn(prefab, Vector3.zero, Quaternion.identity, false);
        }

        for (int i = 0; i < qty; i++)
        {
            Despawn(obs[i]);
        }

        return obs;
    }

    /// <summary>
    /// only spawn if livetime is greater than zero
    /// </summary>
    /// <param name="prefab"></param>
    /// <param name="pos"></param>
    /// <param name="rot"></param>
    /// <param name="liveTime">must be greater than 0</param>
    /// <param name="isActive"></param>
    /// <returns></returns>
    public static GameObject SpawnAutoDespawn(GameObject prefab, Vector3 pos, Quaternion rot, float liveTime, bool ignoreTimeScale, bool isActive = true)
    {
        if (liveTime <= 0) return null;

        var go = Spawn(prefab, pos, rot, isActive);
        if (go != null)
        {
            DOVirtual.DelayedCall(liveTime, () => Despawn(go), ignoreTimeScale);
        }
        return go;
    }
    public static GameObject Spawn(GameObject prefab, Vector3 pos, Quaternion rot, bool isActive = true)
    {
        Init(prefab);

        return pools[prefab].Spawn(pos, rot, isActive);
    }

    public static GameObject Spawn(GameObject prefab, Transform content, bool isActive = true)
    {
        Init(prefab);

        return pools[prefab].Spawn(content, isActive);
    }

    public static GameObject Spawn(GameObject prefab)
    {
        return Spawn(prefab, Vector3.zero, Quaternion.identity, true);
    }
    public static T Spawn<T>(T prefab) where T : Component
    {
        return Spawn(prefab, Vector3.zero, Quaternion.identity);
    }
    public static T Spawn<T>(T prefab, Vector3 pos, Quaternion rot, bool isActive = true) where T : Component
    {
        Init(prefab.gameObject);
        return pools[prefab.gameObject].Spawn<T>(pos, rot);
    }
    public static void Despawn(GameObject obj)
    {
        if (obj == null)
            return;

        PoolMember pm = obj.GetComponent<PoolMember>();
        if (pm == null || pm.myPool == null)
        {
#if LOG_HACK
            Debug.Log("Object '" + obj.name + "' wasn't spawned from a pool. Destroying it instead.");
#endif
            GameObject.Destroy(obj);
        }
        else
        {
            pm.myPool.Despawn(obj);
        }
    }
    public static void ClearAllPoolType(GameObject prefab)
    {
        if (prefab == null)
            return;
        if (pools == null)
        {
            Debug.Log("pool null");
            pools = new Dictionary<GameObject, Pool>();
        }

        if (pools.ContainsKey(prefab) == false)
        {
            Debug.Log("not ct,eturn");
        }
        else
        {
            pools[prefab].Clear();
            pools.Remove(prefab);
        }
    }

    public static int GetStackCount(GameObject prefab)
    {
        if (pools == null)
            pools = new Dictionary<GameObject, Pool>();
        if (prefab == null) return 0;
        return pools.ContainsKey(prefab) ? pools[prefab].StackCount : 0;
    }

    public static void ClearPool()
    {
        if (pools != null)
        {
            pools.Clear();
        }
    }
}