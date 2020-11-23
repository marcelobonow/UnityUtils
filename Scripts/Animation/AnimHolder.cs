using System;
using System.Collections;
using UnityEngine;

///Funciona como slider
public class AnimHolder : MonoBehaviour
{
    ///TODO: Passar para ser especifico por objeto (passar por parametro no metodo?)
    private static float speed = 800;

    public static IEnumerator AnimateHolder(bool toRight, GameObject holder, GameObject sliderBackGroundOn, GameObject sliderBackGroundOff, float maxHolderPosition,
        float minHolderPosition, Action onEnd = null)
    {
        var rectTransform = holder.GetComponent<RectTransform>();
        if(toRight)
        {
            while((rectTransform.localPosition.x + speed * Time.deltaTime) <= maxHolderPosition)
            {
                var step = speed * Time.deltaTime;
                rectTransform.localPosition += Vector3.right * step;
                yield return new WaitForEndOfFrame();
            }
            rectTransform.localPosition = new Vector3(maxHolderPosition, rectTransform.localPosition.y);
        }
        else
        {
            while((rectTransform.localPosition.x - speed * Time.deltaTime) >= minHolderPosition)
            {
                var step = speed * Time.deltaTime;
                rectTransform.localPosition += Vector3.left * step;
                yield return new WaitForEndOfFrame();
            }
            rectTransform.localPosition = new Vector3(minHolderPosition, rectTransform.localPosition.y);
        }
        SetState(toRight, holder, sliderBackGroundOn, sliderBackGroundOff, maxHolderPosition, minHolderPosition, rectTransform);
        if(onEnd != null)
            onEnd();
    }

    ///É usado para colocar o holder direto na posição final, é utilizado quando o popup que possui o holder se fecha no menu de configuração de áudio
    public static void SetState(bool state, GameObject holder, GameObject sliderBackGroundOn, GameObject sliderBackGroundOff, float maxHolderPosition, float minHolderPosition, RectTransform rectTransform)
    {
        rectTransform.localPosition = state ? new Vector3(maxHolderPosition, rectTransform.localPosition.y) :
           rectTransform.localPosition = new Vector3(minHolderPosition, rectTransform.localPosition.y);

        sliderBackGroundOn.SetActive(state);
        sliderBackGroundOff.SetActive(!state);
    }
}
