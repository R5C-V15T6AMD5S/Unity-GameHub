using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

// Klasa za glavni upravitelj igre, služi za stvaranje svijeta i igraèa
public class GameManager : MonoBehaviour
{
    // Definiranje Prefab-a igraèa
    public GameObject playerPrefab;
    private GameObject player;
    // Definiranje trenutne pozicije igraèa u chunk-u
    public Vector3Int currentPlayerChunkPosition;
    private Vector3Int currentChunkCenter = Vector3Int.zero;
    // Referenca na svijet
    public World world;

    // Vrijeme za detekciju promjene pozicije igraèa
    public float detectionTime = 1;
    // Virtualna kamera koja  slijedi igraèa
    public CinemachineVirtualCamera camera_VM;

    // Metoda za stvaranje igraèa na poèetku igre
    public void SpawnPlayer()
    {
        // Provjera postoji li igraè, ukoliko postoji prekidanje metode
        if (player != null) return;
        // Stvara se odreðena toèka za raycast prema dolje kako bi se postavio igraè na tlo
        Vector3Int raycastStartposition = new Vector3Int(world.chunkSize / 2, 100, world.chunkSize / 2);
        RaycastHit hit;
        // Provjera gdje zraka udara tlo
        if (Physics.Raycast(raycastStartposition, Vector3.down, out hit, 120))
        {
            // Stvaranje igraèa na mjestu gdje zraka udara u tlo
            player = Instantiate(playerPrefab, hit.point + Vector3Int.up, Quaternion.identity);
            // Postavljanje kamere da slijedi igraèa
            camera_VM.Follow = player.transform.GetChild(0);
            StartCheckingTheMap();
        }
    }
    // Metoda koja zapoèinje provjeru pozicije igraèa na karti
    public void StartCheckingTheMap()
    {
        SetCurrentChunkCoordinates();

        // Zaustavljanje svih postojeæih korutina i pokretanje nove korutine za provjeru
        StopAllCoroutines();
        StartCoroutine(CheckIfShouldLoadNextPosition());
    }

    // Korutina koja provjerava treba li uèitati sljedeæu poziciju na karti
    IEnumerator CheckIfShouldLoadNextPosition()
    {
        // Èekanje odreðenog vremena prije sljedeæe provjere
        yield return new WaitForSeconds(detectionTime);
        // Provjera udaljenosti izmeðu igraèa i središta chunk-a
        if (
            Mathf.Abs(currentChunkCenter.x - player.transform.position.x) > world.chunkSize ||
            Mathf.Abs(currentChunkCenter.z - player.transform.position.z) > world.chunkSize ||
            (Mathf.Abs(currentPlayerChunkPosition.y - player.transform.position.y) > world.chunkHeight)
            )
        {
            // Ako je udaljenost prevelika, traži se uèitavanje dodatnih chunk-ova
            world.LoadAdditionalChunksRequest(player);
        }
        else
        {
            // Ako nije, ponovno pokreæe korutinu za provjeru
            StartCoroutine(CheckIfShouldLoadNextPosition());
        }
    }
    // Metoda koja postavlja trenutne koordinate chunk-a
    private void SetCurrentChunkCoordinates()
    {
        // Odreðuje se trenutna pozicija igraèa u chunk-u
        currentPlayerChunkPosition = WorldDataHelper.ChunkPositionFromBlockCoords(world, Vector3Int.RoundToInt(player.transform.position));
        // Odreðuje se središte chunk-a na temelju trenutne pozicije igraèa
        currentChunkCenter.x = currentPlayerChunkPosition.x + world.chunkSize / 2;
        currentChunkCenter.z = currentPlayerChunkPosition.z + world.chunkSize / 2;
    }
}
