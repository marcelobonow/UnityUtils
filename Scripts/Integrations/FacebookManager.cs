using System.Collections;
using System.Collections.Generic;
using Facebook.Unity;
using UnityEngine;

public class FacebookManager : MonoBehaviour
{
    public static Events onFacebookInitialized = new Events();

    private FacebookManager instance;
    private Coroutine retryingCoroutine;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        InitializeFacebook();
    }

    private void InitializeFacebook()
    {
        if (!FB.IsInitialized)
            FB.Init(OnFacebookInitialized);
        else if (retryingCoroutine != null)
            StopCoroutine(retryingCoroutine);
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (!pauseStatus)
        {
            if (FB.IsInitialized)
            {
                FB.ActivateApp();
                if (retryingCoroutine != null)
                    StopCoroutine(retryingCoroutine);
            }
            else
                FB.Init(InitializeFacebook);
        }
    }

    private void OnFacebookInitialized()
    {
        if (FB.IsInitialized)
        {
            FB.ActivateApp();
            onFacebookInitialized.Trigger();
            if (retryingCoroutine != null)
                StopCoroutine(retryingCoroutine);
        }
        else
        {
            Debug.LogError("[FacebookSDK] Couldn't initialize Facebook API, Trying again later");
            retryingCoroutine = GenericCoroutines.DoAfterTime(5f, InitializeFacebook, this);
        }
    }

    public static void LogEvent(string eventName, Dictionary<string, object> parameters)
    {
        if (FB.IsInitialized)
            FB.LogAppEvent(eventName, null, parameters);
        else
            Debug.Log($"[FacebookSDK] Trying to log event '{eventName}' but the SDK is not initialized");
    }
}
