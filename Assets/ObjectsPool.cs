using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Linq;
using UnityEngine;

public class ObjectsPool  : Singleton<ObjectsPool>
{
    Dictionary<string, List<GameObject>> m_singleUsedObjects = new Dictionary<string, List<GameObject>>();
    Dictionary<string, List<GameObject>> m_freeObjects = new Dictionary<string, List<GameObject>>();
    Dictionary<string, List<GameObject>> m_blokedOjects = new Dictionary<string, List<GameObject>>();

    public void AddToPoolObjects(GameObject ObjectToPool, int numberCreatedObjects, bool singleUseObject = false)
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

        if (singleUseObject)
        {
            List<GameObject> elems;
            if (m_singleUsedObjects.TryGetValue(ObjectToPool.name, out elems))
            {
                elems.AddRange(objectpool);
            }
            else m_singleUsedObjects.Add(ObjectToPool.name, objectpool);
        }
        else
        {       
            if (!m_freeObjects.ContainsKey(ObjectToPool.name))
            {
                m_freeObjects.Add(ObjectToPool.name, objectpool);
                m_blokedOjects.Add(ObjectToPool.name, new List<GameObject>());
            }
            else
            {
                m_freeObjects[ObjectToPool.name].AddRange(objectpool);
            }
        }
    }

    public GameObject GetObjectFromPool(GameObject ObjectFromPool, bool singleUsedObjec = false)
    {
        List<GameObject> poolObjetcs = null;

        if (singleUsedObjec)
        {
            if (m_singleUsedObjects.TryGetValue(ObjectFromPool.name, out poolObjetcs))
            {
                if (poolObjetcs.Count == 0)
                    AddToPoolObjects(ObjectFromPool, 50, true);

              
                var elem = poolObjetcs[0];
                elem.SetActive(true);
                poolObjetcs.Remove(elem);
                return elem;
            }
            else
            {
                AddToPoolObjects(ObjectFromPool, 50, true);
                var elem = m_singleUsedObjects[ObjectFromPool.name][0];
                m_singleUsedObjects[ObjectFromPool.name].Remove(elem);
                elem.SetActive(true);
                return elem;
            }

        }
        else
        {
            if (m_freeObjects.TryGetValue(ObjectFromPool.name, out poolObjetcs))
            {
                if (poolObjetcs.Count == 0)
                    AddToPoolObjects(ObjectFromPool, 50);

                var obj = poolObjetcs[0];
                m_blokedOjects[ObjectFromPool.name].Add(obj);
                m_freeObjects[ObjectFromPool.name].Remove(obj);
                obj.SetActive(true);
                obj.Descendants().ForEach(c => c.SetActive(true));
                return obj;

            }
            else
            {
                Debug.LogError(ObjectFromPool.name);
                AddToPoolObjects(ObjectFromPool, 50);
                var elem = m_freeObjects[ObjectFromPool.name][0];
                elem.SetActive(true);
                return elem;
            }
        }
    }

    public void UnblockAllObjects()
    {
        foreach (var name in m_blokedOjects.Keys)
        {
            m_blokedOjects[name].ForEach(o => { o.SetActive(false); o.transform.parent = null; });
            m_freeObjects[name].AddRange(m_blokedOjects[name]);
            m_blokedOjects[name].Clear();
        }

    }

    public override string ToString()
    {
        string str = "<color=green>[Object pool]: </color> ";
        foreach (var elem in m_freeObjects.Keys)
        {
            str += elem + "= " + (m_freeObjects[elem].Count + m_blokedOjects[elem].Count) + "; ";
        }

        return str;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        Debug.Log(ToString());
    }

}
