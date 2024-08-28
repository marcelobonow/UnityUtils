using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParseUrlUtils
{
    public static Dictionary<string, string> ParseUrl(string url)
    {
        var urlParts = url.Split("?");
        if (urlParts.Length <= 1)
            return null;

        var queryString = urlParts[1];
        return ParseQuery(queryString);
    }

    public static Dictionary<string, string> ParseQuery(string query)
    {
        var queryParameters = new Dictionary<string, string>();
        var querySegments = query.Split('&');
        Logger.Log($"Tem {querySegments.Length} segmentos");
        foreach (var segment in querySegments)
        {
            var parts = segment.Split('=');
            if (parts.Length > 0)
            {
                var key = parts[0].Trim(new char[] { '?', ' ' });
                var val = parts[1].Trim();
                Logger.Log($"Tem parte, key: {key}. Val: {val}");
                queryParameters.Add(key, val);
            }
        }

        return queryParameters;
    }
}
