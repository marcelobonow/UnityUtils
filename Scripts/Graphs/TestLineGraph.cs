using UnityEngine;

public class TestLineGraph : MonoBehaviour
{
    [SerializeField] private LineGraph lineGraph;
    [SerializeField] private float y;
    [SerializeField] private float jitter;

    private void Update()
    {
        lineGraph.AddPoint("test", Time.time, y + Random.Range(-jitter, jitter));
    }
}
