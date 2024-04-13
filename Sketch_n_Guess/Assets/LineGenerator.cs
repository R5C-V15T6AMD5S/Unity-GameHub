using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineGenerator : MonoBehaviour
{

    // refereca na nas line prefab
    public GameObject linePrefab;

    // referenca na aktivnu liniju
    Line activeLine;

    void Update() {
        // kada kliknemo lijevi klik misem
        if (Input.GetMouseButtonDown(0)) {
            GameObject newLine = Instantiate(linePrefab);
            activeLine = newLine.GetComponent<Line>();
        }

        // kad otpustimo mis(zavrsimo s crtanjem linije)
        if(Input.GetMouseButtonUp(0)) {
            activeLine = null;
        }

        if(activeLine != null) {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            activeLine.updatePosition(mousePos);
        }
    }
}
