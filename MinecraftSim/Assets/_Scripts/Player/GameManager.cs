using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

// Klasa za glavni upravitelj igre, slu�i za stvaranje svijeta i igra�a
public class GameManager : MonoBehaviour
{
    // Definiranje Prefab-a igra�a
    public GameObject playerPrefab;
    private GameObject player;
    // Definiranje trenutne pozicije igra�a u chunk-u
    public Vector3Int currentPlayerChunkPosition;
    private Vector3Int currentChunkCenter = Vector3Int.zero;
    // Referenca na svijet
    public World world;

    // Vrijeme za detekciju promjene pozicije igra�a
    public float detectionTime = 1;
    // Virtualna kamera koja  slijedi igra�a
    public CinemachineVirtualCamera camera_VM;

    // Metoda za stvaranje igra�a na po�etku igre
    public void SpawnPlayer()
    {
        // Provjera postoji li igra�, ukoliko postoji prekidanje metode
        if (player != null) return;
        // Stvara se odre�ena to�ka za raycast prema dolje kako bi se postavio igra� na tlo
        Vector3Int raycastStartposition = new Vector3Int(world.chunkSize / 2, 100, world.chunkSize / 2);
        RaycastHit hit;
        // Provjera gdje zraka udara tlo
        if (Physics.Raycast(raycastStartposition, Vector3.down, out hit, 120))
        {
            // Stvaranje igra�a na mjestu gdje zraka udara u tlo
            player = Instantiate(playerPrefab, hit.point + Vector3Int.up, Quaternion.identity);
            // Postavljanje kamere da slijedi igra�a
            camera_VM.Follow = player.transform.GetChild(0);
            StartCheckingTheMap();
        }
    }
    // Metoda koja zapo�inje provjeru pozicije igra�a na karti
    public void StartCheckingTheMap()
    {
        SetCurrentChunkCoordinates();

        // Zaustavljanje svih postoje�ih korutina i pokretanje nove korutine za provjeru
        StopAllCoroutines();
        StartCoroutine(CheckIfShouldLoadNextPosition());
    }

    // Korutina koja provjerava treba li u�itati sljede�u poziciju na karti
    IEnumerator CheckIfShouldLoadNextPosition()
    {
        // �ekanje odre�enog vremena prije sljede�e provjere
        yield return new WaitForSeconds(detectionTime);
        // Provjera udaljenosti izme�u igra�a i sredi�ta chunk-a
        if (
            Mathf.Abs(currentChunkCenter.x - player.transform.position.x) > world.chunkSize ||
            Mathf.Abs(currentChunkCenter.z - player.transform.position.z) > world.chunkSize ||
            (Mathf.Abs(currentPlayerChunkPosition.y - player.transform.position.y) > world.chunkHeight)
            )
        {
            // Ako je udaljenost prevelika, tra�i se u�itavanje dodatnih chunk-ova
            world.LoadAdditionalChunksRequest(player);
        }
        else
        {
            // Ako nije, ponovno pokre�e korutinu za provjeru
            StartCoroutine(CheckIfShouldLoadNextPosition());
        }
    }
    // Metoda koja postavlja trenutne koordinate chunk-a
    private void SetCurrentChunkCoordinates()
    {
        // Odre�uje se trenutna pozicija igra�a u chunk-u
        currentPlayerChunkPosition = WorldDataHelper.ChunkPositionFromBlockCoords(world, Vector3Int.RoundToInt(player.transform.position));
        // Odre�uje se sredi�te chunk-a na temelju trenutne pozicije igra�a
        currentChunkCenter.x = currentPlayerChunkPosition.x + world.chunkSize / 2;
        currentChunkCenter.z = currentPlayerChunkPosition.z + world.chunkSize / 2;
    }
}
