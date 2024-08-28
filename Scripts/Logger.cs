using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Threading.Tasks;


public class Logger : MonoBehaviour
{
    public static bool isDebug = false;
    private const bool writeFiles = true;
    private static readonly long maxFileSize = 10 * 1024 * 1024;
    private static readonly float logInterval = 30;

    private static string path;
    private static string fullPath;

    private static StringBuilder buffer;

    public void Awake()
    {
        path = Application.persistentDataPath;
        fullPath = path + "/log.txt";
        UnityEngine.Debug.Log("Logando no arquivo: " + Application.persistentDataPath);
        Application.logMessageReceived += OnLog;
        buffer = new StringBuilder();
        StartCoroutine(WriteFileDaemon());

    }
    public void OnLog(string message, string stackTrace, LogType type)
    {
        if (type == LogType.Exception || type == LogType.Error)
        {
            var timeStamp = "[" + DateTime.UtcNow.ToString("hh:mm:ss") + "] ";
            var messageText = timeStamp + "ERRO!: " + message;

            AddtoBuffer(messageText);
            AddtoBuffer(message + ": " + stackTrace);
        }
        else
        {
            AddtoBuffer(message);
        }
    }

    public static void Log(object message, LogType logLevel = LogType.Log, GameObject emitter = null)
    {
        var isLogEnabled = true;

        ///Se for erro, loga a mensagem independente de estar habilitado ou não
        var timeStamp = "[" + DateTime.UtcNow.ToString("yyyy/MM/dd HH:mm:ss") + "] ";
        if (logLevel == LogType.Error)
        {
            UnityEngine.Debug.LogError($"{timeStamp} Error: {message}", emitter);
        }
        else if (logLevel == LogType.Warning)
        {
            var messageText = $"{timeStamp} Warning: {message}";
            if (isLogEnabled)
                UnityEngine.Debug.LogWarning(messageText, emitter);
            else
                AddtoBuffer(messageText);
        }
        else
        {
            var messageText = $"{timeStamp} Info: {message}";
            if (isLogEnabled)
                UnityEngine.Debug.Log(messageText, emitter);
            else
                AddtoBuffer(messageText);
        }
    }

    public static void Debug(object message, GameObject emitter = null)
    {
        if (!isDebug)
            return;

        Log(message, LogType.Log, emitter);
    }
    public static void LogWarning(object message) => Log(message, LogType.Warning);
    public static void LogError(object message) => Log(message, LogType.Error);
    public static void LogError(object message, GameObject emitter) => Log(message, LogType.Error, emitter);

    private IEnumerator WriteFileDaemon()
    {
        while (true)
        {
            if (buffer.Length > 0)
            {
                FileLog();
                buffer.Length = 0;
            }
            yield return new WaitForSeconds(logInterval);

        }
    }
    private static void AddtoBuffer(string toLog)
    {
        buffer.Append(toLog);
        buffer.Append("\n");
#if INSTANT_LOG
        FileLog();
        buffer.Length = 0;
#endif
    }
    ///TODO: Passar a escrita para outra thread
    private static void FileLog()
    {
        if (writeFiles)
        {
            try
            {
                File.AppendAllText(fullPath, buffer.ToString());

                var fileSize = new FileInfo(fullPath).Length;
                if (fileSize > maxFileSize)
                {
                    var lines = File.ReadAllLines(fullPath);
                    File.WriteAllLines(fullPath, lines.Skip(lines.Length / 2).ToArray());
                }
            }
            catch
            {
                UnityEngine.Debug.LogWarning("Não foi possivel salvar o log, adicionando ao buffer");
            }
        }
    }
    public static string GetLog() => File.ReadAllText(fullPath);
    private void OnDestroy() => FileLog();
}
