using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Sandevistan : MonoBehaviour
{
    public float activetime = 2f;

    [Header("Mesh Related")]
    public float meshRefreshRate = 0.1f;
    public float meshDestroyDelay = 3f;
    public Transform positionToSpawn;

    [Header("Shader Related")]
    public Material mat;
    public string shaderVarRef;
    public float shaderVarRate = 0.1f;
    public float shaderVarRefreshRate = 0.05f;

    private bool isTrailActive;
    private SkinnedMeshRenderer[] skinnedMeshRenderer;
    private readonly List<GameObject> trailObjects = new List<GameObject>();
    private readonly List<Mesh> trailMeshes = new List<Mesh>();
    private readonly List<Material> trailMaterials = new List<Material>();

    private void OnDisable()
    {
        // TimeTravel disables one character model while enabling the other. Unity
        // stops this object's coroutines on deactivation, so reset their state here.
        StopAllCoroutines();
        isTrailActive = false;
        skinnedMeshRenderer = null;
        ClearTrailObjects();
    }

    void Update()
    {
        if(Input.GetKeyDown (KeyCode.LeftShift) && !isTrailActive)
        {
            isTrailActive = true;
            StartCoroutine (ActivateTrail(activetime));
        }
    }
    
    IEnumerator ActivateTrail (float timeActive)
    {
        while (timeActive > 0)
        {
            timeActive -= meshRefreshRate;
            trailObjects.RemoveAll(trailObject => trailObject == null);
            trailMeshes.RemoveAll(trailMesh => trailMesh == null);
            trailMaterials.RemoveAll(trailMaterial => trailMaterial == null);

            if (skinnedMeshRenderer == null)
            {
                skinnedMeshRenderer = GetComponentsInChildren<SkinnedMeshRenderer>();
            }

            if (positionToSpawn == null)
            {
                break;
            }

            for (int i = 0; i < skinnedMeshRenderer.Length; i++)
            {
                if (skinnedMeshRenderer[i] == null || !skinnedMeshRenderer[i].enabled)
                {
                    continue;
                }

                GameObject gObj = new GameObject();
                trailObjects.Add(gObj);
                gObj.transform.SetPositionAndRotation(positionToSpawn.position, positionToSpawn.rotation);

                MeshRenderer mr = gObj.AddComponent<MeshRenderer>();
                MeshFilter mf = gObj.AddComponent<MeshFilter>();

                Mesh mesh = new Mesh();
                skinnedMeshRenderer[i].BakeMesh(mesh);
                trailMeshes.Add(mesh);

                mf.mesh = mesh;
                mr.material = mat;
                Material trailMaterial = mr.material;
                trailMaterials.Add(trailMaterial);

                StartCoroutine (AnimateMaterialFloat(trailMaterial, 0, shaderVarRate, shaderVarRefreshRate));

                Destroy (gObj, meshDestroyDelay);
                Destroy (mesh, meshDestroyDelay);
                Destroy (trailMaterial, meshDestroyDelay);
            }

            yield return new WaitForSeconds(meshRefreshRate);
        }
        
        isTrailActive = false;
    }

    private void ClearTrailObjects()
    {
        foreach (GameObject trailObject in trailObjects)
        {
            if (trailObject != null)
            {
                Destroy(trailObject);
            }
        }

        foreach (Mesh trailMesh in trailMeshes)
        {
            if (trailMesh != null)
            {
                Destroy(trailMesh);
            }
        }

        foreach (Material trailMaterial in trailMaterials)
        {
            if (trailMaterial != null)
            {
                Destroy(trailMaterial);
            }
        }

        trailObjects.Clear();
        trailMeshes.Clear();
        trailMaterials.Clear();
    }
    IEnumerator AnimateMaterialFloat(Material mat, float goal, float rate, float meshRefreshRate)
    {
        float valueToAnimate = mat.GetFloat(shaderVarRef);

        while (valueToAnimate > goal)
        {
            valueToAnimate -= rate;
            mat.SetFloat(shaderVarRef, valueToAnimate);
            yield return new WaitForSeconds(meshRefreshRate);
        }
    }
}
