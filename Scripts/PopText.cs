using CustomGenericText;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PopText : MonoBehaviour
{
    [SerializeField] private GameObject textToPopPrefab;
    [SerializeField] private Canvas canvas;
    [SerializeField] private float riseAmount;

    private GameObject textGameObject;

    public IEnumerator DoPopText(string message, float animationDuration, RectTransform initialPosition = null)
    {
        if (initialPosition == null)
            initialPosition = GetComponent<RectTransform>();
        textGameObject = Instantiate(textToPopPrefab, canvas.transform);
        textGameObject.SetActive(true);
        textGameObject.transform.position = new Vector2(initialPosition.position.x, initialPosition.position.y);
        var textToPop = textGameObject.GetComponent<GenericText>();
        textToPop.SetText(message);
        ///The text must be destroyed after changing scene, unless it is don't destroy on load
        SceneManager.sceneLoaded += OnSceneLoaded;
        yield return StartCoroutine(MovePopText(textGameObject, animationDuration));
        DestroyText();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) => DestroyText();
    private void DestroyText()
    {
        if (textGameObject)
            Destroy(textGameObject);
    }

    private IEnumerator MovePopText(GameObject text, float animationDuration)
    {

        var initialTime = Time.time;
        var originalPosition = text.transform.position.y;
        while (Time.time < initialTime + animationDuration)
        {
            var step = (Time.time - initialTime) / animationDuration;
            var currentPosition = Mathf.Lerp(originalPosition, originalPosition + riseAmount, step);
            ///Sobe sua posição em até 1 no final da animação, em um segundo
            text.transform.position = new Vector2(text.transform.position.x, currentPosition);
            yield return new WaitForEndOfFrame();
        }
        text.transform.position = new Vector2(text.transform.position.x, originalPosition + riseAmount);
    }
}
