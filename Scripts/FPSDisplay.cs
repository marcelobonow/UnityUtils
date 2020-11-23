using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class FPSDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI fpsValue;
    [SerializeField] private TextMeshProUGUI averageValue;
    [SerializeField] private TextMeshProUGUI tenPercentValue;
    [SerializeField] private TextMeshProUGUI onePercentValue;

    private List<float> allFrameTimes;
    private float lastLowerValuesUpdate;
    private float lastFpsUpdate;

    private void Awake()
    {
        allFrameTimes = new List<float>(4096);
        lastLowerValuesUpdate = 0;
    }

    private void Update()
    {
        ///Para não sujar os dados quando a cena não tiver inicializada
        if (Time.time < 1f)
            return;


        if (Time.unscaledTime > lastFpsUpdate + 0.25f)
        {
            lastFpsUpdate = Time.unscaledTime;
            fpsValue.text = (1 / Time.unscaledDeltaTime).ToString("00.0");
        }



        allFrameTimes.Add(Time.unscaledDeltaTime);
        if (Time.unscaledTime > lastLowerValuesUpdate + 1)
        {
            lastLowerValuesUpdate = Time.unscaledTime;
            allFrameTimes = allFrameTimes.OrderByDescending(x => x).ToList();
            var tenPercentFrameTimes = allFrameTimes.Take(Mathf.CeilToInt(allFrameTimes.Count / 10f)).ToList();
            var onePercentFrameTimes = allFrameTimes.Take(Mathf.CeilToInt(allFrameTimes.Count / 100f)).ToList();
            averageValue.text = GetMean(allFrameTimes).ToString("00.0");
            tenPercentValue.text = GetMean(tenPercentFrameTimes).ToString("00.0");
            onePercentValue.text = GetMean(onePercentFrameTimes).ToString("00.0");
        }
    }

    private float GetMean(List<float> values) => values.Count / values.Sum();
}
