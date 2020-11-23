using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CustomButtonChildrenMoveDown : CustomButton
{
    [SerializeField] private Vector3 displacement;
    [SerializeField] private GameObject[] gameObjectsToMoveDown;

    private Dictionary<GameObject, Vector3> originalPositions = new Dictionary<GameObject, Vector3>();

    protected override void GetInformations()
    {
        foreach (var gameObjectMoveDown in gameObjectsToMoveDown)
            originalPositions.Add(gameObjectMoveDown, gameObjectMoveDown.GetComponent<RectTransform>().localPosition);
        base.GetInformations();
    }

    protected override void PressEffect()
    {
        base.PressEffect();
        foreach (var gameObjectMoveDown in gameObjectsToMoveDown)
        {
            if (originalPositions.ContainsKey(gameObjectMoveDown))
                gameObjectMoveDown.GetComponent<RectTransform>().localPosition = originalPositions[gameObjectMoveDown] + displacement;
        }
    }

    protected override void ReleaseEffect()
    {
        base.ReleaseEffect();
        foreach (var gameObjectMoveDown in gameObjectsToMoveDown)
        {
            if (originalPositions.ContainsKey(gameObjectMoveDown))
                gameObjectMoveDown.GetComponent<RectTransform>().localPosition = originalPositions[gameObjectMoveDown];
        }
    }
}
