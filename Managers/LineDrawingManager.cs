using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LineDrawingManager : Manager<LineDrawingManager>
{
    public LineDraw activeLineDraw;
    public GameObject lineDrawPrefab;
    public float linePoints, maxLinePoints, linePointsPerPoint;

    public delegate void LineDrawingFloatEvent(float number);
    public event LineDrawingFloatEvent LineDrawingPointsChanged;

    // Start is called before the first frame update
    void Start()
    {
        SceneManager.sceneLoaded += Reset;
        Reset(new Scene(), new LoadSceneMode());
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= Reset;
    }

    public void CreateNewLine()
    {
        activeLineDraw = Instantiate(lineDrawPrefab).GetComponent<LineDraw>();
        Vector2[] startingPoints = new Vector2[2];
        startingPoints[0] = new Vector3(20, 0, 0);
        startingPoints[1] = new Vector3(20, 1, 0);
        activeLineDraw.edgeCollider.points = startingPoints;
    }

    void AddNewPoint()
    {
        if(linePoints > 0)
        {
            activeLineDraw.AddLinePoint(MouseToWorldPosition());
            linePoints = linePoints - linePointsPerPoint;
            if(linePoints < 0)
            {
                linePoints = 0;
            }
            float linePointsPercentage = (linePoints / maxLinePoints);
            LineDrawingPointsChanged.Invoke(linePointsPercentage);
        }
    }

    //Get mouse position in world space
    Vector3 MouseToWorldPosition()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return mousePosition;
    }

    void Reset(Scene scene, LoadSceneMode loadSceneMode)
    {
        linePoints = maxLinePoints;
        LevelSetup levelSetup = FindObjectOfType<LevelSetup>();
        linePointsPerPoint = levelSetup.linePointsPerPoint;
        CreateNewLine();
        Manager<InputManager>.Instance.OnLeftClick += AddNewPoint;
        Manager<InputManager>.Instance.OnLeftMouseRelease += CreateNewLine;
    }
}
