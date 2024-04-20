using System.Collections.Generic;
using UnityEngine;

public class MeshData
{
    // MeshData sprema vrhove, trokute i UV koordinate (koordinate tekstura) za generaciju mesha

    // Lista vrhova - varijabilan broj točaka na chunkovima
    public List<Vector3> vertices = new List<Vector3>();
    public List<int> triangles = new List<int>();

    // Koordinate tekstura koje se postavljaju na stranama meshova
    public List<Vector2> uv = new List<Vector2>();
    
    // Collider vrhovi koji omogućuju detekciju kolizija. Npr. voda i zrak ne bi trebali blokirati kretanje igrača, dok bi ostali blokovi trebali. 
    public List<Vector3> colliderVertices = new List<Vector3>();
    public List<int> colliderTriangles = new List<int>();

    // Različiti MeshData za vodu, s obzirom da koristi drugačiji materijal koji je transparentan
    public MeshData waterMesh;

    // Pomoć za konstruktor
    private bool isMainMesh = true;


    public MeshData(bool isMainMesh) {
        // Konstruktor inicijalizira MeshData

        if(isMainMesh) {
            waterMesh = new MeshData(false);
        }
    }

    public void AddVertex(Vector3 vertex, bool vertexGeneratesCollider) {
        //Dodavanje vrhova za mesh. Generira se collider vrh ukoliko blok nije voda ili zrak.

        vertices.Add(vertex);
        if (vertexGeneratesCollider) {
            colliderVertices.Add(vertex);
        }
    }

    public void AddQuadTriangles(bool quadGeneratesCollider) {
        /*
            Dodaju se trokuti koji će sačinjavati kvadrate. Kvadrati su sačinjeni od 2 trokuta, te treba paziti na poredak zapisivanja 
            točaka kako bi ispravna strana bila vidljiva (s obzirom da materijal renderira samo prednje strane, odnosno ono što igrač vidi).
        */

        // Dodavanje točaka trokuta za sačinjavanje kvadrata
        triangles.Add(vertices.Count - 4);
        triangles.Add(vertices.Count - 3);
        triangles.Add(vertices.Count - 2);

        triangles.Add(vertices.Count - 4);
        triangles.Add(vertices.Count - 2);
        triangles.Add(vertices.Count - 1);

        // Dodavanje točaka kolizijskih trokuta
        if(quadGeneratesCollider) {
            colliderTriangles.Add(colliderVertices.Count - 4);
            colliderTriangles.Add(colliderVertices.Count - 3);
            colliderTriangles.Add(colliderVertices.Count - 2);
            colliderTriangles.Add(colliderVertices.Count - 4);
            colliderTriangles.Add(colliderVertices.Count - 2);
            colliderTriangles.Add(colliderVertices.Count - 1);
        }
    }
}
