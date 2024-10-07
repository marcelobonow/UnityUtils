using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileUtils : MonoBehaviour
{
    public static bool SaveToFile(string filePath, byte[] data)
    {
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            File.WriteAllBytes(filePath, data);
            return true;
        }
        catch (Exception exception)
        {
            Debug.LogError("Erro salvando: " + exception);
            return false;
        }
    }
}
