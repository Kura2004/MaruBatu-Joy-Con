using UnityEngine;
using DG.Tweening;
using System.Collections;

public class ObjectShaker : MonoBehaviour
{
    [SerializeField] private float shakeDuration = 2f;  // 震える秒数
    [SerializeField] private float shakeIntensity = 1f;  // 震えの強さ
    [SerializeField] private int vibrato = 10;  // 震えの回数
    [SerializeField] private bool repeatShake = false;  // 一定周期で震えるかどうか
    [SerializeField] private float repeatInterval = 5f;  // 震えの周期（秒）

    private Vector3 originalPosition;  // オブジェクトの元の位置
    private Tween currentShakeTween;  // 現在の震えアニメーションを管理するTween

    private void Start()
    {
        originalPosition = transform.position;  // オブジェクトの元の位置を記録
        if (repeatShake)
        {
            // 一定周期で震えるコルーチンを開始
            StartCoroutine(ShakeObjectRoutine());
        }
    }

    private void ShakeObject()
    {
        // 既存のアニメーションがあれば中断
        StopShake();

        // 新しい震えアニメーションを開始
        currentShakeTween = transform.DOShakePosition(shakeDuration, shakeIntensity, vibrato)
            .SetEase(Ease.OutQuad)
            .OnKill(() =>
            {
                // アニメーションが終了したら元の位置に戻す
                transform.position = originalPosition;
            });
    }

    public void StopShake()
    {
        if (currentShakeTween != null && currentShakeTween.IsPlaying())
        {
            // アニメーションが再生中である場合、停止する
            currentShakeTween.Kill();
            // 元の位置に戻す
            transform.position = originalPosition;
        }
    }

    private IEnumerator ShakeObjectRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(repeatInterval);

            if (!TimeControllerToggle.isTimeStopped)
            {
                // 再び震えエフェクトを適用
                ShakeObject();
            }
            else
            {
                // TimeControllerToggle.isTimeStoppedがtrueの場合に震えを中断
                StopShake();
            }
        }
    }
}
