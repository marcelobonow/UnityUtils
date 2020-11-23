using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CustomGenericText
{
    [ExecuteAlways]
    public class NormalText : GenericText
    {
        [SerializeField] private Text text;

        private void Awake()
        {
            if(!text)
                text = GetComponent<Text>();
        }

        public override Color GetColor() => text.color;
        public override string GetText() => text.text;
        public override void SetColor(Color color) => text.color = color;
        public override void SetText(string textString)
        {
            if(!text)
                text = GetComponent<Text>();
            text.text = textString;
        }
        public override void SetText(object textObject)
        {
            if(!text)
                text = GetComponent<Text>();
            text.text = textObject.ToString();
        }
    }
}
