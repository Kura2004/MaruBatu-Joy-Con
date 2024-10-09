using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class UIObjectShaker : MonoBehaviour
{
    [SerializeField] private float shakeDuration = 2f;  // 震える秒数
    [SerializeField] private float shakeIntensity = 1f;  // 震えの強さ
    [SerializeField] private int vibrato = 10;  // 震えの回数
    [SerializeField] private float prefabSpawnInterval = 1f;  // プレハブ生成のインターバル
    [SerializeField] private GameObject prefabToSpawn;  // 生成するプレハブ
    [SerializeField] private RectTransform uiElementToShake;  // 震えるUI要素（RectTransform）

    private Vector3 originalPosition;  // UI要素の元の位置
    private Tween currentShakeTween;  // 現在の震えアニメーションを管理するTween
    private Coroutine spawnCoroutine;  // プレハブ生成コルーチン

    public void ShakeUIElement()
    {
        SaveOriginalPosition();

        // 既存のアニメーションがあれば中断
        StopShake();

        // 新しい震えアニメーションを開始
        currentShakeTween = uiElementToShake.DOShakePosition(shakeDuration, shakeIntensity, vibrato)
            .SetEase(Ease.OutQuad)
            .OnKill(() =>
            {
                // アニメーションが終了したら元の位置に戻す
                uiElementToShake.anchoredPosition = originalPosition;
                StopPrefabSpawn();  // プレハブ生成も停止
            });

        // プレハブを生成するコルーチンを開始
        StartPrefabSpawn();
    }

    // 震えを止める
    public void StopShake()
    {
        if (currentShakeTween != null && currentShakeTween.IsPlaying())
        {
            // アニメーションが再生中である場合、停止する
            currentShakeTween.Kill();
            // 元の位置に戻す
            uiElementToShake.anchoredPosition = originalPosition;
        }

        // プレハブの生成も停止
        StopPrefabSpawn();
    }

    // 元の位置を記録
    private void SaveOriginalPosition()
    {
        originalPosition = uiElementToShake.anchoredPosition;
    }

    // プレハブを生成し続けるコルーチンを開始
    private void StartPrefabSpawn()
    {
        if (prefabToSpawn != null)
        {
            spawnCoroutine = StartCoroutine(SpawnPrefabCoroutine());
        }
    }

    // プレハブ生成コルーチン
    private IEnumerator SpawnPrefabCoroutine()
    {
        while (true)
        {
            // プレハブを生成
            Instantiate(prefabToSpawn, uiElementToShake.position, Quaternion.identity);

            // 指定されたインターバルだけ待つ
            yield return new WaitForSeconds(prefabSpawnInterval);
        }
    }

    // プレハブ生成コルーチンを停止
    private void StopPrefabSpawn()
    {
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }
    }
}
