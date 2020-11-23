using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatTabAnimation : MonoBehaviour
{

    [SerializeField] float animationTime;
    [Tooltip("Deve ser setado previamente para o código saber se a esta habilitado ou não")]
    [SerializeField] protected bool isUp;
    [SerializeField] protected RectTransform downPosition;
    [SerializeField] protected RectTransform upPosition;
    public Image backgroundImage;


    public void RiseTab()
    {
        ///Se ja estava posicionado à cima, so habilita
        if (isUp)
            gameObject.SetActive(true);
        else
        {
            isUp = true;
            gameObject.SetActive(true);
            iTween.MoveTo(gameObject, iTween.Hash("position", upPosition.transform.position,
            "time", animationTime, "easeType", iTween.EaseType.easeInOutCubic));
        }
    }

    public void DownTab()
    {
        if (isUp)
        {
            isUp = false;
            iTween.MoveTo(gameObject, iTween.Hash("position", downPosition.transform.position,
            "time", animationTime, "easeType", iTween.EaseType.easeInOutCubic, "oncomplete", "OnDownTab"));
        }
    }
}
