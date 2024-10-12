using DG.Tweening;
using UnityEngine;

public class MainGameOverManager : MonoBehaviour
{
    public static bool loadGameOver = false;
    int GameEndCounter = 0;
    [SerializeField] CanvasFader[] fadeUI;

    private void OnEnable()
    {
        GameEndCounter = 0;
        loadGameOver = false;
        //Invoke(nameof(ExecutePlayerWin), 7.0f);
    }

    private void ExecutePlayerWin()
    {
        MoveHorizontally.Instance.MoveRight();
        VictoryCameraAnimator.Instance.MoveCameraLeftToResetVictory();
        GameWinnerManager.Instance.SetWinner(GameWinnerManager.Winner.Player1);
        ExecuteGameOver();
    }

    private void ExecuteOpponentWin()
    {
        MoveHorizontally.Instance.MoveLeft();
        VictoryCameraAnimator.Instance.MoveCameraRightForVictory();
        GameWinnerManager.Instance.SetWinner(GameWinnerManager.Winner.Player2);
        ExecuteGameOver();
    }

    private void ExecuteDraw()
    {
        GameWinnerManager.Instance.SetWinner(GameWinnerManager.Winner.Draw);
        ExecuteGameOver();
    }

    // ゲームオーバーを実行するメソッド
    private void ExecuteGameOver()
    {
        loadGameOver = true;
        for (int i = 0; i < fadeUI.Length; i++)
            fadeUI[i].HideCanvas();
        GameStateManager.Instance.ResetBoardSetup();
        TimeLimitController.Instance.ResetEffect();
        TimeLimitController.Instance.StopTimer();
        //ScenesAudio.WinSe();
    }

    // プレイヤーと相手の状態を確認してゲームオーバーを管理
    private void Update()
    {
        if (loadGameOver) return; // ゲームオーバーが既に実行されていたら処理しない
        var gameState = GameStateManager.Instance;

        // プレイヤーが勝利した場合
        if (gameState.IsPlayerWin)
        {
            ExecutePlayerWin();
            return;
        }

        // 相手が勝利した場合
        if (gameState.IsOpponentWin)
        {
            ExecuteOpponentWin();
            return;
        }

        if (GameTurnManager.Instance.IsGameEnd())
        {
            GameEndCounter++;
            if (GameEndCounter == 2)
            {
                ExecuteDraw();
                return;
            }
        }
    }

    // 両方が勝利状態の場合、ゲームオーバーを実行
    private void LateUpdate()
    {
        if (loadGameOver) return; // ゲームオーバーが既に実行されていたら処理しない
        var gameState = GameStateManager.Instance;

        if (gameState.AreBothPlayersWinning())
        {
            ExecuteDraw();
            return;
        }
    }

    // 非アクティブになるときにloadGameOverをfalseにリセット
    private void OnDisable()
    {
        loadGameOver = false;
    }
}
