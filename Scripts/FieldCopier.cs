using System;
using System.Collections.Generic;
using System.Reflection;

public static class FieldCopier
{
    public static void CopyCommonFields(object src, object dest)
    {
        if (src is null || dest is null) return;

        // indexa DEST (nome+tipo)
        var dFields = new Dictionary<(string, Type), FieldInfo>();
        foreach (var f in AllFields(dest.GetType()))
            dFields[(f.Name, f.FieldType)] = f;

        // percorre SRC (inclui campos private herdados)
        foreach (var sf in AllFields(src.GetType()))
            if (dFields.TryGetValue((sf.Name, sf.FieldType), out var df))
                df.SetValue(dest, sf.GetValue(src));
    }

    static IEnumerable<FieldInfo> AllFields(Type t)
    {
        const BindingFlags Flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;
        for (; t != null && t != typeof(object); t = t.BaseType)
            foreach (var f in t.GetFields(Flags))
                yield return f;
    }
}