using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building_Generator : MonoBehaviour
{
    float[,] Noise;
    public int resolution;
    public int seed;
    public float scale;
    public int octaves;
    public float persistance;
    public float lacunarity;
    public Vector2 offset;
    public float size;
    public Vector2 group;
    public List<Material> buildingMat;
    GameObject Buildings;

    void Generate()
    {
        Destroy(Buildings);
        Buildings = new GameObject();
        Noise = NoiseMap.GenerateNoiseMap(resolution, seed, scale, octaves, persistance, lacunarity, offset, size);
        float halfSize = size / 2f;
        for (int z = 0; z <= resolution; z++) {
            for (int x = 0; x <= resolution; x++) {
                GameObject b = GameObject.CreatePrimitive(PrimitiveType.Cube);
                b.transform.position = new Vector3(x + x / (int)group.x * group.y, Noise[x, z] / 2, z + z / (int)group.x * group.y);
                b.transform.localScale = new Vector3 (1f, Noise[x, z], 1f);
                b.transform.parent = Buildings.transform;
                b.GetComponent<Renderer>().material = buildingMat[Random.Range(0, buildingMat.Count)];
            }
        }
    }

    void Start()
    {
        Generate();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            Generate();
        }
    }
}
