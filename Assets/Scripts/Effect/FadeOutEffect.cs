using UnityEngine;
using DG.Tweening;

public class FadeOutEffect : MonoBehaviour
{
    [SerializeField]
    private float fadeDuration = 2f; // アルファ値が0になるまでの時間（秒）

    private SpriteRenderer spriteRenderer; // SpriteRendererの参照

    private void Awake()
    {
        // SpriteRendererコンポーネントを取得
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Debug.LogWarning("SpriteRenderer component is missing on the GameObject.");
        }
    }

    /// <summary>
    /// フェードアウトを開始するメソッド
    /// </summary>
    public void StartFadeOut()
    {
        if (spriteRenderer != null)
        {
            // 現在の色を取得
            Color currentColor = spriteRenderer.color;

            // DoTweenを使用してアルファ値を補完的に0まで減少させる
            spriteRenderer.DOColor(new Color(currentColor.r, currentColor.g, currentColor.b, 0f), fadeDuration)
                          .SetEase(Ease.Linear);
        }
    }
}