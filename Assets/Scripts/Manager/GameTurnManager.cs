using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ゲームのターン管理を行うシングルトンなクラス
/// </summary>
public class GameTurnManager : SingletonMonoBehaviour<GameTurnManager>
{
    public enum TurnState
    {
        PlayerPlacePiece,
        PlayerRotateGroup,
        OpponentPlacePiece,
        OpponentRotateGroup
    }

    public TurnState CurrentTurnState { get; private set; }
    public bool IsTurnChanging { get; private set; }
    public bool IsGameStarted;

    public int TotalTurnCount { get; private set; }

    protected override void OnEnable()
    {
        base.OnEnable();
        SceneManager.sceneLoaded += OnSceneLoaded;
        Initialize();
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Initialize();
    }

    private void Initialize()
    {
        CurrentTurnState = TurnState.PlayerPlacePiece;
        IsTurnChanging = false;
        IsGameStarted = false;
        TotalTurnCount = 0;
    }

    public void AdvanceTurn()
    {
        TotalTurnCount++;
        switch (CurrentTurnState)
        {
            case TurnState.PlayerPlacePiece:
                CurrentTurnState = TurnState.PlayerRotateGroup;
                break;
            case TurnState.PlayerRotateGroup:
                CurrentTurnState = TurnState.OpponentPlacePiece;
                break;
            case TurnState.OpponentPlacePiece:
                CurrentTurnState = TurnState.OpponentRotateGroup;
                break;
            case TurnState.OpponentRotateGroup:
                CurrentTurnState = TurnState.PlayerPlacePiece;
                break;
        }
        Debug.Log("ターンが進行しました: " + CurrentTurnState);
    }

    public bool IsCurrentTurn(TurnState turnState)
    {
        return CurrentTurnState == turnState;
    }

    public void SetTurnState(TurnState newTurnState)
    {
        CurrentTurnState = newTurnState;
        Debug.Log("ターン状態が設定されました: " + CurrentTurnState);
    }

    public bool IsGameEnd()
    {
        return TotalTurnCount >= 16 * 2;
    }

    // TurnChangeを設定するメソッド
    public void SetTurnChange(bool value)
    {
        IsTurnChanging = value;
        Debug.Log("TurnChangeが設定されました: " + IsTurnChanging);
    }

}

