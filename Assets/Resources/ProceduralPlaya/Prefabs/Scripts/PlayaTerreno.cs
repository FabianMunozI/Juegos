using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter), typeof(MeshCollider))]

public class PlayaTerreno : MonoBehaviour
{
    public int width = 64, height = 64, octaves = 8, seed = 0;
    public float freq = 8f, redistribution = 1.0f, persistance = 0.5f, lacunarity = 2f;

    private List<float> heights = new List<float>();
    private Mesh mesh;
    private UnityEngine.Mesh terrainMesh;
    private MeshCollider meshCollider;

    void Start()
    {
        heights = new List<float>();
        mesh = GetComponent<MeshFilter>().mesh;
        ShapeTerrain();
    }

    private void ShapeTerrain()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        mesh = meshFilter.mesh;


        Vector3 [] vertices = mesh.vertices;


        // generate 2d lists of noise

        for (int i = 0; i < mesh.vertices.Length; i++)
        {
            //Debug.Log("x: " + (float) mesh.vertices[i].x + ", z: "+ (float) mesh.vertices[i].z);
            float scale = 50f;
            float amplitude = 1f;
            float noiseHeight = 0f;
            float frequency = freq;

            for (int o = 0; o < octaves; o++)
            {
                float xValue = (float) mesh.vertices[i].x / scale * frequency + width/2;
                float zValue = (float) mesh.vertices[i].z / scale * frequency + height/2;

                float perlinValue = Mathf.PerlinNoise(xValue + seed, zValue + seed);

                noiseHeight += perlinValue * amplitude;
                
                amplitude *= persistance;
                frequency *= lacunarity;
            }

            float denom = Mathf.Sqrt(0.5f);
            float nx = (float) mesh.vertices[i].x / width;
            float nz = (float) mesh.vertices[i].z / height;

            //heights.Add(noiseHeight * (1 - (Mathf.Sqrt((nx * nx) + (nz * nz)) / denom)));
            //heights.Add(noiseHeight);
            heights.Add(noiseHeight * (1 - (Mathf.Sqrt((nx * nx) + (nz * nz)) / denom)));
            //if (vertices[i].y <= 0f) vertices[i].y = 0;
            //else vertices[i].y = Mathf.Pow(noiseHeight * (1 - (Mathf.Sqrt((nx * nx) + (nz * nz)) / denom)), redistribution); 
            //Debug.Log(vertices[i].y);
            

        }

        float min = heights.Min();
        float max = heights.Max();
        float percentage = 0.7f;
        float range = max - min;

        float limit = min + range * percentage;

        //max = limit;

        for (int i = 0; i < heights.Count; i++)
        {   
            if (heights[i] > limit)
            {
                heights[i] = limit + Random.Range(0f,.1f);
            }

            // normalization
            heights[i] = (heights[i] - min) / (max - min);

            heights[i] = Mathf.Pow(heights[i], redistribution);
            Debug.Log(heights[i]);

            vertices[i].y = heights[i] * 100f;
        }

        /*
        self.elevation = (self.elevation - np.min(self.elevation)) / np.ptp(
            self.elevation
        )*/

        mesh.vertices = vertices;
        transform.GetComponent<MeshCollider>().sharedMesh = terrainMesh;

        // Recalcular las normales y la información de la malla
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        meshCollider = GetComponent<MeshCollider>();
        meshCollider.sharedMesh = mesh;
        

    }

    // Update is called once per frame
    void Update()
    {
        /*
        List<Vector3> vertices = new List<Vector3>();

        for (int i = 0; i < mesh.vertices.Length; i++)
        {
            vertices[i].y += Random.Range(-deformacion, deformacion);
            //Debug.Log("x: "+ mesh.vertices[i].x + ", z: "+ mesh.vertices[i].z);
        }

        terrainMesh = new UnityEngine.Mesh();
        terrainMesh.vertices = vertices.ToArray();
        //terrainMesh.uv = uvs.ToArray();
        //terrainMesh.triangles = triangles.ToArray();
        //terrainMesh.colors = colors.ToArray();
        //terrainMesh.normals = normals.ToArray();

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        transform.GetComponent<MeshFilter>().mesh = terrainMesh;
        transform.GetComponent<MeshCollider>().sharedMesh = terrainMesh;
        */
        
    }
}
