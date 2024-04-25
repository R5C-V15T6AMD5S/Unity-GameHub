using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawWithMouse : MonoBehaviour
{
    private LineRenderer line;
    private Vector3 previousPosition;

    [SerializeField] private float minDistance = 0.1f;

    private void Start() {
        line = GetComponent<LineRenderer>();
        //line.gameObject.layer = LayerMask.NameToLayer("Background");
        previousPosition = transform.position;
    }

    // While the mouse is pressed, it draws
    // When released, the content gets deleted
    private void Update() {
        if(Input.GetMouseButton(0)) { Draw(); }
        else if(Input.GetMouseButtonUp(0)) { line.positionCount = 0; }
    }

    private void Draw() {
        Vector3 currentPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        currentPosition.z = 0f;

        if (Vector3.Distance(currentPosition, previousPosition) > minDistance) {
            line.positionCount++;
            line.SetPosition(line.positionCount - 1, currentPosition);
            previousPosition = currentPosition;
        }
    }
}
