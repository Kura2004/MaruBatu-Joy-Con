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

    private Vector3 originalScale; // 元のスケール
    private Vector3 enlargedScale; // 拡大後のスケール

    // マウス入力を無視するかどうかを管理する
    public bool ignoreMouseInput = false;

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

    // マウス入力が無視されていない場合で、時間が止まっていない場合に処理を実行するかどうかを判定するメソッド
    protected virtual bool CanProcessMouseInput()
    {
        return !ignoreMouseInput && !TimeControllerToggle.isTimeStopped && !GameStateManager.Instance.IsRotating;
    }

    // マウスがオブジェクトに触れた時に呼ばれるメソッド
    protected virtual void OnMouseEnter()
    {
        if (CanProcessMouseInput()) // マウス入力が無視されていない場合のみ処理を実行
        {
            EnlargeObject();
        }
    }

    // マウスがオブジェクト上にある間に呼ばれるメソッド
    protected virtual void OnMouseOver()
    {
        if (CanProcessMouseInput() && !targetObject.DOScale(enlargedScale, scaleDuration).IsPlaying()) // マウス入力が無視されておらず、アニメーションが実行中でない場合のみ処理を実行
        {
            EnlargeObject();
        }
    }

    // マウスがオブジェクトから離れた時に呼ばれるメソッド
    protected virtual void OnMouseExit()
    {
        if (CanProcessMouseInput()) // マウス入力が無視されていない場合のみ処理を実行
        {
            ResetObjectSize();
        }
    }

    // オブジェクトのサイズを大きくするメソッド
    protected virtual void EnlargeObject()
    {
        targetObject.DOScale(enlargedScale, scaleDuration).SetEase(Ease.OutBack);
    }

    // オブジェクトのサイズを徐々に元に戻すメソッド
    protected virtual void ResetObjectSize()
    {
        targetObject.DOScale(originalScale, scaleDuration).SetEase(Ease.InOutQuad);
    }

    // マウス入力を無視するメソッド
    public void IgnoreMouseInput()
    {
        ignoreMouseInput = true;
        // すべてのTweenを無効にする
        targetObject.DOKill();
    }

    // マウス入力を元に戻すメソッド
    public void ResumeMouseInput()
    {
        ignoreMouseInput = false;
    }

    // オブジェクトの大きさが元に戻っているかどうかを判定するメソッド
    protected virtual bool IsObjectSizeReset()
    {
        return targetObject.localScale != originalScale && !GetComponent<MouseHoverChecker>().IsMouseOver()
            && !targetObject.DOScale(originalScale, scaleDuration).IsActive();
    }
}
