using UnityEngine;
using DG.Tweening;

public class MoveAndReturn : SingletonMonoBehaviour<MoveAndReturn>
{
    [Header("移動設定")]
    [SerializeField] private Transform startPoint; // 開始地点
    [SerializeField] private Transform endPoint;   // 目的地
    [SerializeField] private float moveDuration = 2f; // 目的地までの移動時間
    [SerializeField] private float waitTime = 1f; // 目的地での待機時間

    [Header("戻る設定")]
    [SerializeField] private float returnDuration = 2f; // 元の位置に戻る時間

    [Header("アニメーション対象")]
    [SerializeField] private GameObject targetObject; // アニメーションをかけるオブジェクト

    private Sequence moveSequence;

    private void Start()
    {
        // アニメーション対象オブジェクトを非アクティブに設定
        if (targetObject != null)
        {
            targetObject.transform.position = startPoint.position;
            targetObject.SetActive(false);
        }
        // 移動シーケンスの作成
        CreateMovementSequence();
    }

    private void CreateMovementSequence()
    {
        // 既存のシーケンスがあれば、削除する
        if (moveSequence != null)
        {
            moveSequence.Kill();
        }

        // DOTweenを使用して移動シーケンスを作成
        moveSequence = DOTween.Sequence()
            .Append(targetObject.transform.DOMove(endPoint.position, moveDuration).SetEase(Ease.InOutQuad)) // 移動
            .AppendInterval(waitTime) // 待機
            .Append(targetObject.transform.DOMove(startPoint.position, returnDuration).SetEase(Ease.InOutQuad)) // 戻る
            .OnComplete(OnAnimationComplete) // アニメーション完了時にメソッドを呼び出す
            .SetAutoKill(false) // シーケンスを自動的に削除しない
            .Pause(); // シーケンスを一時停止しておく
    }

    public void StartAnimation()
    {
        if (targetObject != null)
        {
            // アニメーション対象オブジェクトをアクティブにする
            targetObject.SetActive(true);
        }

        if (!moveSequence.IsPlaying())
        {
            moveSequence.Restart();
        }
    }

    private void OnAnimationComplete()
    {
        if (targetObject != null)
        {
            // アニメーション完了後、オブジェクトを非アクティブにする
            targetObject.SetActive(false);
        }

        ScenesLoader.Instance.LoadGameOver(0);
    }

    public void StopAnimation()
    {
        if (moveSequence != null)
        {
            moveSequence.Pause();
        }

        if (targetObject != null)
        {
            targetObject.transform.position = startPoint.position;
            // アニメーション停止後、オブジェクトを非アクティブにする
            targetObject.SetActive(false);
        }
    }
}
