using System.Collections;
using System;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;


public class DebuggerUtils : MonoBehaviour
{
    private class MeshInfo
    {
        public int vertexCount;
        public int vertsCount;
        public int materialsCount;
        public GameObject gameObject;

    }

    [MenuItem("Beupse/Debug Meshes")]
    private static void DebugMeshes()
    {
        var meshInfos = new List<MeshInfo>();
        MeshFilter[] allMeshes = FindObjectsByType<MeshFilter>(FindObjectsSortMode.None);

        foreach (MeshFilter mesh in allMeshes)
        {
            if (!mesh.gameObject.activeInHierarchy)
                continue;
            if (mesh.sharedMesh != null)
            {
                var renderer = mesh.GetComponent<Renderer>();
                meshInfos.Add(new MeshInfo
                {
                    vertexCount = mesh.sharedMesh.vertexCount,
                    vertsCount = mesh.sharedMesh.triangles.Length,
                    materialsCount = renderer != null ? renderer.sharedMaterials.Length : 0,
                    gameObject = mesh.gameObject,
                });
            }
        }

        // Ordenar pela quantidade de vértices em ordem decrescente
        meshInfos = meshInfos.OrderByDescending(m => m.vertexCount).ToList();

        foreach (var meshInfo in meshInfos)
        {
            Debug.Log(
      $"<b>Objeto:</b> {meshInfo.gameObject.name} " +
      $"| <b>Vértices:</b> {meshInfo.vertexCount} " +
      $"| <b>Tris:</b> {meshInfo.vertsCount / 3} " +
      $"| <b>Materiais:</b> {meshInfo.materialsCount} " +
      $"| <i>Path:</i> {GetGameObjectPath(meshInfo.gameObject)}", meshInfo.gameObject
  );
        }
    }


    private static string GetGameObjectPath(GameObject obj)
    {
        string path = obj.name;
        while (obj.transform.parent != null)
        {
            obj = obj.transform.parent.gameObject;
            path = obj.name + "/" + path;
        }
        return path;
    }
}
