using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using System.Collections.Generic;
using System.Linq;
#if USE_NATIVE_CURSOR
using Riten.Native.Cursors;
#endif

public class ClickableLink : MonoBehaviour, IPointerClickHandler, IPointerMoveHandler, IPointerExitHandler
{
    [SerializeField] private string initialMessage;

    private TextMeshProUGUI text;
    private Camera sceneCamera;

    public void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        sceneCamera = FindFirstObjectByType<Camera>();

        if (!GlobalMemory.globalStore.ContainsKey("links"))
            GlobalMemory.globalStore.Add("links", new Dictionary<int, string>());
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        var index = TMP_TextUtilities.FindIntersectingLink(text, Input.mousePosition, sceneCamera);
#if USE_NATIVE_CURSOR
        NativeCursor.SetCursor(index >= 0 ? NTCursors.Link : NTCursors.Default);
#endif
    }

    public void OnPointerExit(PointerEventData data)
    {
#if USE_NATIVE_CURSOR
        NativeCursor.SetCursor(NTCursors.Default);
#endif
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            //var index = TMP_TextUtilities.FindIntersectingWord(text, Input.mousePosition, sceneCamera);
            var index = TMP_TextUtilities.FindIntersectingLink(text, Input.mousePosition, sceneCamera);
            if (index >= 0)
            {
                //var word = text.textInfo.wordInfo[index].GetWord();
                //Debug.Log(word);
                //return;
                var linkInfo = text.textInfo.linkInfo[index];
                var linkId = linkInfo.GetLinkID();

                Debug.Log("LinkId: " + linkId);

                ///TODO: Se for abrir o whatsapp
                ///https://wa.me/552196312XXXX

                if (linkId.Contains("whatsapp"))
                {
                    Application.OpenURL($"{linkId}&text={UnityWebRequest.EscapeURL(initialMessage)}");
                }
                else if (int.TryParse(linkId, out var id) &&
                    GlobalMemory.globalStore["links"].ContainsKey(id))
                {
                    var url = GlobalMemory.globalStore["links"][id];
                    Debug.Log("URL: " + url);
                    if (url.StartsWith("https") || url.StartsWith("http"))
                        Application.OpenURL(url);
                }

            }
        }
    }

    public void AddLinksTag()
    {
        if (text == null && !TryGetComponent(out text))
            return;

        var links = text.text.GetLinks();
        int offset = 0;
        foreach (var link in links)
        {

            int id = 0;
            if (GlobalMemory.globalStore["links"].ContainsValue(link.text))
                id = GlobalMemory.globalStore["links"].FirstOrDefault().Key;
            else
            {
                id = GlobalMemory.globalStore["links"].Count;
                GlobalMemory.globalStore["links"].Add(id, link.text);
            }

            if (link.text.EndsWith("/"))
                link.text = link.text.Remove(link.text.Length - 1, 1);

            var idString = id.ToString();
            var linkStart = $"<link=\"{idString}\"><u><color=#2a78f5>";
            var linkEnd = "</color></u></link>";

            var startIndex = link.startPosition + offset;
            text.text = text.text.Insert(startIndex, linkStart);
            text.text = text.text.Insert(startIndex + linkStart.Length + link.text.Length, linkEnd);
            offset += linkStart.Length + linkEnd.Length;
        }

        Debug.Log("Finalizou");

    }

}
