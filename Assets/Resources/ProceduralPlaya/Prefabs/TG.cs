using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TG : MonoBehaviour
{
    public int width = 256; //x-axis of the terrain
    public int height = 256; //z-axis

    public int depth = 100; //y-axis

    public float scale = 20f;

    public float offsetX = 100f;
    public float offsetY = 100f;

    public 

    private void Start()
    {
        offsetX = Random.Range(0f, 9999f);
        offsetY = Random.Range(0f, 9999f);

        Terrain terrain = GetComponent<Terrain>();
        terrain.terrainData = GenerateTerrain(terrain.terrainData);
    }

    TerrainData GenerateTerrain (TerrainData terrainData)
    {
        terrainData.heightmapResolution = width + 1;
        terrainData.size = new Vector3(width, depth, height);

        terrainData.SetHeights(0, 0, GenerateHeights());
        return terrainData;
    }

    public float limit_h = 10f;
    public float heightScale = 1f;

    float[,] GenerateHeights()
    {
        float[,] heights = new float[width, height];
        for(int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float t_h = (float) CalculateHeight(x, y) + CalculateDistance(x,y) * heightScale;
                t_h = Mathf.Pow(t_h, 1.1f);
                heights[x, y] = t_h > limit_h ? limit_h + Random.Range(-.005f,.005f) : t_h;
            }
        }

        return heights;
    }

    float CalculateDistance(int x, int y)
    {   
        float r = Mathf.Sqrt(
                        Mathf.Pow((float) (x - width/2 )/width, 2) + Mathf.Pow((float) (y - height/2 )/height, 2)
                    ) / Mathf.Sqrt(0.5f);
        return (1f) - r;
    }

    float CalculateHeight (int x, int y)
    {
        float xCoord = (float)x / width * scale + width/2;
        float yCoord = (float)y / height * scale + height/2;

        return Mathf.PerlinNoise(xCoord, yCoord) * 0.1f;
    }
}
