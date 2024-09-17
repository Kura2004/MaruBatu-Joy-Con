using UnityEngine;
using DG.Tweening;

public class MoveHorizontally : SingletonMonoBehaviour<MoveHorizontally>
{
    [SerializeField]
    private float moveDistance = 5f; // 移動する距離
    [SerializeField]
    private float moveDuration = 2f; // 移動にかかる時間
    [SerializeField]
    private Ease moveEase = Ease.Linear; // イージングオプション
    private Transform objectTransform; // 通常のTransform

    protected override void OnEnable()
    {
        base.OnEnable();
        objectTransform = GetComponent<Transform>();
    }

    /// <summary>
    /// 指定した距離だけ右に移動するメソッド
    /// </summary>
    public void MoveRight()
    {
        objectTransform.DOMoveX(objectTransform.position.x + moveDistance, moveDuration).SetEase(moveEase);
    }

    /// <summary>
    /// 指定した距離だけ左に移動するメソッド
    /// </summary>
    public void MoveLeft()
    {
        objectTransform.DOMoveX(objectTransform.position.x - moveDistance, moveDuration).SetEase(moveEase);
    }
}