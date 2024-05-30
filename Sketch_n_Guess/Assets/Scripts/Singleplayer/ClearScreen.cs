using UnityEngine;
using UnityEngine.UI;

public class ClearScreen : MonoBehaviour
{
    // inicijalizacija gumba
    private Button clearButton;

    private void Start()
    {
        // Pronalaženje reference na gumb
        clearButton = GetComponent<Button>();

        // Dodavanje funkcionalnosti gumbu
        clearButton.onClick.AddListener(ClearAllLines);
    }

    // Metoda za brisanje svih linija
    private void ClearAllLines()
    {
        // Pronalaženje svih objekata s linijama u sceni
        Line[] lines = FindObjectsOfType<Line>();

        // Brisanje svake linije
        foreach (Line line in lines) {
            Destroy(line.gameObject);
        }
    }
}
