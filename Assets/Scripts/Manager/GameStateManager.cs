using UnityEngine;
using System.Collections;

public class GameStateManager : SingletonMonoBehaviour<GameStateManager>
{
    // 回転状態を示すプロパティ
    public bool IsRotating { get; private set; } = false;

    public bool IsBoardSetupComplete { get; private set; } = false;

    // プレイヤーの勝利状態
    public bool IsPlayerWin { get; private set; } = false;

    // 相手の勝利状態
    public bool IsOpponentWin { get; private set; } = false;

    protected override void OnEnable()
    {
        base.OnEnable();
        IsRotating = false;
        IsBoardSetupComplete = false;
        IsPlayerWin = false;
        IsOpponentWin = false;
    }

    // 回転を開始するメソッド
    public void StartRotation()
    {
        Debug.Log("回転を始めます");
        IsRotating = true;
    }

    // 回転を終了するメソッド
    public void EndRotation()
    {
        IsRotating = false;
    }

    // 盤面セット完了のフラグを更新するメソッド
    private void SetBoardSetupComplete(bool isComplete)
    {
        IsBoardSetupComplete = isComplete;

        Debug.Log("Board setup complete status: " + IsBoardSetupComplete);
    }

    // 盤面セットを開始するメソッド（引数としてセットアップ完了までの秒数を受け取る）
    public void StartBoardSetup(float setupDuration)
    {
        ScenesAudio.UnPauseBgm();
        StartCoroutine(BoardSetupCoroutine(setupDuration));
    }

    // コルーチンで非同期に盤面セットアップ処理を行う
    private IEnumerator BoardSetupCoroutine(float setupDuration)
    {
        Debug.Log("Starting board setup...");

        yield return new WaitForSeconds(setupDuration);

        TimeLimitController.Instance.StartTimer();
        SetBoardSetupComplete(true);
    }

    // 盤面セット完了フラグをリセットするメソッド
    public void ResetBoardSetup()
    {
        IsBoardSetupComplete = false;
        Debug.Log("Board setup has been reset.");
    }

    // プレイヤーの勝利状態を設定するメソッド
    public void SetPlayerWin(bool isWin)
    {
        IsPlayerWin = isWin;
        Debug.Log("Player win status: " + IsPlayerWin);
    }

    // 相手の勝利状態を設定するメソッド
    public void SetOpponentWin(bool isWin)
    {
        IsOpponentWin = isWin;
        Debug.Log("Opponent win status: " + IsOpponentWin);
    }

    // 両者が勝利した場合に true を返すメソッド
    public bool AreBothPlayersWinning()
    {
        return IsPlayerWin && IsOpponentWin;
    }
}
