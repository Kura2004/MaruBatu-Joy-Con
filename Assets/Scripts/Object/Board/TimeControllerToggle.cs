using UnityEngine;
using DG.Tweening;  // DOTweenの名前空間をインポート
using Saito.SoundManager;

public class TimeControllerToggle : MonoBehaviour
{
    public static bool isTimeStopped = false;  // 時間が止まっているかどうかを管理する静的なbool型の変数

    [SerializeField, Tooltip("生成するオブジェクトのプレハブ")]
    private GameObject objectPrefab;  // 生成するオブジェクトのプレハブ

    [SerializeField, Tooltip("オブジェクトが大きくなるまでの時間")]
    private float growDuration = 2f;  // オブジェクトが大きくなるまでの時間

    [SerializeField, Tooltip("オブジェクトが成長する最終的なスケール")]
    private Vector3 targetScale = Vector3.one * 2f;  // オブジェクトが成長する最終的なスケール

    [SerializeField, Tooltip("オブジェクトの位置オフセット")]
    private Vector3 positionOffset = Vector3.zero;  // オブジェクトの位置オフセット

    [SerializeField, Tooltip("オブジェクトの初期スケール")]
    private Vector3 initialScale = Vector3.one;  // オブジェクトの初期スケール

    [SerializeField, Tooltip("パーティクルシステム")]
    private new ParticleSystem particleSystem;  // パーティクルシステム

    //[SerializeField]
    //ObjectShaker backShaker;

    private GameObject spawnedObject;  // 生成されたオブジェクト
    private Tweener currentTween;  // 現在のアニメーションを管理するTweener

    private void OnEnable()
    {
        isTimeStopped = false;
        // オブジェクトを生成し、初期化する
        if (spawnedObject == null && objectPrefab != null)
        {
            Vector3 createPos = transform.position + positionOffset;
            spawnedObject = Instantiate(objectPrefab, createPos, Quaternion.identity);
            spawnedObject.transform.localScale = initialScale;
        }
    }

    // オブジェクト上でマウスがクリックされた時に呼び出される
    void OnMouseDown()
    {
        if (TimeLimitController.Instance.isEffectTriggered) { return; }
        if (GameStateManager.Instance.IsBoardSetupComplete &&
            !GameStateManager.Instance.IsRotating)
        {
            isTimeStopped = !isTimeStopped;
            // 時間停止状態を切り替える
            if (!isTimeStopped)
            {
                EndTimeStop();
            }
            else
            {
                StartTimeStop();
            }
        }
    }

    // 時間停止を開始するメソッド
    private void StartTimeStop()
    {
        TimeLimitController.Instance.StopTimer();
        ScenesAudio.ShootSe();
        //ScenesAudio.PauseBgm();
        //backShaker.StopShake();

        // 現在のアニメーションを停止
        if (currentTween != null && currentTween.IsActive())
        {
            currentTween.Kill();
        }

        // パーティクルシステムを停止
        if (particleSystem != null)
        {
            particleSystem.Pause();
        }

        // オブジェクトを大きくする
        if (spawnedObject != null)
        {
            currentTween = spawnedObject.transform.DOScale(targetScale, growDuration).SetEase(Ease.OutExpo);
        }
    }

    // 時間停止を終了するメソッド
    private void EndTimeStop()
    {
        TimeLimitController.Instance.StartTimer();
        ScenesAudio.RestartSe();
        //ScenesAudio.UnPauseBgm();
        // 現在のアニメーションを停止
        if (currentTween != null && currentTween.IsActive())
        {
            currentTween.Kill();
        }

        // オブジェクトを小さくする
        if (spawnedObject != null)
        {
            currentTween = spawnedObject.transform.DOScale(initialScale, growDuration).SetEase(Ease.InExpo);
        }

        // パーティクルシステムを再開
        if (particleSystem != null)
        {
            particleSystem.Play();
        }
    }
}
