using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class WorldDataHelperTest
{
    private World world;

    [SetUp]
    public void Setup()
    {
        GameObject worldObject = GameObject.FindWithTag("World");
        world = worldObject.GetComponent<World>();
        world.chunkDrawingRange = 1;
    }

    [TearDown]
    public void Teardown()
    {
        world = null;
    }

    [Test]
    public void ChunkPositionFromBlockCoords_ReturnsCorrectChunkPosition()
    {
        Vector3Int worldPosition = new Vector3Int(10, 5, -3);
        Vector3Int expectedChunkPosition = new Vector3Int(0, 0, -16); 

        Vector3Int result = WorldDataHelper.ChunkPositionFromBlockCoords(world, worldPosition);

        Assert.AreEqual(expectedChunkPosition, result);
    }

    [Test]
    public void GetChunkPositionsAroundPlayer_ReturnsCorrectChunkPositions()
    {
        Vector3Int playerPosition = new Vector3Int(0, 0, 0); 
        List<Vector3Int> expectedChunkPositions = new List<Vector3Int>
        {
            new Vector3Int(-16, 0, -16),
            new Vector3Int(-16, -100, -16),
            new Vector3Int(-16, -200, -16),
            new Vector3Int(-16, 0, 0),
            new Vector3Int(-16, -100, 0),
            new Vector3Int(-16, -200, 0),
            new Vector3Int(-16, 0, 16),
            new Vector3Int(-16, -100, 16),
            new Vector3Int(-16, -200, 16),
            new Vector3Int(0, 0, -16),
            new Vector3Int(0, -100, -16),
            new Vector3Int(0, -200, -16),
            new Vector3Int(0, 0, 0),
            new Vector3Int(0, -100, 0),
            new Vector3Int(0, -200, 0),
            new Vector3Int(0, 0, 16),
            new Vector3Int(0, -100, 16),
            new Vector3Int(0, -200, 16),
            new Vector3Int(16, 0, -16),
            new Vector3Int(16, -100, -16),
            new Vector3Int(16, -200, -16),
            new Vector3Int(16, 0, 0),
            new Vector3Int(16, -100, 0),
            new Vector3Int(16, -200, 0),
            new Vector3Int(16, 0, 16),
            new Vector3Int(16, -100, 16),
            new Vector3Int(16, -200, 16),
        };

        List<Vector3Int> result = WorldDataHelper.GetChunkPositionsAroundPlayer(world, playerPosition);

        Assert.AreEqual(expectedChunkPositions.Count, result.Count);
        Assert.IsTrue(result.All(pos => expectedChunkPositions.Contains(pos)));
    }
}
