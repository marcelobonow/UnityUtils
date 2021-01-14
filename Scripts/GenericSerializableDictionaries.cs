using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[Serializable] public class StringGameObjectDictionary : SerializableDictionary<string, GameObject> { };
[Serializable] public class StringActionDictionary : SerializableDictionary<string, UnityEvent> { };
[Serializable] public class StringFloatDictionary : SerializableDictionary<string, float> { };
[Serializable] public class StringImageDictionary : SerializableDictionary<string, Image> { };
[Serializable] public class IntGameObjectDictionary : SerializableDictionary<int, GameObject> { };
[Serializable] public class IntBoolDictionary : SerializableDictionary<int, bool> { };
[Serializable] public class IntStringDictionary : SerializableDictionary<int, string> { };
[Serializable] public class UIntBoolDictionary : SerializableDictionary<uint, bool> { };
