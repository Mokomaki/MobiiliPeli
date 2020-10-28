using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowPolyTerrain : MonoBehaviour
{
    [SerializeField] Color terrainColor;

    TerrainData m_terrainData;

    Mesh m_mesh; 
    MeshFilter m_filter;
    void Start()
    {
        m_mesh = new Mesh();
        m_filter = GetComponent<MeshFilter>();
        m_terrainData = GetComponent<Terrain>().terrainData;
        Generate();
        GetComponent<MeshCollider>().sharedMesh = m_filter.sharedMesh;
    }

    void Generate()
    {
        GetComponent<Terrain>().drawHeightmap = false;

        int w = m_terrainData.heightmapResolution;
        int h = m_terrainData.heightmapResolution;

        Vector3 meshScale = m_terrainData.size;
        int tRes = (int)Mathf.Pow(2,1);
        meshScale = new Vector3(meshScale.x / (w - 1) * tRes, meshScale.y, meshScale.z / (h - 1) * tRes);
        float[,] tData = m_terrainData.GetHeights(0, 0, w, h);

        w = (w - 1) / tRes + 1;
        h = (h - 1) / tRes + 1;
        Vector3[] tVertices = new Vector3[w * h];

        int[] tPolys;


        tPolys = new int[(w - 1) * (h - 1) * 6];

        for (int y = 0; y < h; y++)
        {
            for (int x = 0; x < w; x++)
            {
                tVertices[y * w + x] = Vector3.Scale(meshScale, new Vector3(y, -tData[x * tRes, y * tRes], x));
            }
        }

        int index = 0;

        for (int y = 0; y < h - 1; y++)
        {
            for (int x = 0; x < w - 1; x++)
            {
                tPolys[index++] = (y * w) + x;
                tPolys[index++] = ((y + 1) * w) + x;
                tPolys[index++] = (y * w) + x + 1;

                tPolys[index++] = ((y + 1) * w) + x;
                tPolys[index++] = ((y + 1) * w) + x + 1;
                tPolys[index++] = (y * w) + x + 1;
            }
        }

        Vector3[] oldVerts = tVertices;
        int[] triangles = tPolys;
        Vector3[] vertices = new Vector3[triangles.Length];
        Color[] colors = new Color[vertices.Length];
        for (int i = 0; i < triangles.Length; i++)
        {

            vertices[i] = oldVerts[triangles[i]];
            triangles[i] = i;
            colors[i] = terrainColor;
        }

        m_mesh.vertices = vertices;
        m_mesh.colors = colors;
        m_mesh.triangles = triangles;
        m_mesh.RecalculateBounds();
        m_mesh.RecalculateNormals();

        m_filter.sharedMesh = m_mesh;
    }

    int[] ConvertPosition(Vector3 position)
    {
        Vector3 terrainPosition = position - transform.position;

        Vector3 mapPosition = new Vector3
        (terrainPosition.x / m_terrainData.size.x, 0,
        terrainPosition.z / m_terrainData.size.z);

        float xCoord = mapPosition.x * m_terrainData.alphamapWidth;
        float zCoord = mapPosition.z * m_terrainData.alphamapHeight;

        return new int[] { (int)xCoord, (int)zCoord };
    }
}
