using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CountdownText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private Image uiPanel; // UIパネルをImageクラスで設定
    [SerializeField] private float initialScale = 2f;
    [SerializeField] private float endScale = 0.5f;
    [SerializeField] private float initialAlpha = 1f;
    [SerializeField] private float endAlpha = 0f;
    [SerializeField] private List<float> durations = new List<float>(); // 各アニメーションの補完時間
    [SerializeField] private List<Color> countdownColors = new List<Color>(); // 各カウントダウンの色

    [SerializeField] private Ease panelFadeEase = Ease.OutCubic; // パネルフェードのイージングタイプ
    [SerializeField] private float panelDuration = 1.0f;

    [SerializeField] PlayerImageAnimator[] animator;

    private Vector3 savedInitialScale;

    private void Start()
    {
        // 初期のローカルスケールを保存
        savedInitialScale = countdownText.transform.localScale;
    }

    // 全ての補完時間を合計して返すメソッド
    public float GetTotalDuration()
    {
        float totalDuration = 0f;
        foreach (float duration in durations)
        {
            totalDuration += duration;
        }
        return totalDuration;
    }

    public IEnumerator StartCountdown()
    {
        string[] countdownNumbers = { "3", "2", "1", "Start" };

        // パネルをフェードアウト
        FadeOutPanelAfterCountdown();

        for (int i = 0; i < countdownNumbers.Length; i++)
        {
            countdownText.text = countdownNumbers[i];

            // テキストの色を変更
            ChangeTextColor(countdownColors[i]);

            // 補完時間をリストから取得
            float duration = durations[i];

            // インデックスごとの処理を呼び出す
            ExecuteCountdownAction(i, duration);

            yield return new WaitForSeconds(duration);
        }
        AnimateFade(endAlpha, 0.3f);

    }

    // インデックスに応じた処理を実行
    private void ExecuteCountdownAction(int index, float duration)
    {
        switch (index)
        {
            case 0: // "3"
                ResetScale(initialScale);
                ResetAlpha(initialAlpha);
                AnimateScale(endScale, duration);
                AnimateFade(endAlpha, duration);
                animator[0].StartMovement();
                animator[1].StartMovement();
                break;

            case 1: // "2"
                ResetScale(initialScale);
                ResetAlpha(initialAlpha);
                AnimateScale(endScale, duration);
                AnimateFade(endAlpha, duration);
                break;

            case 2: // "1"
                ResetScale(initialScale);
                ResetAlpha(initialAlpha);
                AnimateScale(endScale, duration);
                AnimateFade(endAlpha, duration);
                GameTurnManager.Instance.IsGameStarted = true;
                break;

            case 3: // "Start"（最後のインデックス）
                ResetScale(endScale);
                ResetAlpha(endAlpha);
                AnimateScale(initialScale, duration);
                AnimateFade(initialAlpha, duration);
                animator[0].ChangeSpritesColorToWhite();
                animator[1].ChangeSpritesColor(Color.gray, 0.8f);
                break;

            default:
                break;
        }
    }

    // スケールのリセット
    private void ResetScale(float scaleMultiplier)
    {
        countdownText.transform.localScale = savedInitialScale * scaleMultiplier;
    }

    // アルファ値のリセット
    private void ResetAlpha(float alpha)
    {
        countdownText.alpha = alpha;
    }

    // スケールアニメーション
    private void AnimateScale(float targetScale, float duration)
    {
        countdownText.transform.DOScale(targetScale, duration);
    }

    private void AnimateFade(float targetAlpha, float duration)
    {
        countdownText.DOFade(targetAlpha, duration); // アルファ値の補完
    }

    // テキストの色を変更
    private void ChangeTextColor(Color color)
    {
        countdownText.color = color; // 指定された色に変更
    }

    // 全ての補完時間を使ってUIパネルをフェードアウト
    public void FadeOutPanelAfterCountdown()
    {
        float totalDuration = GetTotalDuration(); // 全ての補完時間を取得
        uiPanel.DOFade(0f, panelDuration).SetEase(panelFadeEase); // イージングを設定
    }
}
