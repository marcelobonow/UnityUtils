using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class DebugCanvas : MonoBehaviour
{
    [SerializeField] private Button togglePanel;
    [SerializeField] private Button resetScene;
    [SerializeField] private Button clearPrefs;

    [SerializeField] private GameObject fpsPanel;

    [Header("Version")]
    [SerializeField] private TextMeshProUGUI platform;
    [SerializeField] private TextMeshProUGUI versionNumber;

    void Start()
    {
#if !DEBUG_MODE
        gameObject.SetActive(false);
#else
            Setup();
#endif
    }

    public void Setup()
    {
        togglePanel.onClick.RemoveAllListeners();
        togglePanel.onClick.AddListener(() => fpsPanel.SetActive(!fpsPanel.activeSelf));

        resetScene.onClick.RemoveAllListeners();
        resetScene.onClick.AddListener(() => SceneManager.LoadScene(SceneManager.GetActiveScene().name));

        clearPrefs.onClick.RemoveAllListeners();
        clearPrefs.onClick.AddListener(() => PlayerPrefs.DeleteAll());

        platform.text = SystemInfo.operatingSystem;
        versionNumber.text = "v" + Application.version;
    }
}