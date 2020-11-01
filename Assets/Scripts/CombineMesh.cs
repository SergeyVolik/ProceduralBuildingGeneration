using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class CombineMesh : MonoBehaviour
{
    public void CombineMeshes(Material material)
    {
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        

        List<MeshFilter> filtersToCombine = new List<MeshFilter>();

        foreach (var tomCombine in meshFilters)
        {
            if (tomCombine.GetComponent<MeshRenderer>().material.name == material.name)
                filtersToCombine.Add(tomCombine);
        }

        CombineInstance[] combines = new CombineInstance[filtersToCombine.Count];

        for (var i = 0; i < filtersToCombine.Count; i++)
        {
            combines[i].mesh = filtersToCombine[i].sharedMesh;
            combines[i].transform = filtersToCombine[i].transform.localToWorldMatrix;

           

            Destroy(filtersToCombine[i].gameObject.GetComponent<MeshRenderer>());
            Destroy(filtersToCombine[i].gameObject.GetComponent<MeshFilter>());
            //filtersToCombine[i].gameObject.GetComponent<MeshRenderer>().enabled = false;

        }

        MeshFilter meshFilter = transform.GetComponent<MeshFilter>();
        meshFilter.mesh = new Mesh();
        meshFilter.mesh.CombineMeshes(combines, true, true);
        GetComponent<MeshCollider>().sharedMesh = meshFilter.mesh;
        GetComponent<MeshRenderer>().material = material;
        transform.gameObject.SetActive(true);
    }
}
