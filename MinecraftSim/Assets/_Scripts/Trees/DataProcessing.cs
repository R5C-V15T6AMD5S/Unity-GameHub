using System;
using System.Collections.Generic;
using UnityEngine;

public static class DataProcessing
{
    // Klasa DataProcessing sadrži metode koje pronalaze pozicije stabala unutar proslijeđenih chunkova

    // directions varijabla služi za dobivanje pozicije susjeda
    public static List<Vector2Int> directions = new List<Vector2Int>
    {
        new Vector2Int( 0, 1 ),   // N
        new Vector2Int( 1, 1 ),   // NE
        new Vector2Int( 1, 0 ),   // E
        new Vector2Int( -1, 1 ),  // SE
        new Vector2Int( -1, 0 ),  // S
        new Vector2Int( -1, -1 ), // SW
        new Vector2Int( 0, -1 ),  // W
        new Vector2Int( 1, -1 )   // NW
    };

    public static List<Vector2Int> FindLocalMaxima(float[,] dataMatrix, int xCoord, int zCoord)
    {
        // Metoda FindLocalMaxima nalazi lokalne ekstreme unutar 2D polja dataMatrix i vraća Listu pozicija gdje se nalaze stabla

        // Lista maximas sadrži svjetske pozicije stabala
        List<Vector2Int> maximas = new List<Vector2Int>();

        for (int x = 0; x < dataMatrix.GetLength(0); x++)
        {
            for (int y = 0; y < dataMatrix.GetLength(1); y++)
            {
                float noiseVal = dataMatrix[x, y];
                if (CheckNeighbours(dataMatrix, x, y, (neighbourNoise) => neighbourNoise < noiseVal))
                {
                    maximas.Add(new Vector2Int(xCoord + x, zCoord + y));
                }
            }
        }
        return maximas;
    }

    private static bool CheckNeighbours(float[,] dataMatrix, int x, int y, Func<float, bool> successCondition)
    {
        /*
            Metoda CheckNeighbours provjerava za pojedinu vrijednost šuma, da li je ona lokalni ekstrem. Lokalni je ekstrem ukoliko ni jedan od susjeda nema veću vrijednost.
            Parametar dataMatrix označava generiranu vrijednost šuma za svaku poziciciju unutar chunka, x i z su svjetske koordinate chunka. Funkcija successCondition
            predstavlja uvjet koji mora vrijediti za svakog od susjeda kako bi promatrana pozicija bila lokalni maksimum.
        */


        foreach (var dir in directions)
        {

            // Pozicija susjeda
            var newPos = new Vector2Int(x + dir.x, y + dir.y);

            // Provjera nalazi li se pozicija u željenom rasponu unutar chunka
            if (newPos.x < 0 || newPos.x >= dataMatrix.GetLength(0) || newPos.y < 0 || newPos.y >= dataMatrix.GetLength(1))
            {
                continue;
            }

            // Ukoliko je vrijednost lokalnog maksimuma jednog od susjeda veća od vrijednosti lokalnog maksimuma trenutne pozicije, na trenutnoj poziciji neće biti postavljeno drveće
            if (successCondition(dataMatrix[x + dir.x, y + dir.y]) == false)
            {
                return false;
            }
        }

        // Pronađen je lokalni maksimum
        return true;
    }
}