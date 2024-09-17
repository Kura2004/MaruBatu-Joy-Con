using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

public class ButtonScaler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private Button targetButton; // 操作対象のボタン

    [SerializeField]
    float scaleRate = 1.3f;

    private Vector3 enlargedScale;

    [SerializeField]
    private float scaleDuration = 0.3f; // 拡大・縮小にかかる時間

    private Vector3 originalScale; // 元のスケール

    private void Start()
    {
        if (targetButton == null)
        {
            targetButton = GetComponent<Button>();
        }

        // 元のスケールを保存
        originalScale = targetButton.transform.localScale;
        enlargedScale = originalScale * scaleRate; // 拡大後のスケール
    }

    // マウスがボタンに触れた時に呼ばれるメソッド
    public void OnPointerEnter(PointerEventData eventData)
    {
        EnlargeButton();
    }

    // マウスがボタンから離れた時に呼ばれるメソッド
    public void OnPointerExit(PointerEventData eventData)
    {
        ResetButtonSize();
    }

    // ボタンのサイズを大きくするメソッド
    public void EnlargeButton()
    {
        targetButton.transform.DOScale(enlargedScale, scaleDuration).SetEase(Ease.OutBack);
    }

    // ボタンのサイズを徐々に元に戻すメソッド
    public void ResetButtonSize()
    {
        targetButton.transform.DOScale(originalScale, scaleDuration).SetEase(Ease.InOutQuad);
    }
}

