using UnityEngine;
using DG.Tweening;

public class ObjectScaler : MonoBehaviour
{
    [SerializeField]
    private Transform targetObject; // 操作対象のオブジェクト

    [SerializeField]
    private float scaleRate = 1.3f; // 拡大率

    [SerializeField]
    private float scaleDuration = 0.3f; // 拡大・縮小にかかる時間

    [SerializeField]
    private string targetTag = "Player"; // 触れる対象のタグ

    private Vector3 originalScale; // 元のスケール
    private Vector3 enlargedScale; // 拡大後のスケール

    // タグを持つオブジェクトが触れているかのフラグ
    private bool isTouchingTarget = false;

    private Tween scaleTween; // 現在のスケールアニメーション用Tween

    private void OnEnable()
    {
        if (targetObject == null)
        {
            targetObject = transform;
        }

        // 元のスケールを保存
        originalScale = targetObject.localScale;
        enlargedScale = originalScale * scaleRate; // 拡大後のスケール
    }

    // タグを持つオブジェクトが触れたときに呼ばれるメソッド
    private void OnTriggerEnter(Collider other)
    {
        if ((GameTurnManager.Instance.IsCurrentTurn(GameTurnManager.TurnState.OpponentRotateGroup) ||
            GameTurnManager.Instance.IsCurrentTurn(GameTurnManager.TurnState.PlayerRotateGroup))) return;
        if (other.CompareTag(targetTag) && CanProcessInput())
        {
            isTouchingTarget = true;
            EnlargeObject();
        }
    }

    // タグを持つオブジェクトが離れたときに呼ばれるメソッド
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(targetTag) && CanProcessInput())
        {
            isTouchingTarget = false;
            ResetObjectSize();
        }
    }

    // タグを持つオブジェクトが触れている場合にのみ処理を実行するメソッド
    private void LateUpdate()
    {
        //if (TimeControllerToggle.isTimeStopped || GameStateManager.Instance.IsRotating)
        //{
        //    return;
        //}

        //if (isTouchingTarget)
        //{
        //    EnlargeObject();
        //}
        //else
        //{
        //    ResetObjectSize();
        //}
    }

    // オブジェクトのサイズを大きくするメソッド
    private void EnlargeObject()
    {
        // 現在のアニメーションを停止
        if (scaleTween != null && scaleTween.IsPlaying())
        {
            scaleTween.Kill();
        }

        scaleTween = targetObject.DOScale(enlargedScale, scaleDuration).SetEase(Ease.OutBack);
    }

    // オブジェクトのサイズを徐々に元に戻すメソッド
    private void ResetObjectSize()
    {
        // 現在のアニメーションを停止
        if (scaleTween != null && scaleTween.IsPlaying())
        {
            scaleTween.Kill();
        }

        scaleTween = targetObject.DOScale(originalScale, scaleDuration).SetEase(Ease.InOutQuad);
    }

    // 引数で指定したスケールに設定するメソッド
    public void SetObjectScale(bool isEnlarged)
    {
        // 現在のアニメーションを停止
        if (scaleTween != null && scaleTween.IsPlaying())
        {
            scaleTween.Kill();
        }

        Vector3 targetScale = isEnlarged ? enlargedScale : originalScale;
        scaleTween = targetObject.DOScale(targetScale, scaleDuration).SetEase(isEnlarged ? Ease.OutBack : Ease.InOutQuad);
    }

    // 入力を処理できるかどうかを判定するメソッド
    private bool CanProcessInput()
    {
        return !TimeControllerToggle.isTimeStopped && !GameStateManager.Instance.IsRotating;
    }
}
