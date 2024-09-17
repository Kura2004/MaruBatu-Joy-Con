using UnityEngine;
using DG.Tweening; // DOTweenの名前空間

public class SpriteFader : MonoBehaviour
{
    [Header("Fade Settings")]
    [SerializeField] private float fadeDuration = 1f; // フェードの持続時間
    [SerializeField] private float fadeInAlpha = 1f; // フェードイン時の目標透明度
    [SerializeField] private float fadeOutAlpha = 0f; // フェードアウト時の目標透明度

    private SpriteRenderer spriteRenderer; // SpriteRendererコンポーネント
    private bool isFadedIn = true; // 現在の透明度状態を追跡するフラグ

    private void OnEnable()
    {
        // SpriteRendererコンポーネントを取得
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer component is missing on this GameObject.");
        }
    }

    /// <summary>
    /// SpriteRendererの透明度を指定した値にフェードするメソッド
    /// </summary>
    public void FadeTo(float targetAlpha, float duration)
    {
        if (spriteRenderer != null)
        {
            Color currentColor = spriteRenderer.color; // 現在の色を取得
            Color targetColor = new Color(currentColor.r, currentColor.g, currentColor.b, targetAlpha); // 目標色を設定

            spriteRenderer.DOColor(targetColor, duration).OnStart(() =>
            {
                Debug.Log($"Fading to alpha {targetAlpha} over {duration} seconds.");
            }).OnComplete(() =>
            {
                Debug.Log("Fade complete.");
            });
        }
    }

    /// <summary>
    /// デフォルト設定で透明度を補完的に変化させるメソッド
    /// </summary>
    public void FadeToDefault()
    {
        FadeTo(fadeInAlpha, fadeDuration);
    }

    /// <summary>
    /// 現在の透明度状態に応じて、フェードインまたはフェードアウトを切り替えるメソッド
    /// </summary>
    public void ToggleFade()
    {
        if (spriteRenderer != null)
        {
            if (isFadedIn)
            {
                FadeTo(fadeOutAlpha, fadeDuration); // フェードアウト
                var cameraHandle = CameraSwitcher.CameraType.Main;
                CameraSwitcher.Instance.SwitchCamera(cameraHandle);
            }
            else
            {
                FadeTo(fadeInAlpha, fadeDuration); // フェードイン
                var cameraHandle = CameraSwitcher.CameraType.Secondary;
                CameraSwitcher.Instance.SwitchCamera(cameraHandle);
            }

            isFadedIn = !isFadedIn; // 状態を反転
        }
    }
}

