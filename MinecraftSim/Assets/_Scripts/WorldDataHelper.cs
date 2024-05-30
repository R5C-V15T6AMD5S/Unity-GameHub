using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class WorldDataHelper
{
    // Ova klasa pruža korisne metode za upravljanje podacima svijeta i pozicijama chunkova

    public static Vector3Int ChunkPositionFromBlockCoords(World world, Vector3Int worldPosition)
    {
        // Ova metoda vraća početne world koordinate chunka

        return new Vector3Int
        {
            x = Mathf.FloorToInt(worldPosition.x / (float)world.chunkSize) * world.chunkSize,
            y = Mathf.FloorToInt(worldPosition.y / (float)world.chunkHeight) * world.chunkHeight,
            z = Mathf.FloorToInt(worldPosition.z / (float)world.chunkSize) * world.chunkSize
        };
    }

    internal static ChunkRenderer GetChunk(World worldReference, Vector3Int worldPosition)
    {
        if(worldReference.worldData.chunkDictionary.ContainsKey(worldPosition))
                return worldReference.worldData.chunkDictionary[worldPosition];
        return null;
    }


    internal static List<Vector3Int> GetChunkPositionsAroundPlayer(World world, Vector3Int playerPosition)
    {
        // Ova metoda računa pozicije svih chunkova koje su potrebne oko igrača. Pozicije dodaje u listu te vraća tu listu.

        // Računaju se početne i krajnje pozicije za generaciju chunkova oko igrača
        int startX = playerPosition.x - (world.chunkDrawingRange) * world.chunkSize;
        int startZ = playerPosition.z - (world.chunkDrawingRange) * world.chunkSize;
        int endX = playerPosition.x + (world.chunkDrawingRange) * world.chunkSize;
        int endZ = playerPosition.z + (world.chunkDrawingRange) * world.chunkSize;

        // Npr. 0, 16, 32...
        List<Vector3Int> chunkPositionsToCreate = new List<Vector3Int>();
        for (int x = startX; x <= endX; x += world.chunkSize)
        {
            for (int z = startZ; z <= endZ; z += world.chunkSize)
            {
                Vector3Int chunkPos = ChunkPositionFromBlockCoords(world, new Vector3Int(x, 0, z));
                chunkPositionsToCreate.Add(chunkPos);
                if (x >= playerPosition.x - world.chunkSize
                    && x <= playerPosition.x + world.chunkSize
                    && z >= playerPosition.z - world.chunkSize
                    && z <= playerPosition.z + world.chunkSize)
                {
                    for (int y = -world.chunkHeight; y >= playerPosition.y - world.chunkHeight * 2; y -= world.chunkHeight)
                    {
                        chunkPos = ChunkPositionFromBlockCoords(world, new Vector3Int(x, y, z));
                        chunkPositionsToCreate.Add(chunkPos);
                    }
                }
            }
        }
        
        return chunkPositionsToCreate;
    }

    internal static List<Vector3Int> GetDataPositionsAroundPlayer(World world, Vector3Int playerPosition)
    {
        // Ova metoda računa pozicije svih podataka chunkova koje su potrebne oko igrača. Pozicije dodaje u listu te vraća tu listu.

        int startX = playerPosition.x - (world.chunkDrawingRange + 1) * world.chunkSize;
        int startZ = playerPosition.z - (world.chunkDrawingRange + 1) * world.chunkSize;
        int endX = playerPosition.x + (world.chunkDrawingRange + 1) * world.chunkSize;
        int endZ = playerPosition.z + (world.chunkDrawingRange + 1) * world.chunkSize;

        List<Vector3Int> chunkDataPositionsToCreate = new List<Vector3Int>();
        for (int x = startX; x <= endX; x += world.chunkSize)
        {
            for (int z = startZ; z <= endZ; z += world.chunkSize)
            {
                Vector3Int chunkPos = ChunkPositionFromBlockCoords(world, new Vector3Int(x, 0, z));
                chunkDataPositionsToCreate.Add(chunkPos);
                if (x >= playerPosition.x - world.chunkSize
                    && x <= playerPosition.x + world.chunkSize
                    && z >= playerPosition.z - world.chunkSize
                    && z <= playerPosition.z + world.chunkSize)
                {
                    for (int y = -world.chunkHeight; y >= playerPosition.y - world.chunkHeight * 2; y -= world.chunkHeight)
                    {
                        chunkPos = ChunkPositionFromBlockCoords(world, new Vector3Int(x, y, z));
                        chunkDataPositionsToCreate.Add(chunkPos);
                    }
                }
            }
        }
        
        return chunkDataPositionsToCreate;
    }

    internal static List<Vector3Int> GetUnneededChunks(WorldData worldData, List<Vector3Int> allChunkPositionsNeeded)
    {
        // Ova metoda pronalazi i vraća pozicije chunkova koje više nisu potrebne.

        List<Vector3Int> positionToRemove = new List<Vector3Int>();
        foreach (var pos in worldData.chunkDictionary.Keys.Where(pos => allChunkPositionsNeeded.Contains(pos) == false))
        {
            if (worldData.chunkDictionary.ContainsKey(pos))
            {
                positionToRemove.Add(pos);
            }
        }

        return positionToRemove;
    }

    internal static List<Vector3Int> GetUnneededData(WorldData worldData, List<Vector3Int> allChunkDataPositionsNeeded)
    {
        // Ova metoda pronalazi i vraća pozicije podataka chunkova koje više nisu potrebne.

        return worldData.chunkDataDictionary.Keys
        .Where(pos => allChunkDataPositionsNeeded.Contains(pos) == false && worldData.chunkDataDictionary[pos].modifiedByThePlayer == false)
        .ToList();
    }

    internal static void RemoveChunk(World world, Vector3Int pos)
    {
        // Ova metoda briše chunk iz specificirane pozicije u svijetu.

        ChunkRenderer chunk = null;
        if (world.worldData.chunkDictionary.TryGetValue(pos, out chunk))
        {
            world.worldRenderer.RemoveChunk(chunk);
            world.worldData.chunkDictionary.Remove(pos);
        }
    }

    internal static void RemoveChunkData(World world, Vector3Int pos)
    {
        // Ova metoda briše podatak chunka iz specificirane pozicije u svijetu.

        world.worldData.chunkDataDictionary.Remove(pos);
    }

    internal static List<Vector3Int> SelectDataPositionsToCreate(WorldData worldData, List<Vector3Int> allChunkDataPositionsNeeded, Vector3Int playerPosition)
    {
        // Ova metoda bira podatke chunkova koji se trebaju stvoriti s obzirom na blizinu igrača. Sortira ih, te vraća listu tih podataka.

        return allChunkDataPositionsNeeded.Where(pos => worldData.chunkDataDictionary.ContainsKey(pos) == false).OrderBy(pos => Vector3.Distance(playerPosition, pos)).ToList();
    }

    internal static List<Vector3Int> SelectPositionsToCreate(WorldData worldData, List<Vector3Int> allChunkPositionsNeeded, Vector3Int playerPosition)
    {
        // Ova metoda bira chunkove koji se trebaju stvoriti s obzirom na blizinu igrača. Sortira ih, te vraća listu tih podataka.

        return allChunkPositionsNeeded.Where(pos => worldData.chunkDictionary.ContainsKey(pos) == false).OrderBy(pos => Vector3.Distance(playerPosition, pos)).ToList();
    }

    public static ChunkData GetChunkData(World worldReference, Vector3Int worldBlockPosition)
    {
        // U metodi GetChunkData pokušava se dohvatiti chunk koji sadrži blok sa definiranom pozicijom u svijetu

        Vector3Int chunkPosition = ChunkPositionFromBlockCoords(worldReference, worldBlockPosition);
        ChunkData containerChunk = null;
        worldReference.worldData.chunkDataDictionary.TryGetValue(chunkPosition, out containerChunk);
        return containerChunk;
    }

    internal static void SetBlock(World worldReference, Vector3Int worldBlockPosition, BlockType blockType)
    {
        // U ovoj metodi pokušava se postaviti tip bloka na blok sa poznatom pozicijom u svijetu

        ChunkData chunkData = GetChunkData(worldReference, worldBlockPosition);
        if (chunkData != null)
        {
            Vector3Int localPosition = Chunk.GetBlockInChunkCoordinates(chunkData, worldBlockPosition);
            Chunk.SetBlock(chunkData, localPosition, blockType);
        }
    }
}
