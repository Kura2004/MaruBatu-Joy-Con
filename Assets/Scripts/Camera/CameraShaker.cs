using UnityEngine;
using Cinemachine;
using DG.Tweening;
using System.Collections;

public class CameraShaker : MonoBehaviour
{
    [SerializeField] private float shakeDuration = 0.5f;  // 1回の揺れの継続時間
    [SerializeField] private float shakeIntensity = 0.5f;  // 揺れの強さ
    [SerializeField] private int vibrato = 10;  // 揺れの回数
    [SerializeField] private float interval = 5f;  // 揺れの間隔（秒）
    [SerializeField] private CinemachineVirtualCamera virtualCamera; // CinemachineVirtualCameraへの参照

    private void OnEnable()
    {
        // 一定間隔でカメラを揺らすコルーチンを開始
        StartCoroutine(ShakeCameraRoutine());
    }

    private IEnumerator ShakeCameraRoutine()
    {
        while (true)
        {
            // 次の揺れまで指定した間隔を待つ
            yield return new WaitForSeconds(interval);

            if (!TimeControllerToggle.isTimeStopped)
            {
                // DOTweenを使ってカメラの揺れエフェクトを適用
                virtualCamera.transform.DOShakePosition(shakeDuration, shakeIntensity, vibrato).SetEase(Ease.OutQuad);

                // エフェクトの開始アニメーションをトリガー
                InvertColorsEffect.Instance.StartAnimation(0.01f, 0.5f);
            }
        }
    }
}


