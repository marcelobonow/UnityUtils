using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingCircleAnimation : MonoBehaviour
{
    [Tooltip("Quantos segundos irá durar a animação de uma volta")]
    [SerializeField]
    float duration = 1;
    private void Update()
    {
        transform.Rotate(0, 0, -360 * Time.deltaTime * duration);
    }
}
