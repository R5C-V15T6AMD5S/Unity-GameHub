using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineGenerator : MonoBehaviour 
{
    // refereca na nas line prefab
    public GameObject linePrefab;

    // referenca na aktivnu liniju
    Line activeLine;

    // referenca na aktivni canvas
    RectTransform canvasRect;

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

    // funkcija za limitiranje linije u samom canvasu
    /*Vector2 GetClampedMousePosition() {
        Vector2 mousePos = Input.mousePosition;
        Vector2 clampedMousePos = new Vector2(
            Mathf.Clamp(mousePos.x, canvasRect.position.x - canvasRect.sizeDelta.x / 2f, canvasRect.position.x + canvasRect.sizeDelta.x / 2f),
            Mathf.Clamp(mousePos.y, canvasRect.position.y - canvasRect.sizeDelta.y / 2f, canvasRect.position.y + canvasRect.sizeDelta.y / 2f)
        );
        return clampedMousePos;
    }*/
}