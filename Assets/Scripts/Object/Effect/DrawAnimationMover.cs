using DG.Tweening;
using UnityEngine;

public class DrawAnimationMover : MonoBehaviour
{
    // 2つのオブジェクトを登録するためのフィールド
    [SerializeField] private Transform player1Object; // プレイヤー1のオブジェクト
    [SerializeField] private Transform player2Object; // プレイヤー2のオブジェクト
    [SerializeField] private Transform leftPanel;     // 左パネル
    [SerializeField] private Transform rightPanel;    // 右パネル

    // アニメーションの設定
    [SerializeField] private Vector3 player1OutwardVector = new Vector3(5f, 0f, 0f); // Player1の行きのベクトル
    [SerializeField] private Vector3 player1ReturnVector = new Vector3(-3f, 0f, 0f); // Player1の帰りのベクトル
    [SerializeField] private Vector3 player2OutwardVector = new Vector3(5f, 0f, 0f); // Player2の行きのベクトル
    [SerializeField] private Vector3 player2ReturnVector = new Vector3(-3f, 0f, 0f); // Player2の帰りのベクトル
    [SerializeField] private Vector3 panelCloseVector = new Vector3(-5f, 0f, 0f);   // パネルが閉じる方向のベクトル
    [SerializeField] private float outwardDuration = 2f; // 行きのアニメーションの時間
    [SerializeField] private float returnDuration = 1.5f; // 帰りのアニメーションの時間
    [SerializeField] private float panelCloseDuration = 1f; // パネルが閉じるアニメーションの時間
    [SerializeField] private Ease startEase = Ease.Linear; // イージングのタイプ
    [SerializeField] private Ease returnEase = Ease.Linear; // 帰りのイージングのタイプ
    [SerializeField] private Ease panelCloseEase = Ease.InOutQuad; // パネルの閉じるアニメーションのイージング

    private bool hasMovedOutward = false;

    /// <summary>
    /// 行きのアニメーションを実行
    /// </summary>
    public void MoveOutward()
    {
        // Player1の行きのアニメーション
        player1Object.DOMove(player1Object.position + player1OutwardVector, outwardDuration)
            .SetEase(startEase);

        // Player2の行きのアニメーション
        player2Object.DOMove(player2Object.position - player2OutwardVector, outwardDuration)
            .SetEase(startEase)
            .OnComplete(() => MoveReturn());

        hasMovedOutward = true;
    }

    /// <summary>
    /// 帰りのアニメーションを実行
    /// </summary>
    private void MoveReturn()
    {
        if (!hasMovedOutward) return;

        // Player1の帰りのアニメーション
        player1Object.DOMove(player1Object.position + player1ReturnVector, returnDuration)
            .SetEase(returnEase);

        // Player2の帰りのアニメーション
        player2Object.DOMove(player2Object.position - player2ReturnVector, returnDuration)
            .SetEase(returnEase)
            .OnComplete(() => ClosePanels());
    }

    /// <summary>
    /// 左右パネルが閉じるアニメーションを再生するメソッド
    /// </summary>
    private void ClosePanels()
    {
        // 左パネルが閉じるアニメーション
        leftPanel.DOLocalMove(leftPanel.localPosition + panelCloseVector, panelCloseDuration)
            .SetEase(panelCloseEase);

        // 右パネルが閉じるアニメーション（逆方向）
        rightPanel.DOLocalMove(rightPanel.localPosition - panelCloseVector, panelCloseDuration)
            .SetEase(panelCloseEase)
            .OnComplete(() => ScenesLoader.Instance.LoadGameOver(Color.black));
    }
}
