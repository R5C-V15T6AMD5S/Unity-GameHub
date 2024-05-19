using UnityEngine;

public class ChunkData 
{   
    // ChunkData klasa sadržava podatke o pojedinom chunku

    // Chunk je 3D objekt (visina, širina i duljina), sve reference blokova koje su unutar chunka spremamo u 1D polju naziva blocks
    public BlockType[] blocks;

    public int chunkSize = 16;
    public int chunkHeight = 100;

    // Referenca na svijet koji sadržava chunk
    public World worldReference;

    /* Referenca na poziciju u svijetu gdje je chunk postavljen, bitno zbog pretvorbe worldPosition točke u lokalni koordinatni sustav chunka za pristup bloku -
    zbog razbijanja/postavljanja blokova u chunku
    */
    public Vector3Int worldPosition;

    // U proceduralno generiranom svijetu, lako se mogu regenerirati chunkovi, međutim u slučaju da igrač napravi izmjene na chunku, spremamo podatke o chunkovima koji su izmijenjeni
    public bool modifiedByThePlayer = false;

    // Podaci o stablima unutar chunka
    public TreeData treeData;

    public ChunkData(int chunkSize, int chunkHeight, World world, Vector3Int worldPosition) {
        this.chunkHeight = chunkHeight;
        this.chunkSize = chunkSize;
        this.worldReference = world;
        this.worldPosition = worldPosition;
        blocks = new BlockType[chunkSize * chunkHeight * chunkSize];
    }
}
