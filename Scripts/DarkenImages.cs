using UnityEngine;
using UnityEngine.UI;

public class DarkenImages : MonoBehaviour
{

    public SpriteRenderer[] images;

    public void GetMoreDarkImages()
    {
        GetMoreDarkImages(images, 0.7f);
    }

    public void GetMoreDarkImages(SpriteRenderer[] imagesToDark, float whiteFactor)
    {
        foreach (SpriteRenderer image in imagesToDark)
        {
            image.color *= whiteFactor;
        }
    }

}
