using UnityEngine;
using DG.Tweening;  // DoTweenの名前空間
using UnityEngine.SceneManagement;  // シーン管理のための名前空間

public class ScaleRotateFadeEffect : MonoBehaviour
{
    // 回転軸の選択肢
    public enum Axis
    {
        X,
        Y,
        Z
    }

    [SerializeField] private float initialScale = 0.5f;      // 画像の初期スケール
    [SerializeField] private Vector3 initialRotation = Vector3.zero;  // 画像の初期回転角度
    [SerializeField] private float startMenuScale = 1.5f;     // StartMenuシーンでの拡大後の目標スケール
    [SerializeField] private float fourByFourScale = 2f;      // 4x4シーンでの拡大後の目標スケール
    [SerializeField] private float gameOverScale = 3f;        // GameOverシーンでの拡大後の目標スケール
    [SerializeField] private float animationDuration = 1f;    // アニメーションの時間
    [SerializeField] private float fadeDuration = 1f;         // フェードアウトの時間
    [SerializeField] private Axis rotationAxis = Axis.Y;      // 回転軸
    private float targetScale;

    private void Start()
    {
        SetTargetScale();
        InitializeEffect();
        ApplyEffect();
    }

    private void SetTargetScale()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        switch (sceneName)
        {
            case "StartMenu":
                targetScale = startMenuScale;
                break;
            case "4x4":
                targetScale = fourByFourScale;
                break;
            case "GameOver":
                targetScale = gameOverScale;
                break;
            default:
                targetScale = 2f; // デフォルト値
                break;
        }
    }

    private void InitializeEffect()
    {
        // 初期のスケールと回転を設定
        transform.localScale = Vector3.one * initialScale;
        transform.rotation = Quaternion.Euler(initialRotation);
    }

    private CanvasGroup GetOrAddCanvasGroup()
    {
        // CanvasGroupコンポーネントを取得または追加（フェードアウト用）
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
        return canvasGroup;
    }

    private Vector3 GetRotationVector()
    {
        // 回転軸に応じた回転ベクトルを取得
        switch (rotationAxis)
        {
            case Axis.X:
                return new Vector3(360, 0, 0);
            case Axis.Y:
                return new Vector3(0, 360, 0);
            case Axis.Z:
                return new Vector3(0, 0, 360);
            default:
                return Vector3.zero;
        }
    }

    private void ApplyEffect()
    {
        CanvasGroup canvasGroup = GetOrAddCanvasGroup();

        Vector3 rotationVector = GetRotationVector();
        Vector3 finalRotation = transform.eulerAngles + rotationVector;

        // アニメーション設定
        Sequence animationSequence = DOTween.Sequence();
        animationSequence
            .Append(transform.DOScale(targetScale, animationDuration).SetEase(Ease.InOutQuad))
            .Join(transform.DORotate(finalRotation, animationDuration, RotateMode.FastBeyond360).SetEase(Ease.InOutQuad))
            .Join(canvasGroup.DOFade(0, fadeDuration).SetDelay(animationDuration - fadeDuration))
            .OnComplete(() => Destroy(gameObject));  // アニメーションが完了したらオブジェクトを削除
    }
}
