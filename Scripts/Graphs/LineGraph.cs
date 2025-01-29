using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LineGraph : MonoBehaviour
{
    ///TODO: Criar os line renderes dinamicamente colocando no directonary
    [SerializeField] LineRenderer lineRenderPrefab;

    [SerializeField] private TextMeshProUGUI maxYText;
    [SerializeField] private TextMeshProUGUI minYText;

    [SerializeField] private Vector2 scale;
    [SerializeField] private int maxPoints;


    public Dictionary<string, LineRenderer> lineRenderers = new();
    public Dictionary<string, List<Vector2>> points = new();


    private Vector2 boundX = new(Mathf.Infinity, Mathf.NegativeInfinity);
    private Vector2 boundY = new(Mathf.Infinity, Mathf.NegativeInfinity);

    public void SetupLineColor(string category, Color colorStart, Color colorEnd)
    {
        if (!lineRenderers.ContainsKey(category))
        {
            var lineRenderer = Instantiate(lineRenderPrefab, transform);
            lineRenderers.Add(category, lineRenderer);
        }

        lineRenderers[category].startColor = colorStart;
        lineRenderers[category].endColor = colorEnd;
    }

    public void AddPoint(string category, float x, float y)
    {
        LineRenderer lineRenderer = null;
        if (!points.ContainsKey(category))
            points.Add(category, new List<Vector2>());


        if (!lineRenderers.ContainsKey(category))
        {
            lineRenderer = Instantiate(lineRenderPrefab, transform);
            lineRenderers.Add(category, lineRenderer);
        }
        else
            lineRenderer = lineRenderers[category];

        points[category].Add(new Vector2(x, y));

        var pointsToRemove = points[category].Count - maxPoints;
        if (pointsToRemove > 0)
        {
            points[category].RemoveRange(0, pointsToRemove);
            UpdateBounds(category);
        }
        else
        {
            if (x < boundX.x)
                boundX.x = x;
            if (x > boundX.y)
                boundX.y = x;
            if (y < boundY.x)
                boundY.x = y;
            if (y > boundY.y)
                boundY.y = y;

            maxYText.text = boundY.y.ToString("n2");
            minYText.text = boundY.x.ToString("n2");
        }


        UpdateGraph(points[category], lineRenderer);
    }

    public void UpdateGraph(List<Vector2> points, LineRenderer lineRenderer)
    {
        var pointsToPrint = new Vector3[points.Count * 2 - 1];
        var lastPoint = Vector3.zero;
        var limitsSize = new Vector2(boundX.y - boundX.x, boundY.y - boundY.x);

        var position = new Vector2(transform.position.x, transform.position.y);

        if (points.Count < 2)
            return;

        if (points.Count < maxPoints)
        {
            var averageTime = (points[^1].x - points[0].x) / points.Count;
            limitsSize.x = averageTime * maxPoints + boundX.x;
        }

        for (int i = 0; i < pointsToPrint.Length; i += 2)
        {
            Vector3 pointNormalized = (((Vector3)points[i / 2] - new Vector3(boundX.x, boundY.x)) / limitsSize) * scale + position;
            pointNormalized.z = transform.position.z;

            if (i > 0)
                pointsToPrint[i - 1] = new Vector3(lastPoint.x, pointNormalized.y, transform.position.z);
            pointsToPrint[i] = pointNormalized;
            lastPoint = pointNormalized;
        }

        lineRenderer.positionCount = pointsToPrint.Length;
        ///TODO: Pegar novo minimo e maximo e renormalizar os pontos baseado nisso
        lineRenderer.SetPositions(pointsToPrint);
    }

    private void UpdateBounds(string category)
    {
        if (!points.ContainsKey(category))
            return;

        boundX = new(Mathf.Infinity, Mathf.NegativeInfinity);
        boundY = new(Mathf.Infinity, Mathf.NegativeInfinity);

        foreach (var points in points.Values)
        {
            foreach (var point in points)
            {
                var x = point.x;
                var y = point.y;

                if (x < boundX.x)
                    boundX.x = x;
                if (x > boundX.y)
                    boundX.y = x;
                if (y < boundY.x)
                    boundY.x = y;
                if (y > boundY.y)
                    boundY.y = y;
            }

        }
        maxYText.text = boundY.y.ToString("n2");
        minYText.text = boundY.x.ToString("n2");
    }
}
