using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GuideScaler : MonoBehaviour
{
    [SerializeField] private Image guideImage;       // ガイドとして設定するImage
    [SerializeField] private float duration = 1f;    // スケールの補完時間
    [SerializeField] private Ease easing = Ease.InOutQuad;  // イージングをインスペクターで設定
    [SerializeField] private float scaleMultiplier = 1.5f;  // 拡大時の倍率をインスペクターで指定

    private Vector3 initialScale; // 初期のスケールを保存
    private bool isGuideVisible = true; // ガイドが表示されているかどうかのフラグ

    private void Start()
    {
        if (guideImage == null)
        {
            Debug.LogError("ガイドImageが設定されていません！");
            return;
        }

        // 初期スケールを保存
        initialScale = guideImage.transform.localScale;
        guideImage.enabled = false;
        isGuideVisible = false;
    }

    // n秒でガイドのスケールを拡大するメソッド
    public void ShowGuide()
    {
        guideImage.enabled = true;
        if (guideImage != null)
        {
            Vector3 targetScale = initialScale * scaleMultiplier;  // 拡大倍率を適用
            guideImage.transform.localScale = Vector3.zero;
            guideImage.transform.DOScale(targetScale, duration).SetEase(easing);
        }
    }

    // n秒でガイドのスケールを初期値に戻す（非表示状態）メソッド
    public void HideGuide()
    {
        if (guideImage != null)
        {
            guideImage.transform.DOScale(Vector3.zero, duration).SetEase(easing).OnComplete(() =>
            {
                guideImage.enabled = false;
            });
        }
    }

    // ガイドの表示/非表示をトグルするメソッド
    public void ToggleGuide()
    {
        if (guideImage != null)
        {
            if (isGuideVisible)
            {
                HideGuide();
            }
            else
            {
                ShowGuide();
            }
            isGuideVisible = !isGuideVisible; // フラグを反転
        }
    }

    // ガイドをn秒間隔で自動的にトグルし続けるメソッド
    public void StartAutoToggleGuide()
    {
        if (guideImage != null)
        {
            DOTween.Sequence()
                .Append(guideImage.transform.DOScale(initialScale * scaleMultiplier, duration).SetEase(easing)) // 拡大
                .Append(guideImage.transform.DOScale(Vector3.zero, duration).SetEase(easing)) // 縮小
                .SetLoops(-1, LoopType.Yoyo); // 無限ループで交互に切り替え
        }
    }
}
