using UnityEngine;

public static class BlockHelper
{
    // Pomoću BlockHelper klase će se ispuniti podaci iz ChunkData u MeshData stvaranjem vrhova i trokuta ovisno o voxelu koji se analizira

    // directions se koristi unutar foreach petlje kako bi se izvršio rad nad voxelima prilikom analize ChunkData
    private static Direction[] directions = 
    {
        Direction.backwards,
        Direction.down,
        Direction.forward,
        Direction.left,
        Direction.right,
        Direction.up
    };

    public static MeshData GetMeshData(ChunkData chunk, int x, int y, int z, MeshData meshData, BlockType blockType)
    {
        // Metoda GetMeshData vraća meshData za pojedini chunk, generira potrebne strane svakog voxela

        // Ukoliko su blokovi vrste "zrak" ili "ništa" to se neće renderirati
        if (blockType == BlockType.Air || blockType == BlockType.Nothing) return meshData;

        foreach (Direction direction in directions)
        {
        // Provjerava se treba li se generirati strana voxela u pojedinom smjeru        

            var neighbourBlockCoordinates = new Vector3Int(x, y, z) + direction.GetVector();
            var neighbourBlockType = Chunk.GetBlockFromChunkCoordinates(chunk, neighbourBlockCoordinates);

            if(ShouldRenderFace(neighbourBlockType, blockType))
            {
                meshData = GetFaceData(neighbourBlockType, direction, chunk, x, y, z, meshData, blockType);
            }
        }
        return meshData;
    }

    private static bool ShouldRenderFace(BlockType neighbourBlockType, BlockType blockType)
    {
        // ShouldRenderFace je metoda koja prima određenu stranu te provjerava treba li se ispuniti mesh podacima

        return neighbourBlockType != BlockType.Nothing && 
            !BlockDataManager.blockTextureDataDictionary[neighbourBlockType].isSolid;
    }

    private static MeshData GetFaceData(BlockType neighbourBlockType, Direction direction, ChunkData chunk, int x, int y, int z, MeshData meshData, BlockType blockType)
    {
        // GetFaceData metoda poziva metode koje ispunjuju meshData sa podacima ovisno o tome koji je trenutni tip bloka (npr. iznimka je voda, puni se waterMesh)
        
        if (blockType == BlockType.Water)
        {
            // Pojedina strana vode se treba renderirati ukoliko 'dira' zrak
            if (neighbourBlockType == BlockType.Air)
            meshData.waterMesh = GetFaceDataIn(direction, chunk, x, y, z, meshData.waterMesh, blockType);
        }
        else
        {
            meshData = GetFaceDataIn(direction, chunk, x, y, z, meshData, blockType);
        }
        return meshData;
    }

    public static MeshData GetFaceDataIn(Direction direction, ChunkData chunk, int x, int y, int z, MeshData meshData, BlockType blockType)
    {
        // Metoda GetFaceDataIn ispunjuje meshData sa podacima (vrhovi, trokuti, koordinate tekstura) te vraća meshData

        GetFaceVertices(direction, x, y, z, meshData, blockType);
        meshData.AddQuadTriangles(BlockDataManager.blockTextureDataDictionary[blockType].generatesCollider);
        meshData.uv.AddRange(FaceUVs(direction, blockType));

        return meshData;
    }

    public static void GetFaceVertices(Direction direction, int x, int y, int z, MeshData meshData, BlockType blockType)
    {
        /* 
        Metoda koja dodaje vrhove po pojedinoj strani voxela. Parametri: direction => smjer u kojem se gleda strana,
        x, y, z => središnje točke voxela, meshData => MeshData koji se želi ispuniti vrhovima, blockType => tip voxela
        */

        var generatesCollider = BlockDataManager.blockTextureDataDictionary[blockType].generatesCollider;
        switch (direction)
        {
            // Dodaju se točke po pojedinoj strani (kvadratu) voxela

            case Direction.backwards:
                meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f), generatesCollider);
                meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f), generatesCollider);
                meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f), generatesCollider);
                meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f), generatesCollider);
                break;
            case Direction.forward:
                meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f), generatesCollider);
                meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f), generatesCollider);
                meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f), generatesCollider);
                meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f), generatesCollider);
                break;
            case Direction.left:
                meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f), generatesCollider);
                meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f), generatesCollider);
                meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f), generatesCollider);
                meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f), generatesCollider);
                break;
            case Direction.right:
                meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f), generatesCollider);
                meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f), generatesCollider);
                meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f), generatesCollider);
                meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f), generatesCollider);
                break;
            case Direction.down:
                meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f), generatesCollider);
                meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f), generatesCollider);
                meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f), generatesCollider);
                meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f), generatesCollider);
                break;
            case Direction.up:
                meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f), generatesCollider);
                meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f), generatesCollider);
                meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f), generatesCollider);
                meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f), generatesCollider);
                break;
            default:
                break;
        }
    }

    public static Vector2[] FaceUVs(Direction direction, BlockType blockType)
    { 
        // Metoda FaceUVs vraća koordinate tekstura za pojedinu stranu voxela

        // Svaka strana voxela ima 4 vrha, te za sva 4 vrha treba dodijeliti uv koordinate kako bi se oslikala korektna tekstura na svakoj strani
        Vector2[] UVs = new Vector2[4];
        var tilePos = TexturePosition(direction, blockType);

        // tileSize = 0.1, tilePos = indeks teksture, tekstureOffset = 0.001
        UVs[0] = new Vector2(BlockDataManager.tileSizeX * tilePos.x + BlockDataManager.tileSizeX - BlockDataManager.textureOffset,
        BlockDataManager.tileSizeY * tilePos.y + BlockDataManager.textureOffset);

        UVs[1] = new Vector2(BlockDataManager.tileSizeX * tilePos.x + BlockDataManager.tileSizeX - BlockDataManager.textureOffset,
        BlockDataManager.tileSizeY * tilePos.y + BlockDataManager.tileSizeY - BlockDataManager.textureOffset);

        UVs[2] = new Vector2(BlockDataManager.tileSizeX * tilePos.x + BlockDataManager.textureOffset,
        BlockDataManager.tileSizeY * tilePos.y + BlockDataManager.tileSizeY - BlockDataManager.textureOffset);

        UVs[3] = new Vector2(BlockDataManager.tileSizeX * tilePos.x + BlockDataManager.textureOffset,
        BlockDataManager.tileSizeY * tilePos.y + BlockDataManager.textureOffset);

        return UVs;
    }

    public static Vector2Int TexturePosition(Direction direction, BlockType blockType)
    {
        // Metoda TexturePosition vraća poziciju teksture

        return direction switch
        {
            Direction.up => BlockDataManager.blockTextureDataDictionary[blockType].up,
            Direction.down => BlockDataManager.blockTextureDataDictionary[blockType].down,
            _ => BlockDataManager.blockTextureDataDictionary[blockType].side
        };
    }
}
