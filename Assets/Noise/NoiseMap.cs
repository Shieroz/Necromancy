using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NoiseMap
{
    //resolution = resolution of map
    //seed = like Minecraft seed
    //scale = level of detail of map
    //octaves = levels of Perlin noise
    //persistance = decrease in amplitude(or level of influence, mountains, boulders, rocks, etc.) per octave
    //lacunarity = frequency of noise per level
    //offset = offset from current seed
    //size = the resolution of a grid with specified length (in meters)
    public static float[,] GenerateNoiseMap(int resolution, int seed, float scale, int octaves, float persistance, float lacunarity, Vector2 offset, float size) {
        float halfSize = size / 2;

        float[,] NoiseMap = new float[resolution + 1, resolution + 1];

        //clamp scale so that we don't have divide by 0
        if (scale <= 0) {
            scale = 0.00001f;
        }

        System.Random prng = new System.Random(seed);
        Vector2[] octaveOffset = new Vector2[octaves];
        float amplitude = 1f; //measure of persistance
        float frequency = 1f; //measure of lacunarity
        for (int o = 0; o < octaves; o++) {
            octaveOffset[o] = new Vector2(prng.Next(-100000, 100000) + offset.x, prng.Next(-100000, 100000) + offset.y);
            amplitude *= persistance;
        }
        

        //Generate Perlin noise map
        for (int z = 0; z <= resolution; z++) {
            for (int x = 0; x <= resolution; x++) {
                amplitude = 1f;
                frequency = 1f;
                float noiseHeight = 0f;
                //Apply octaves to the coordinate height
                for (int o = 0; o < octaves; o++) {
                    float coordX = ((float)x / resolution * size - halfSize + octaveOffset[o].x) / scale * frequency;
                    float coordZ = ((float)z / resolution * size - halfSize + octaveOffset[o].y) / scale * frequency;

                    noiseHeight += Mathf.PerlinNoise(coordX, coordZ) * amplitude;
                    amplitude *= persistance;
                    frequency *= lacunarity;
                }
                NoiseMap[x, z] = noiseHeight;
            }
        }
        return NoiseMap;
    }
}
