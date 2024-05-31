using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class ChunkTest
{
    [Test]
    public void GetBlockInChunkCoordinates_ReturnsCorrectCoordinates()
    {
        ChunkData chunkData = new ChunkData(16, 100, null, new Vector3Int(16, 3, 7));
        Vector3Int globalPosition = new Vector3Int(17, 53, 37); 

        Vector3Int result = Chunk.GetBlockInChunkCoordinates(chunkData, globalPosition);

        Assert.AreEqual(new Vector3Int(1, 50, 30), result);
    }

    [Test]
    public void SetBlock_SetsBlockAtCorrectPosition()
    {

        ChunkData chunkData = new ChunkData(16, 100, null, Vector3Int.zero);
        Vector3Int position = new Vector3Int(2, 8, 2); 
        BlockType blockType = BlockType.Grass_Dirt; 

        Chunk.SetBlock(chunkData, position, blockType);
        BlockType result = Chunk.GetBlockFromChunkCoordinates(chunkData, position);

        Assert.AreEqual(blockType, result);
    }

    [Test]
    public void GetBlockFromEmptyChunkCoordinates_ReturnsCorrectBlockType()
    {
        ChunkData chunkData = new ChunkData(16, 100, null, Vector3Int.zero);
        Vector3Int coordinates = new Vector3Int(2, 5, 3); 
        BlockType expectedBlockType = BlockType.Nothing; 

        BlockType result = Chunk.GetBlockFromChunkCoordinates(chunkData, coordinates);

        Assert.AreEqual(expectedBlockType, result);
    }
    
    [Test]
    public void SetBlockAtChunkEdge_SetsBlockCorrectly()
    {
        ChunkData chunkData = new ChunkData(16, 100, null, Vector3Int.zero);
        Vector3Int position = new Vector3Int(14, 99, 15);
        BlockType blockType = BlockType.Grass_Dirt; 

        Chunk.SetBlock(chunkData, position, blockType);
        BlockType result = Chunk.GetBlockFromChunkCoordinates(chunkData, position);

        Assert.AreEqual(blockType, result);
    }

    [Test]
    public void SetBlockAtChunkCorner_SetsBlockCorrectly()
    {
        ChunkData chunkData = new ChunkData(16, 100, null, Vector3Int.zero);
        Vector3Int position = new Vector3Int(0, 0, 0); 
        BlockType blockType = BlockType.Stone; 

        Chunk.SetBlock(chunkData, position, blockType);
        BlockType result = Chunk.GetBlockFromChunkCoordinates(chunkData, position);

        Assert.AreEqual(blockType, result);
    }

    [Test]
    public void IsOnTheEdge_ReturnsTrueForEdgeBlocks()
    {
        ChunkData chunkData = new ChunkData(16, 100, null, Vector3Int.zero);
        Vector3Int edgePosition = new Vector3Int(15, 50, 15); 

        bool result = Chunk.IsOnTheEdge(chunkData, edgePosition);

        Assert.IsTrue(result);
    }

    [Test]
    public void IsOnTheEdge_ReturnsFalseForNonEdgeBlocks()
    {
        ChunkData chunkData = new ChunkData(16, 100, null, Vector3Int.zero);
        Vector3Int nonEdgePosition = new Vector3Int(4, 44, 8); 

        bool result = Chunk.IsOnTheEdge(chunkData, nonEdgePosition);

        Assert.IsFalse(result);
    }

    [Test]
    public void GetChunkMeshData_ReturnsValidMeshData()
    {
        ChunkData chunkData = new ChunkData(16, 100, null, Vector3Int.zero);

        MeshData meshData = Chunk.GetChunkMeshData(chunkData);

        Assert.NotNull(meshData);
        Assert.IsEmpty(meshData.vertices);
        Assert.IsEmpty(meshData.triangles);
    }


    /*
    // A Test behaves as an ordinary method
    [Test]
    public void ChunkTestSimplePasses()
    {
        // Use the Assert class to test conditions
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator ChunkTestWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }*/
}
