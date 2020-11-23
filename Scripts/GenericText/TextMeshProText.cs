using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace CustomGenericText
{
    [ExecuteInEditMode]
    public class TextMeshProText : GenericText
    {

        [SerializeField] private TextMeshProUGUI text;

        private void Awake()
        {
            if (!text)
                text = GetComponent<TextMeshProUGUI>();
        }

        public override Color GetColor() => text.color;
        public override string GetText() => text.text;
        public override void SetColor(Color color) => text.color = color;
        public override void SetText(string textString)
        {
            if (!text)
                text = GetComponent<TextMeshProUGUI>();
            text.text = textString;
        }
        public override void SetText(object textObject)
        {
            if (!text)
                text = GetComponent<TextMeshProUGUI>();
            text.text = textObject.ToString();
        }
    }
}