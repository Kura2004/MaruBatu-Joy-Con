using UnityEngine;
using TMPro; // TextMeshPro 名前空間
using DG.Tweening; // DOTween 名前空間

public class ColorLerpTextTMP : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshPro; // TextMeshProUGUI コンポーネント
    [SerializeField] private Color startColor = Color.white; // 開始色
    [SerializeField] private Color endColor = Color.red; // 終了色
    [SerializeField] private float duration = 1f; // 色が変わるまでの時間

    private void Start()
    {
        // DOTweenで色の補間アニメーションを設定
        textMeshPro.color = startColor; // 初期色を設定
        LoopColor();
    }

    private void LoopColor()
    {
        // 色を終了色まで補間し、完了後に逆方向に補間
        textMeshPro.DOColor(endColor, duration)
            .SetEase(Ease.Linear) // 線形補完
            .SetLoops(-1, LoopType.Yoyo); // 無限ループで行ったり来たり
    }
}
