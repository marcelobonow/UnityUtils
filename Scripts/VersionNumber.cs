using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VersionNumber : MonoBehaviour
{
    [SerializeField] private bool onlyTest;

    private void Start()
    {
        GetComponent<Text>().text = Application.version;
#if !DEBUG_MODE
        if(onlyTest)
            gameObject.SetActive(false);
#else
        gameObject.SetActive(true);
        GetComponent<Text>().text += " DEBUG";
        if(onlyTest)
            transform.localScale = 1.15f * Vector3.one;
#endif
    }
}
