using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsPool  : Singleton<ObjectsPool>
{
    Dictionary<string, List<GameObject>> m_freeObjects = new Dictionary<string, List<GameObject>>();
    Dictionary<string, List<GameObject>> m_blokedOjects = new Dictionary<string, List<GameObject>>();

    public void AddToPoolObjects(GameObject ObjectToPool, int numberCreatedObjects)
    {
        if (!ObjectToPool)
        {
            Debug.LogError("AddToPoolObjects error -> gameObject is null");
            return;
        }
        else if (numberCreatedObjects <= 0)
        {
            Debug.LogError("AddToPoolObjects error -> numberCreatedObjects <=0");
            return;
        }
       
        Debug.Log(ObjectToPool.name);

        var objectpool = new List<GameObject>();

        for (var i = 0; i < numberCreatedObjects; i++)
        {
            var obj = Instantiate(ObjectToPool);
            obj.SetActive(false);
            objectpool.Add(obj);
        }

        if (!m_freeObjects.ContainsKey(ObjectToPool.name))
        {
            m_freeObjects.Add(ObjectToPool.name, objectpool);
            m_blokedOjects.Add(ObjectToPool.name, new List<GameObject>());
        }
        else {
            m_freeObjects[ObjectToPool.name].AddRange(objectpool);
        }
    }

    public GameObject GetObjectFromPool(GameObject ObjectFromPool)
    {
        List<GameObject> poolObjetcs;

        if (m_freeObjects.TryGetValue(ObjectFromPool.name, out poolObjetcs))
        {
            var obj = poolObjetcs[0];
            m_blokedOjects[ObjectFromPool.name].Add(obj);
            m_freeObjects[ObjectFromPool.name].Remove(obj);
            obj.SetActive(true);
            return obj;

        }
        else {
            AddToPoolObjects(ObjectFromPool, 50);
            return m_freeObjects[ObjectFromPool.name][0];
         }
    }

    public void UnblockAllObjects()
    {
        foreach (var name in m_blokedOjects.Keys)
        {
            m_blokedOjects[name].ForEach(o => o.SetActive(false));
            m_freeObjects[name].AddRange(m_blokedOjects[name]);
            m_blokedOjects[name].Clear();
        }
    }

}
