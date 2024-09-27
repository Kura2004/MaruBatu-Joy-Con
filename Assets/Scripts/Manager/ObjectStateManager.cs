using UnityEngine;
using DG.Tweening; // DOTweenを使うための名前空間

public class ObjectStateManager : SingletonMonoBehaviour<ObjectStateManager>
{
    [SerializeField]
    private string firstObjectTag = "FirstObjectTag"; // 1つ目のオブジェクトのタグ

    [SerializeField]
    private string secondObjectTag = "SecondObjectTag"; // 2つ目のオブジェクトのタグ

    [SerializeField]
    private float delayBeforeFinding = 1f; // タグでオブジェクトを探す前の遅延時間（秒）

    [Header("上下運動の設定")]
    [SerializeField] private float moveDuration = 2f;  // 移動にかかる時間
    [SerializeField] private float moveDistance = 2f;  // 移動する範囲の距離

    private GameObject firstObject; // 1つ目のオブジェクト
    private GameObject secondObject; // 2つ目のオブジェクト

    private void Start()
    {
        // 遅延してタグのゲームオブジェクトを探す処理を開始
        Invoke(nameof(FindTaggedObjects), delayBeforeFinding);
    }

    private void FindTaggedObjects()
    {
        // タグでゲームオブジェクトを検索
        firstObject = GameObject.FindGameObjectWithTag(firstObjectTag);
        secondObject = GameObject.FindGameObjectWithTag(secondObjectTag);

        if (secondObject != null)
        {
            secondObject.SetActive(false); // 最初に2つ目のオブジェクトを非アクティブに設定
        }
    }

    /// <summary>
    /// 1つ目のオブジェクトを現在の位置から指定範囲で上下運動させる
    /// </summary>
    public void MoveFirstObjectUpDown(bool isActive)
    {
        if (firstObject != null)
        {
            // 現在の位置を基準に上下運動をする
            firstObject.transform.DOMoveY(firstObject.transform.position.y + moveDistance, moveDuration)
                .SetLoops(2, LoopType.Yoyo) // 1回上がって戻る動きを設定
                .OnComplete(() => SetFirstObjectActive(isActive)); // 元の位置に戻ったらアクティブ状態を切り替える
        }
        else
        {
            Debug.LogWarning("FirstObjectが設定されていません。");
        }
    }

    /// <summary>
    /// 2つ目のオブジェクトを現在の位置から指定範囲で上下運動させる
    /// </summary>
    public void MoveSecondObjectUpDown(bool isActive)
    {
        if (secondObject != null)
        {
            // 現在の位置を基準に上下運動をする
            secondObject.transform.DOMoveY(secondObject.transform.position.y + moveDistance, moveDuration)
                .SetLoops(2, LoopType.Yoyo) // 1回上がって戻る動きを設定
                .OnComplete(() => SetSecondObjectActive(isActive)); // 元の位置に戻ったらアクティブ状態を切り替える
        }
        else
        {
            Debug.LogWarning("SecondObjectが設定されていません。");
        }
    }

    /// <summary>
    /// 1つ目と2つ目のオブジェクトのアクティブ状態をトグルする
    /// </summary>
    public void ToggleObjects(bool isFirstActive, bool isSecondActive)
    {
        SetFirstObjectActive(isFirstActive);
        SetSecondObjectActive(isSecondActive);
    }

    private void SetFirstObjectActive(bool isActive)
    {
        if (firstObject != null)
        {
            firstObject.SetActive(isActive);
        }
        else
        {
            Debug.LogWarning("FirstObjectが設定されていません。");
        }
    }

    private void SetSecondObjectActive(bool isActive)
    {
        if (secondObject != null)
        {
            secondObject.SetActive(isActive);
        }
        else
        {
            Debug.LogWarning("SecondObjectが設定されていません。");
        }
    }
}
