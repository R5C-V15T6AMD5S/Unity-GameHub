using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Recognizer
{
    // Calculates the centroid of a set of points
    // The centroid is the average position of all points in the given array
    public Vector2 GetCentroid(DollarPoint[] points)
    {
        float centerX = points.Sum(point => point.Point.x) / points.Length;
        float centerY = points.Sum(point => point.Point.y) / points.Length;
        return new Vector2(centerX, centerY);
    }

    // Resamples a set of points to a specified number of points
    // This helps in normalizing the number of points for better comparison between gestures
    public DollarPoint[] ResamplePoints(DollarPoint[] points, int n)
    {
        float incrementValue = PathLength(points) / (n - 1);
        float proceedDistance = 0;
        List<DollarPoint> newPoints = new List<DollarPoint> { points[0] };

        for (int i = 1; i < points.Length; i++)
        {
            DollarPoint previousDollarPoint = points[i - 1];
            DollarPoint currentDollarPoint = points[i];

            if (previousDollarPoint.StrokeIndex == currentDollarPoint.StrokeIndex)
            {
                float distance = Vector2.Distance(previousDollarPoint.Point, currentDollarPoint.Point);

                if (proceedDistance + distance >= incrementValue)
                {
                    while (proceedDistance + distance >= incrementValue)
                    {
                        float t = Math.Min(Math.Max((incrementValue - proceedDistance) / distance, 0.0f), 1.0f);
                        if (float.IsNaN(t)) t = 0.5f;

                        float approximatedX = previousDollarPoint.Point.x + t * (currentDollarPoint.Point.x - previousDollarPoint.Point.x);
                        float approximatedY = previousDollarPoint.Point.y + t * (currentDollarPoint.Point.y - previousDollarPoint.Point.y);

                        DollarPoint approximatedDollarPoint = new DollarPoint()
                        {
                            Point = new Vector2(approximatedX, approximatedY),
                            StrokeIndex = previousDollarPoint.StrokeIndex
                        };

                        newPoints.Add(approximatedDollarPoint);

                        distance = proceedDistance + distance - incrementValue;
                        proceedDistance = 0;
                        previousDollarPoint = newPoints[newPoints.Count - 1];
                    }

                    proceedDistance = distance;
                }
                else
                {
                    proceedDistance += distance;
                }
            }
        }

        if (proceedDistance > 0)
        {
            newPoints.Add(points[points.Length - 1]);
        }

        return newPoints.ToArray();
    }

    // Calculates the total path length of a set of points
    // Path length is the sum of the distances between consecutive points
    public float PathLength(DollarPoint[] points)
    {
        float length = 0;

        for (int i = 1; i < points.Length; i++)
        {
            DollarPoint previous = points[i - 1];
            DollarPoint current = points[i];
            float distance = Vector2.Distance(previous.Point, current.Point);

            if (!float.IsNaN(distance) && previous.StrokeIndex == current.StrokeIndex)
            {
                length += distance;
            }
        }

        return length;
    }
}
