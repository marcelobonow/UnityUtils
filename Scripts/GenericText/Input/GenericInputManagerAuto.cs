using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericInputManagerAuto : GenericInputManager
{

    protected override void SetEndMethod()
    {
        genericInput.AddOnEndListener(TrySendMessageText);
    }
}
