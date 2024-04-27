using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshMerger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MergeMeshes() {
        MeshFilter[] meshFilters = transform.GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combineInstances = new CombineInstance[meshFilters.Length];

        for (int i = 0; i < meshFilters.Length; i++)
        {
            combineInstances[i].mesh = meshFilters[i].sharedMesh;
            combineInstances[i].transform = meshFilters[i].transform.localToWorldMatrix;
        }

        // Create a new GameObject to hold the combined mesh
        GameObject combinedObject = new GameObject("CombinedMesh");
        MeshFilter combinedMeshFilter = combinedObject.AddComponent<MeshFilter>();
        Mesh combinedMesh = new Mesh();
        combinedMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        combinedMesh.CombineMeshes(combineInstances, true);
        combinedMeshFilter.sharedMesh = combinedMesh;

        // Add a MeshCollider to the combined object for collision
        MeshCollider meshCollider = combinedObject.AddComponent<MeshCollider>();
        meshCollider.sharedMesh = combinedMesh;

        // Add a MeshRenderer to the combined object
        combinedObject.AddComponent<MeshRenderer>().sharedMaterial = meshFilters[0].GetComponent<MeshRenderer>().sharedMaterial;

        // Destroy the original children objects
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // Add object to the wall container
        combinedObject.transform.SetParent(transform);
    }
}
