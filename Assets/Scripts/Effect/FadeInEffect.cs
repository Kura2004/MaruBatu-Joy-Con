using UnityEngine;
using DG.Tweening;

public class FadeInEffect : MonoBehaviour
{
    [SerializeField]
    private float fadeDuration = 2f; // アルファ値が1になるまでの時間（秒）

    private SpriteRenderer spriteRenderer; // SpriteRendererの参照

    [SerializeField]
    bool OnStart = true;

    private void Start()
    {
        // SpriteRendererコンポーネントを取得
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Debug.LogWarning("SpriteRenderer component is missing on the GameObject.");
        }
        else
        {
            // アルファ値を0に設定しておく
            Color color = spriteRenderer.color;
            color.a = 0f;
            spriteRenderer.color = color;
        }

        if (OnStart)
            StartFadeIn();
    }

    /// <summary>
    /// フェードインを開始するメソッド
    /// </summary>
    public void StartFadeIn()
    {
        if (spriteRenderer != null)
        {
            // 現在の色を取得
            Color currentColor = spriteRenderer.color;

            // DoTweenを使用してアルファ値を補完的に1まで増加させる
            spriteRenderer.DOColor(new Color(currentColor.r, currentColor.g, currentColor.b, 1f), fadeDuration)
                          .SetEase(Ease.Linear);
        }
    }
}