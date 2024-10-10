using UnityEngine;
using DG.Tweening; // DOTweenの名前空間

public class CanvasFader : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private CanvasGroup mainCanvasGroup; // フェードイン・アウト対象のCanvasGroupコンポーネント

    [Header("Fade Settings")]
    [SerializeField] private float fadeDuration = 0.5f; // フェードの持続時間

    [SerializeField] bool onStart = false;

    private void OnEnable()
    {
        // CanvasGroupとRectTransformの設定を確認
        if (mainCanvasGroup == null)
        {
            Debug.LogError("CanvasGroup is not assigned in the inspector.");
        }

        // 初期状態でキャンバスを非表示に設定
        if (mainCanvasGroup != null)
        {
            mainCanvasGroup.alpha = 0; // アルファ値を0に設定
            mainCanvasGroup.interactable = false;
            mainCanvasGroup.blocksRaycasts = false;
        }

        if (onStart)
        {
            ShowCanvas();
        }
    }

    /// <summary>
    /// キャンバスをフェードインで表示するメソッド
    /// </summary>
    public void ShowCanvas()
    {
        if (mainCanvasGroup != null)
        {
            // アルファ値を1にフェード
            mainCanvasGroup.DOFade(1, fadeDuration).OnStart(() =>
            {
                mainCanvasGroup.interactable = true;
                mainCanvasGroup.blocksRaycasts = true;
                Debug.Log("Canvas is fading in.");
            });
        }
    }

    /// <summary>
    /// キャンバスをフェードアウトで非表示にするメソッド
    /// </summary>
    public void HideCanvas()
    {
        if (mainCanvasGroup != null)
        {
            // アルファ値を0にフェード
            mainCanvasGroup.DOFade(0, fadeDuration).OnComplete(() =>
            {
                mainCanvasGroup.interactable = false;
                mainCanvasGroup.blocksRaycasts = false;
                Debug.Log("Canvas is faded out.");
            });
        }
    }

    /// <summary>
    /// キャンバスの表示・非表示をフェードで切り替えるメソッド
    /// </summary>
    public void ToggleCanvas()
    {
        if (mainCanvasGroup != null)
        {
            if (mainCanvasGroup.alpha > 0)
            {
                HideCanvas();
            }
            else
            {
                ShowCanvas();
            }
        }
    }
}
