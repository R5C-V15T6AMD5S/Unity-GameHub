using UnityEngine;

public class ExperienceData : MonoBehaviour
{
    public int amount = 5;
    
    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter2D(Collider2D collider)  //when player collides with exp exp is added up
    {
        if (!collider.CompareTag("Player")) return;
        Destroy(gameObject);
    }
}
