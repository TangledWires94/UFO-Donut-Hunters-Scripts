using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDraw : MonoBehaviour
{
    public LineRenderer LR;
    public List<Vector3> linePoints = new List<Vector3>();
    public EdgeCollider2D edgeCollider;

    //Add mouse position to list of points then update the line to include that point
    public void AddLinePoint(Vector3 newPoint)
    {
        linePoints.Add(new Vector3(newPoint.x, newPoint.y, -0.1f));
        Vector3[] points = new Vector3[linePoints.Count];
        Vector2[] colPoints = new Vector2[linePoints.Count];
        for (int i = 0; i < linePoints.Count; i++)
        {
            points[i] = linePoints[i];
            colPoints[i] = linePoints[i];
        }
        LR.positionCount = linePoints.Count;
        LR.SetPositions(points);
        edgeCollider.points = colPoints;
    }

    //Clear the list of points and erase line
    public void ClearLine()
    {
        linePoints.Clear();
        Vector3[] points = new Vector3[1];
        Vector2[] colPoints = new Vector2[1];
        points[0] = Vector3.zero;
        colPoints[0] = Vector2.zero;
        LR.positionCount = 0;
        LR.SetPositions(points);
        edgeCollider.points = colPoints;
    }
}
