using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineGenerator : MonoBehaviour 
{
    // refereca na nas line prefab
    public GameObject linePrefab;

    // referenca na aktivnu liniju
    private Line activeLine;

    // referenca na aktivni canvas
    [SerializeField] private RectTransform canvasRect;

    void Start() {
        // uzima RectTransform komponentu od canvasa
        canvasRect = GetComponent<RectTransform>();
    }

    void Update() {
        // kada kliknemo lijevi klik misem crtanje
        if (Input.GetMouseButtonDown(0)) {
            GameObject newLine = Instantiate(linePrefab);
            activeLine = newLine.GetComponent<Line>();
        }

        // kad otpustimo mis(zavrsimo s crtanjem linije)
        if(Input.GetMouseButtonUp(0)) {
            activeLine = null;
        }

        // ako activeLine nije null želimo dobiti poziciju miša koju proslijeđujemo updatePosition funkciji
        if(activeLine != null) {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //Vector2 mousePos = GetClampedMousePosition();
            activeLine.updatePosition(mousePos);
        }
    }
}