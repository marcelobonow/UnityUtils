using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class ScriptedAnimationController : MonoBehaviour
{
    public AudioSource sfx;
    ///Relação do nome da particual e o id dela
    private Vector3 midPoint; ///Ponto em que a animação para de crescer e começa a ir em direção ao target
    private Action callback = null;
    private ScriptedAnimationConfig config;

    private GameObject target;

    private float duration = 2f;

    public void Awake() => SceneManager.sceneLoaded += DestroyOnLoad;
    private void OnDestroy() => SceneManager.sceneLoaded -= DestroyOnLoad;

    public void StartAnimation(ScriptedAnimationConfig config, int currentQuantity)
    {
        this.config = config;
        callback = config.callback;
        target = config.target.gameObject;
        duration = config.duration * Random.Range(0.75f, 1.25f);
        gameObject.transform.position = config.sourcePosition - new Vector3(Random.Range(-config.spread, config.spread), Random.Range(-config.spread, config.spread));
        var offset = (currentQuantity % 10) / 10f * (duration / 2f);
        var timeOffset = duration / 2f + offset;
        var randomValueX = Random.Range(-config.spread, config.spread);
        var randomValueY = Random.Range(-config.spread, config.spread);
        midPoint = new Vector3(transform.position.x + randomValueX, transform.position.y + randomValueY, transform.position.z);
        iTween.MoveTo(gameObject, iTween.Hash("position", midPoint,
            "time", timeOffset, "easeType", iTween.EaseType.easeOutQuart));
        iTween.ScaleTo(gameObject, iTween.Hash("scale", Vector3.one * config.scale,
            "time", timeOffset, "easeType", iTween.EaseType.easeOutQuart, "OnComplete", "NextAnimation"));
        //NextAnimation();
    }

    private void NextAnimation()
    {
        if (sfx) sfx.Play();
        iTween.MoveTo(gameObject, iTween.Hash("position", target.transform.position,
            "time", duration / 2f, "easeType", iTween.EaseType.easeInQuad)); ;
        iTween.ScaleTo(gameObject, iTween.Hash("scale", new Vector3(0.7f, 0.7f, 1) * config.scale,
            "time", duration / 2f, "easeType", iTween.EaseType.easeInQuad, "OnComplete", "Destroythis"));
    }

    public void DestroyOnLoad(Scene scene, LoadSceneMode mode)
    {
        Logger.Log("Destruindo no load");
        Destroythis(); ///Se a cena mudar enquanto a moeda ainda existe, destroi a moeda.
    }

    private void Destroythis()
    {
        callback?.Invoke();
        if (sfx != null) sfx.Stop();
        if (gameObject != null) Destroy(gameObject);
    }
}
