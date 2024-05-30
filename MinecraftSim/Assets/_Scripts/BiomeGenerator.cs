using System;
using System.Collections.Generic;
using UnityEngine;

public class BiomeGenerator : MonoBehaviour
{
    // Ova skripta je odgovorna za generiranje terena. Izračunava visinu generirane površine. Poziva Handlere koji postavljaju tip bloka.

    // Razina ispod koje je voda
    public int waterThreshold = 50;
    
    public NoiseSettings biomeNoiseSettings;

    public DomainWarping domainWarping;

    public bool useDomainWarping = true;

    // Handleri osnovnih blokova
    public BlockLayerHandler startLayerHandler;

    // Handleri dodatnih blokova
    public List<BlockLayerHandler> additionalLayerHandlers;

    // Ukoliko se ne želi postavljati stabla u određenom biomu, treeGenerator se postavlja na null vrijednost
    public TreeGenerator treeGenerator;

    public ChunkData ProcessChunkColumn(ChunkData data, int x, int z, Vector2Int mapSeedOffset, int? terrainHeightNoise)
    {
        // Ova metoda prerađuje stupac u chunku zadajući tip bloka za svaki blok unutar chunka baziranim na visini terena.

        biomeNoiseSettings.worldOffset = mapSeedOffset;

        // Izračunava se visina terena ukoliko već nije za trenutnu x i z poziciju unutar chunka
        int groundPosition;
        if (terrainHeightNoise.HasValue == false) groundPosition = GetSurfaceHeightNoise(data.worldPosition.x + x, data.worldPosition.z + z, data.chunkHeight);
        else groundPosition = terrainHeightNoise.Value;

        // Pomoću sustava "Chain Of Responsibility" odlučuje se na temelju visine koji tip bloka postaviti
        for (int y = data.worldPosition.y; y < data.worldPosition.y + data.chunkHeight; y++)
        {
            startLayerHandler.Handle(data, x, y, z, groundPosition, mapSeedOffset);
        }

        foreach (var layer in additionalLayerHandlers)
        {
            layer.Handle(data, x, data.worldPosition.y, z, groundPosition, mapSeedOffset);
        }
        return data;
    }

    internal TreeData GetTreeData(ChunkData data, Vector2Int mapSeedOffset)
    {
        if (treeGenerator == null) return new TreeData();
        return treeGenerator.GenerateTreeData(data, mapSeedOffset);
    }

    public int GetSurfaceHeightNoise(int x, int z, int chunkHeight)
    {
        // Ova metoda izračunava visinu površine za dane x i z koordinate koristeći generaciju šuma

        // Visina površine koja je generirana šumom
        float terrainHeight;

        if(useDomainWarping == false)
        {
            terrainHeight = MyNoise.OctavePerlin(x, z, biomeNoiseSettings);
        }
        else
        {
            terrainHeight = domainWarping.GenerateDomainNoise(x, z, biomeNoiseSettings);
        }

        // Namještanje visine pomoću redistribucije
        terrainHeight = MyNoise.Redistribution(terrainHeight, biomeNoiseSettings);

        // Preslikavanje visine unutar visine chunka
        int surfaceHeight = MyNoise.RemapValue01ToInt(terrainHeight, 0, chunkHeight);
        return surfaceHeight;
    }
}
