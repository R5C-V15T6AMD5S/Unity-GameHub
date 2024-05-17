using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeGenerator : MonoBehaviour
{
    // Klasa TreeGenerator je odgovorna za generiranje podataka drveća za pojedini chunk u svijetu. Koristi postavke šuma i Domain Warping kako bi se odlučile pozicije drveća.
    
    public NoiseSettings treeNoiseSettings;
    public DomainWarping domainWarping;

    public TreeData GenerateTreeData(ChunkData chunkData, Vector2Int mapSeedOffset)
    {
        /* 
            Metoda GenerateTreeData poziva funkciju GenerateTreeNoise kako bi se izračunala vrijednost šuma za svaku poziciju unutar chunka, a potom
            poziva FindLocalMaxima koja izračunava pozicije drveća za odgovarajući chunk
        */

        treeNoiseSettings.worldOffset = mapSeedOffset;
        TreeData treeData = new TreeData();
        float[,] noiseData = GenerateTreeNoise(chunkData, treeNoiseSettings);
        treeData.treePositions = DataProcessing.FindLocalMaxima(noiseData, chunkData.worldPosition.x, chunkData.worldPosition.z);
        return treeData;
    }

    private float[,] GenerateTreeNoise(ChunkData chunkData, NoiseSettings treeNoiseSettings)
    {
        // GenerateTreeNoise metoda stvara 2D polje vrijednosti šuma za odgovarajući chunk (za svaku poziciju u chunku se izračunava vrijednost šuma)

        // 2D polje float vrijednosti šuma
        float[,] noiseMax = new float[chunkData.chunkSize, chunkData.chunkSize];

        // Ove 4 vrijednosti su točke koje definiraju "granični okvir" chunka
        int xMax = chunkData.worldPosition.x + chunkData.chunkSize;
        int xMin = chunkData.worldPosition.x;
        int zMax = chunkData.worldPosition.z + chunkData.chunkSize;
        int zMin = chunkData.worldPosition.z;

        // S obzirom da su x i z vrijednost unutar dvostruke for petlje koordinate u prostoru svijeta, potrebni su xIndex i zIndex kako bi se vrijednosti korektno mapirale u noiseMax polje.
        int xIndex = 0, zIndex = 0;
        for (int x = xMin; x < xMax; x++) 
        {
            for (int z = zMin; z < zMax; z++)
            {
                noiseMax[xIndex, zIndex] = domainWarping.GenerateDomainNoise(x, z, treeNoiseSettings);
                zIndex++;
            }
            xIndex++;
            zIndex = 0;
        }
        return noiseMax;
    }
}
