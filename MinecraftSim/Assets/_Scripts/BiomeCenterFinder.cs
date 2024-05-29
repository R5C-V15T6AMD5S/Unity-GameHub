using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BiomeCenterFinder
{
    // Ovo je klasa koja pronalazi centre bioma oko svakog igrača

    public static List<Vector2Int> neighbours8Directions = new List<Vector2Int>
    {
        // Ova varijabla pomaže pri generaciji 8 točaka oko igrača

        new Vector2Int(0, 1),
        new Vector2Int(1, 1),
        new Vector2Int(1, 0),
        new Vector2Int(1, -1),
        new Vector2Int(0, -1),
        new Vector2Int(-1, -1),
        new Vector2Int(-1, 0),
        new Vector2Int(-1, 1)
    };

    public static List<Vector3Int> CalculateBiomeCenters(Vector3 playerPosition, int drawRange, int mapSize)
    {
        // Ova metoda izračunava centre bioma, playerPosition je zapravo centar svijeta, drawRange i mapSize su vrijednosti iz World klase

        // biomeLength je veličina samog bioma
        int biomeLength = drawRange * mapSize;

        // origin je pozicija igrača u biome mreži
        Vector3Int origin = new Vector3Int(Mathf.RoundToInt(playerPosition.x / biomeLength) * biomeLength, 0, Mathf.RoundToInt(playerPosition.z / biomeLength) * biomeLength);

        // Ova varijabla sadrži izračunate pozicije centara bioma i igrača 
        HashSet<Vector3Int> biomCentersTemp = new HashSet<Vector3Int>();

        biomCentersTemp.Add(origin);

        foreach (Vector2Int offsetXZ in neighbours8Directions)
        {
            // U ovoj foreach petlji se izračunavaju svih 8 pozicija oko igrača, ali isto tako i preostale pozicije oko izračunatih pozicija (drugi sloj pozicija)

            Vector3Int newBiomPoint_1 = new Vector3Int(origin.x + offsetXZ.x * biomeLength, 0, origin.z + offsetXZ.y * biomeLength);
            Vector3Int newBiomPoint_2 = new Vector3Int(origin.x + offsetXZ.x * biomeLength, 0, origin.z + offsetXZ.y * 2 * biomeLength);
            Vector3Int newBiomPoint_3 = new Vector3Int(origin.x + offsetXZ.x * 2 * biomeLength, 0, origin.z + offsetXZ.y * biomeLength);
            Vector3Int newBiomPoint_4 = new Vector3Int(origin.x + offsetXZ.x * 2 * biomeLength, 0, origin.z + offsetXZ.y * 2 * biomeLength);
            biomCentersTemp.Add(newBiomPoint_1);
            biomCentersTemp.Add(newBiomPoint_2);
            biomCentersTemp.Add(newBiomPoint_3);
            biomCentersTemp.Add(newBiomPoint_4);
        }

        
        return new List<Vector3Int>(biomCentersTemp);
    }
}
