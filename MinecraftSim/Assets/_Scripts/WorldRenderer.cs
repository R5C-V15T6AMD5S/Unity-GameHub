using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldRenderer : MonoBehaviour
{
    // Ova klasa je implementirana za ponovno iskorištenje starih isključenih chunkova u nove chunkove (nove pozicije)

    public GameObject chunkPrefab;

    // Isključeni chunk postavlja se u chunkPool, te prilikom "kreacije" novog chunka, uzima se chunk iz chunkPool-a, uključuje se te kreira pomoću novog MeshData
    public Queue<ChunkRenderer> chunkPool = new Queue<ChunkRenderer>();

    public void Clear(WorldData worldData)
    {
        // Ova metoda omogućuje čišćenje svijeta (chunk objekata koji su vidljivi)

        foreach (var item in worldData.chunkDictionary.Values)
        {
            Destroy(item.gameObject);
        }
        chunkPool.Clear();
    }
    internal ChunkRenderer RenderChunk(WorldData worldData, Vector3Int position, MeshData meshData)
    {
        // U ovoj metodi se renderira pojedini MeshData (odnosno chunk)

        ChunkRenderer newChunk = null;
        if (chunkPool.Count > 0)
        {
            newChunk = chunkPool.Dequeue();
            newChunk.transform.position = position;
        }
        else
        {
            GameObject chunkObject = Instantiate(chunkPrefab, position, Quaternion.identity);
            newChunk = chunkObject.GetComponent<ChunkRenderer>();
        }

        newChunk.InitializeChunk(worldData.chunkDataDictionary[position]);
        newChunk.UpdateChunk(meshData);
        newChunk.gameObject.SetActive(true);
        return newChunk;
    }

    public void RemoveChunk(ChunkRenderer chunk)
    {
        // Metoda koja isključuje chunk (ChunkRenderer) te ga postavlja u chunkPool kako bi se mogao ponovno iskoristiti za neku drugu poziciju u svijetu

        chunk.gameObject.SetActive(false);
        chunkPool.Enqueue(chunk);
    }
}
