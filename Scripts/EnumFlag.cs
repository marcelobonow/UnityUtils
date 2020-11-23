using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnumFlag : PropertyAttribute
{
    public string name;

    public EnumFlag() { }

    public EnumFlag(string name)
    {
        this.name = name;
    }
}
