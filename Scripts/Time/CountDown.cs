using CustomGenericText;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CounterDown
{

    public enum Format { Timestamp, Seconds }

    private float startTime;
    private float totalSeconds; ///Tempo que serve como total para fazer o fill
    private double timeRemaning;
    private Coroutine counterCoroutine;
    private GenericText textToCount;
    private Image imageToFill;
    private Action<double> updateListener;
    private Action onCounterEnds;

    private Format format;

    public CounterDown SetText(GenericText text)
    {
        textToCount = text;
        updateListener += UpdateText;
        return this;
    }
    public CounterDown SetImage(Image image, float totalSeconds)
    {
        imageToFill = image;
        this.totalSeconds = totalSeconds;
        updateListener += UpdateFill;
        return this;
    }
    public CounterDown Start(MonoBehaviour monoObject, TimeSpan originalTime, Action onCounterEnds = null, Format format = Format.Timestamp)
    {
        this.format = format;
        if(monoObject.gameObject.activeInHierarchy)
        {
            startTime = (float)Math.Round(Time.time);
            counterCoroutine = monoObject.StartCoroutine(StartCountDown(originalTime, onCounterEnds));
        }
        return this;
    }
    public void StopCountdown(MonoBehaviour monoObject)
    {
        if(counterCoroutine != null)
            monoObject.StopCoroutine(counterCoroutine);
    }
    public void UpdateData(MonoBehaviour monoObject, TimeSpan originalTime, Action onCounterEnds = null)
    {
        if(counterCoroutine != null)
            monoObject.StopCoroutine(counterCoroutine);

        if(monoObject.gameObject.activeInHierarchy)
            counterCoroutine = monoObject.StartCoroutine(StartCountDown(originalTime, onCounterEnds));
    }

    private IEnumerator StartCountDown(TimeSpan originalTime, Action onCounterEnds = null)
    {
        this.onCounterEnds = onCounterEnds;
        timeRemaning = originalTime.TotalSeconds;
        while(timeRemaning >= 0)
        {
            try
            {
                updateListener(timeRemaning);
            }
            catch(Exception exception)
            {
                Logger.LogError("Exceção ocorreu ao atualizar contador: " + exception.ToString());
            }
            yield return new WaitForEndOfFrame();
            timeRemaning = originalTime.TotalSeconds - Math.Ceiling(Time.time - startTime);
        }
        Logger.LogWarning("Terminou uma contagem");
        if(this.onCounterEnds != null)
            this.onCounterEnds();
    }
    private void UpdateText(double timeRemaning)
    {
        switch(format)
        {
            case Format.Timestamp:
                textToCount.SetText(TimeSpan.FromSeconds(timeRemaning).GetTimeString());
                break;
            case Format.Seconds:
                textToCount.SetText(TimeSpan.FromSeconds(timeRemaning).GetSecondsString());
                break;
            default:
                textToCount.SetText(TimeSpan.FromSeconds(timeRemaning).GetTimeString());
                break;
        }

    }
    private void UpdateFill(double timeRemaning)
    {
        var percentage = 1 - (float)timeRemaning / totalSeconds;
        imageToFill.fillAmount = percentage;
    }
}
