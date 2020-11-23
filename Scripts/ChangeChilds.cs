using System.Threading;
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Este é um script que possui funções que lidam com algum ou todos os filhos de um objeto.
/// </summary>
public class ChangeChilds : MonoBehaviour {
	/// mudar todos os for para uma chamada de função privada
	public static void DisableAllChilds(GameObject go)
	{
		foreach (Transform child in go.transform)
		{
			child.gameObject.SetActive(false);
		}
	}
	public static void DisableAll(GameObject[] go)
	{
		foreach(GameObject gameObject in go)
		{
			gameObject.SetActive(false);
		}
	}
	public static void EnableAll(GameObject[] gameObjectList)
	{
		foreach (GameObject gameObject in gameObjectList)
		{
			gameObject.SetActive(true);
		}
	}

	public static GameObject[] GetAllChilds(GameObject go)
	{
		List<GameObject> result = new List<GameObject>();
		foreach(Transform child in go.transform)
		{
			result.Add(child.gameObject);
		}
		return result.ToArray();
	}
	public static void EnableAllChilds(GameObject go)
	{
		foreach(Transform child in go.transform)
		{
			child.gameObject.SetActive(true);
		}
	}
	public static void ChangeScaleAllChilds(Transform go, List<Vector3> OriginalScale, Vector3 Direction, float amount)
	{
		for (UInt16 i = 0; i < go.childCount; i++)
		{
			go.GetChild(i).localScale = OriginalScale[i] +(Direction * amount);
		}
	}
	public static void ChangeScaleAllChilds(Transform go, List<Vector3> size)
	{
		for (UInt16 i = 0; i < go.transform.childCount; i++)
		{
			go.GetChild(i).localScale = size[i];
		}
	}
	public static void ChangeScaleAllChilds(Transform go, Vector3 size)
	{
		for (UInt16 i = 0; i < go.transform.childCount; i++)
		{
			go.GetChild(i).localScale = size;
		}
	}
	public static void ChangePositionAllChilds(Transform go, List<Vector3>originalPositions, Vector3 Direction, float amount)
	{
		for (UInt16 i = 0; i < go.childCount; i++)
		{
			go.GetChild(i).position = originalPositions[i] +(Direction * amount);
		}
	}
	public static void ChangePositionAllChilds(Transform go, List<Vector3> position)
	{
		for (UInt16 i = 0; i < go.transform.childCount; i++)
		{
			go.GetChild(i).position = position[i];
		}
	}
}

