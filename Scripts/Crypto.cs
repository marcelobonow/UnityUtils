using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Crypto
{
    public static string RandomString(int digits)
    {
        const string characters = "abcdefghijklmnopqrstuvwxyz0123456789";
        var builder = new StringBuilder();
        for (var i = 0; i < digits; i++)
            builder.Append(characters[Random.Range(0, characters.Length)]);

        return builder.ToString();
    }
}
