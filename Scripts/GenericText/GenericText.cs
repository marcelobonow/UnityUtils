using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomGenericText
{
    public abstract class GenericText : MonoBehaviour
    {

        public abstract string GetText();
        public abstract void SetText(string text);
        public abstract void SetText(object text);

        public abstract Color GetColor();
        public abstract void SetColor(Color color);
    }
}

