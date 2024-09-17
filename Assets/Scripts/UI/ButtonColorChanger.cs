using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class ButtonColorChanger : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color highlightedColor = Color.gray;
    [SerializeField] private float transitionDuration = 1.0f; // 色が変わるまでの時間

    private Image buttonImage;

    void Start()
    {
        buttonImage = button.GetComponent<Image>();

        // EventTriggerを取得または追加
        EventTrigger trigger = button.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = button.gameObject.AddComponent<EventTrigger>();
        }

        // PointerEnterイベントを設定
        EventTrigger.Entry entryEnter = new EventTrigger.Entry();
        entryEnter.eventID = EventTriggerType.PointerEnter;
        entryEnter.callback.AddListener((eventData) => { OnPointerEnter(); });
        trigger.triggers.Add(entryEnter);

        // PointerExitイベントを設定
        EventTrigger.Entry entryExit = new EventTrigger.Entry();
        entryExit.eventID = EventTriggerType.PointerExit;
        entryExit.callback.AddListener((eventData) => { OnPointerExit(); });
        trigger.triggers.Add(entryExit);
    }

    public void OnPointerEnter()
    {
        buttonImage.DOKill(); // 既存のトゥイーンを停止
        buttonImage.DOColor(highlightedColor, transitionDuration);
    }

    public void OnPointerExit()
    {
        buttonImage.DOKill(); // 既存のトゥイーンを停止
        buttonImage.DOColor(normalColor, transitionDuration);
    }
}



