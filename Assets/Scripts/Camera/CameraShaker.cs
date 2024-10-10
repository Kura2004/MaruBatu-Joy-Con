using UnityEngine;
using DG.Tweening;
using System.Collections;

public class CameraShaker : MonoBehaviour
{
    [SerializeField] private float shakeDuration = 0.5f;  // 1回の揺れの継続時間
    [SerializeField] private float shakeIntensity = 0.5f;  // 揺れの強さ
    [SerializeField] private int vibrato = 10;  // 揺れの回数
    [SerializeField] private float interval = 5f;  // 揺れの間隔（秒）
    [SerializeField] private Camera mainCamera;  // メインカメラ

    private Coroutine shakeCoroutine;  // コルーチンの参照

    public void StartAnimation()
    {
        // 一定間隔でカメラを揺らすコルーチンを開始
        shakeCoroutine = StartCoroutine(ShakeCameraRoutine());
    }

    private IEnumerator ShakeCameraRoutine()
    {
        while (true)
        {
            // DOTweenを使ってカメラの揺れエフェクトを適用
            mainCamera.transform.DOShakePosition(shakeDuration, shakeIntensity, vibrato).SetEase(Ease.OutQuad);

            // エフェクトの開始アニメーションをトリガー
            InvertColorsEffect.Instance.StartAnimation(0.01f, 0.5f);
            // 次の揺れまで指定した間隔を待つ
            yield return new WaitForSeconds(interval);
        }
    }

    /// <summary>
    /// アニメーションを停止するメソッド
    /// </summary>
    public void StopAnimation()
    {
        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);  // コルーチンを停止
            shakeCoroutine = null;  // 参照をリセット
        }
    }
}