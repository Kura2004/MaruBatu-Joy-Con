using DG.Tweening;
using UnityEngine;

public class MainGameOverManager : MonoBehaviour
{
    public static bool loadGameOver = false;

    private float resetDelay = 0.03f; // ResetBoardSetupを呼び出すまでの遅延時間

    // ボードリセットの処理をメソッド化
    private void ExecuteResetBoardSetup()
    {
        GameStateManager.Instance.ResetBoardSetup();
        TimeLimitController.Instance.ResetEffect();
    }

    // ゲームオーバーを実行するメソッド
    public void ExecuteGameOver()
    {
        loadGameOver = true;
        TimeLimitController.Instance.StopTimer();
        //ScenesAudio.WinSe();

        // DOTweenを使って指定した遅延時間後にフラグを立てる
        DOTween.Sequence().AppendInterval(resetDelay).AppendCallback(() =>
        {
            ExecuteResetBoardSetup();
        });

        ScenesLoader.Instance.LoadGameOver(1.0f); // GameOverシーンをロード
    }

    // プレイヤーと相手の状態を確認してゲームオーバーを管理
    private void Update()
    {
        if (loadGameOver) return; // ゲームオーバーが既に実行されていたら処理しない
        var gameState = GameStateManager.Instance;

        // プレイヤーが勝利した場合
        if (gameState.IsPlayerWin)
        {
            MoveHorizontally.Instance.MoveRight();
            ExecuteGameOver();
            return;
        }

        // 相手が勝利した場合
        if (gameState.IsOpponentWin)
        {
            MoveHorizontally.Instance.MoveLeft();
            ExecuteGameOver();
            return;
        }
    }

    // 両方が勝利状態の場合、ゲームオーバーを実行
    private void LateUpdate()
    {
        if (loadGameOver) return; // ゲームオーバーが既に実行されていたら処理しない
        var gameState = GameStateManager.Instance;

        if (gameState.AreBothPlayersWinning() || GameTurnManager.Instance.IsGameEnd())
        {
            ExecuteGameOver();
            return;
        }
    }

    // 非アクティブになるときにloadGameOverをfalseにリセット
    private void OnDisable()
    {
        loadGameOver = false;
    }
}
