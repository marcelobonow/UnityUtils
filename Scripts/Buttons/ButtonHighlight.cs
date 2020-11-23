using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[SerializeField]
public class ButtonHighlight : MonoBehaviour
{
    //[SerializeField]
    //private Button button;
    [SerializeField]
    private Image highlightedImage;
    [SerializeField]
    private Image normalImage;

    public void HighlightButton()
    {
        normalImage.gameObject.SetActive(false);
        highlightedImage.gameObject.SetActive(true);
        //button.targetGraphic = highlightedImage;
    }
    public void UnHighlightButton()
    {
        normalImage.gameObject.SetActive(true);
        highlightedImage.gameObject.SetActive(false);
        //button.targetGraphic = normalImage;
    }
}
