using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class ScriptedAnimation : MonoBehaviour
{
    [HideInInspector] public Transform initialPosition;

    private ScriptedAnimation instance;


    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }


    public static void ScreenAnimation(GameObject prefab, Vector3 sourcePosition, GameObject target, Canvas canvas, float duration = 1f, int quantity = 1, float scale = 1f, float spread = 50f, Action callback = null)
    {
        var config = new ScriptedAnimationConfig();
        config.SetPrefab(prefab).SetSourcePosition(sourcePosition).SetTarget(target).SetDuration(duration).SetQuantity(quantity).SetScale(scale).SetSpread(spread).AddCallback(callback);
        CreateGenericPrize(config, canvas);
    }

    public static void CreateGenericPrize(ScriptedAnimationConfig config, Canvas canvas)
    {
        if (config.quantity == 0 || config.sourcePosition == null)
        {
            config.callback();
            return;
        }
        var total = config.quantity;
        ///A cada ficha, desconta da quantidade de fichas criadas,
        ///ao terminar de animar todas as fichas chama o callback
        var configCallback = config.callback;
        canvas.sortingLayerName = config.layerName;
        canvas.sortingOrder = config.orderId;
        for (var i = 0; i < config.quantity; i++)
        {
            var prizeGO = Instantiate(config.prefab);
            prizeGO.transform.SetParent(canvas.transform);
            prizeGO.transform.position = config.sourcePosition;
            prizeGO.transform.localScale = config.scale * Vector3.one;
            var coinAnimation = prizeGO.GetComponent<ScriptedAnimationController>();
            if (!coinAnimation) coinAnimation = prizeGO.AddComponent<ScriptedAnimationController>();

            config.callback = () =>
            {
                if (--total == 0)
                    configCallback?.Invoke();
            };

            coinAnimation.StartAnimation(config, i);
        }
    }
}
