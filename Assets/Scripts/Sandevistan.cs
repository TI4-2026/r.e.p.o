using UnityEngine;
using System.Collections;

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

            if (skinnedMeshRenderer == null)
            {
                skinnedMeshRenderer = GetComponentsInChildren<SkinnedMeshRenderer>();
            }

            for (int i = 0; i < skinnedMeshRenderer.Length; i++)
            {
                GameObject gObj = new GameObject();
                gObj.transform.SetPositionAndRotation(positionToSpawn.position, positionToSpawn.rotation);

                MeshRenderer mr = gObj.AddComponent<MeshRenderer>();
                MeshFilter mf = gObj.AddComponent<MeshFilter>();

                Mesh mesh = new Mesh();
                skinnedMeshRenderer[i].BakeMesh(mesh);

                mf.mesh = mesh;
                mr.material = mat;

                StartCoroutine (AnimateMaterialFloat(mr.material, 0, shaderVarRate, shaderVarRefreshRate));

                Destroy (gObj, meshDestroyDelay);
            }

            yield return new WaitForSeconds(meshRefreshRate);
        }
        
        isTrailActive = false;
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
