using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class TerrainGenerator : MonoBehaviour
{
    // TerrainGenerator upravlja stvaranjem terenskih podataka za chunk
    public BiomeGenerator biomeGenerator;

    [SerializeField]
    List<Vector3Int> biomeCenters = new List<Vector3Int>();

    // šumovi za svaki centar bioma
    List<float> biomeNoise = new List<float>();

    [SerializeField]
    private NoiseSettings biomeNoiseSettings;

    public DomainWarping biomeDomainWarping;

    [SerializeField]
    private List<BiomeData> biomeGeneratorsData = new List<BiomeData>();
    public ChunkData GenerateChunkData(ChunkData data, Vector2Int mapSeedOffset)
    {
        // Linije koda koje se tiču treeData su postavljene prije 2 for petlje iz razloga jer se želi jednom izračunati vrijednosti šuma.

        // Pomoću biomeSelection varijable, bira se određeni biom, odnosno koriste se njegove postavke
        BiomeGeneratorSelection biomeSelection = SelectBiomeGenerator(data.worldPosition, data, false);

        /*
        Generiraju se podaci o stablima ukoliko je potrebno, razliku može činiti sam mapSeedOffset.
        Podaci o stablima se spremaju u ChunkData. Razlog tome je da prilikom renderiranja chunkova, treba uključiti samo lišće koje je vidljivo u odgovarajućem chunku.
        */
        data.treeData = biomeSelection.biomeGenerator.GetTreeData(data, mapSeedOffset);

        for (int x = 0; x < data.chunkSize; x++)
        {
            for (int z = 0; z < data.chunkSize; z++)
            {
                // Na temelju pozicije, znat će se koji biom koristiti
                biomeSelection = SelectBiomeGenerator(new Vector3Int(data.worldPosition.x + x, 0, data.worldPosition.z + z), data);

                // Za svaku poziciju potrebno je generirati terenske podatke
                data = biomeSelection.biomeGenerator.ProcessChunkColumn(data, x, z, mapSeedOffset, biomeSelection.terrainSurfaceNoise);
            }
        }
        return data;
    }

    private BiomeGeneratorSelection SelectBiomeGenerator(Vector3Int worldPosition, ChunkData data, bool useDomainWarping = true)
    {
        // Ova metoda bira odgovarajući biomeGenerator za određenu poziciju i osigurava gladak prijelaz između bioma

        if (useDomainWarping == true)
        {
            // Koristi se za zanimljiv prijelaz između različitih bioma
            Vector2Int domainOffset = Vector2Int.RoundToInt(biomeDomainWarping.GenerateDomainOffset(worldPosition.x, worldPosition.z));
            worldPosition += new Vector3Int(domainOffset.x, 0, domainOffset.y);
        }

        // Sadrži informacije o biomima oko točke
        List<BiomeSelectionHelper> biomeSelectionHelpers = GetBiomeGeneratorSelectionHelpers(worldPosition);

        // Izabiru se dva najbliža bioma
        BiomeGenerator generator_1 = SelectBiome(biomeSelectionHelpers[0].Index);
        BiomeGenerator generator_2 = SelectBiome(biomeSelectionHelpers[1].Index);

        float distance = Vector3.Distance(biomeCenters[biomeSelectionHelpers[0].Index], biomeCenters[biomeSelectionHelpers[1].Index]);

        // Sljedeće varijable se koriste za kalkulaciju utjecaja najbližih bioma na visinu točke kako bi se dobio efekt glatkog prijelaza
        float weight_0 = biomeSelectionHelpers[0].Distance / distance;
        float weight_1 = 1 - weight_0;
        
        int terrainHeightNoise_0 = generator_1.GetSurfaceHeightNoise(worldPosition.x, worldPosition.z, data.chunkHeight);
        int terrainHeightNoise_1 = generator_2.GetSurfaceHeightNoise(worldPosition.x, worldPosition.z, data.chunkHeight);

        // Vraća se odgovarajući BiomeGeneratorSelection za određenu točku
        return new BiomeGeneratorSelection(generator_1, Mathf.RoundToInt(terrainHeightNoise_0 * weight_0 + terrainHeightNoise_1 * weight_1));
    }

    private BiomeGenerator SelectBiome(int index)
    {   
        // Ova funkcija vraća odgovarajući BiomeGenerator na temelju šuma generiranog za pojedini centar bioma
        
        float temp = biomeNoise[index];
        foreach (var data in biomeGeneratorsData)
        {
            if (temp >= data.temperatureStartThreshold && temp < data.temperatureEndThreshold) return data.biomeTerrainGenerator;
        }
        return biomeGeneratorsData[0].biomeTerrainGenerator;
    }

    private List<BiomeSelectionHelper> GetBiomeGeneratorSelectionHelpers(Vector3Int position)
    {
        // Ova funkcija resetira y vrijednost i poziva funkciju koja vraća najbliže biome oko određene točke

        position.y = 0;
        return GetClosestBiomeIndex(position);
    }

    private List<BiomeSelectionHelper> GetClosestBiomeIndex(Vector3Int position)
    {
        // Ova funkcija vraća BiomeSelectionHelper-e za najbliže biome oko točke

        return biomeCenters.Select((center, index) => new BiomeSelectionHelper
        {
            Index = index,
            Distance = Vector3.Distance(center, position)
        }).OrderBy(helper => helper.Distance).Take(4).ToList();
    }

    private struct BiomeSelectionHelper
    {
        // Ovo je struktura koja daje potrebne informacije o određenom biomu u blizini neke točke

        // Index je pozicija unutar liste BiomeData
        public int Index;
        // Distance se koristi za izbor 4 najbliža bioma
        public float Distance;
    }

    public void GenerateBiomePoints(Vector3 playerPosition, int drawRange, int mapSize, Vector2Int mapSeedOffset)
    {

        // Resetiraju se pozicije
        biomeCenters = new List<Vector3Int>();

        biomeCenters = BiomeCenterFinder.CalculateBiomeCenters(playerPosition, drawRange, mapSize);

        for (int i = 0; i < biomeCenters.Count; i++)
        {
            // Dodaju se offseti središtima bioma
            
            Vector2Int domainWarpingOffset = biomeDomainWarping.GenerateDomainOffsetInt(biomeCenters[i].x, biomeCenters[i].z);
            biomeCenters[i] += new Vector3Int(domainWarpingOffset.x, 0, domainWarpingOffset.y);
        }

        biomeNoise = CalculateBiomeNoise(biomeCenters, mapSeedOffset);
    }

    private List<float> CalculateBiomeNoise(List<Vector3Int> biomeCenters, Vector2Int mapSeedOffset)
    {
        // U ovoj funkciji se računa šum za sve centre bioma (temperaturna vrijednost)

        biomeNoiseSettings.worldOffset = mapSeedOffset;
        return biomeCenters.Select(center => MyNoise.OctavePerlin(center.x, center.z, biomeNoiseSettings)).ToList();
        
    }

    private void OnDrawGizmos()
    {
        // Funkcija kojom se vizualiziraju središnje pozicije bioma

        Gizmos.color = Color.blue;

        foreach (var biomCenterPoint in biomeCenters)
        {
            Gizmos.DrawLine(biomCenterPoint, biomCenterPoint + Vector3.up * 255);
        }
    }
}

[Serializable]
public struct BiomeData
{
    // Ova struktura sadrži karakteristike određenih bioma. Sadrži informacije bitne za biranje i generiranje terena za svaki tip bioma.

    // Na temelju temperaturnih vrijednosti centara bioma, zna se koji je tip bioma
    [Range(0f, 1f)]
    public float temperatureStartThreshold, temperatureEndThreshold;
    public BiomeGenerator biomeTerrainGenerator;
}

public class BiomeGeneratorSelection
{
    // Ova klasa je bitna jer sadrži izabrani BiomeGenerator i visinski šum za određenu točku.

    public BiomeGenerator biomeGenerator = null;

    // Ukoliko se želi visinski gladak prijelaz predat će se visina, inače se može generirati slučajna vrijednost
    public int? terrainSurfaceNoise = null;

    public BiomeGeneratorSelection(BiomeGenerator biomeGenerator, int? terrainSurfaceNoise = null)
    {
        this.biomeGenerator = biomeGenerator;
        this.terrainSurfaceNoise = terrainSurfaceNoise;
    }
}
