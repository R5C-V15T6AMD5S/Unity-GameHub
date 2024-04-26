using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Line : MonoBehaviour
{
    public LineRenderer lineRenderer; // line renderer objekt

    List<Vector2> points; 
    
    public void updatePosition(Vector2 position) {
        if (points == null) {
            points = new List<Vector2>();
            setPoint(position);
            return;
        }

        // provjera da ne dodajemo istu točku dva puta
        if(Vector2.Distance(points.Last(), position) > .1f) {
            setPoint(position);
        }
    }

    /*
        Dodajemo novu točku, ažuriramo line rendererov brojač i
        govorimo mu gdje je nova točka
    */
    void setPoint(Vector2 point) {
        points.Add(point);
        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPosition(points.Count - 1, point);
    }

}
