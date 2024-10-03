using UnityEngine;

public class UIMoveMediator : MonoBehaviour
{
    [SerializeField] private UIMoveRight moveRight; // 右に動かすクラス
    [SerializeField] private UIMoveLeft moveLeft;   // 左に動かすクラス

    [SerializeField] bool moveRightNext; // 次にどちらに動かすかを管理するフラグ
    private void LateUpdate()
    {
        if (!GameStateManager.Instance.IsBoardSetupComplete) return;

        var turnMana = GameTurnManager.Instance;
        if (turnMana.IsCurrentTurn(GameTurnManager.TurnState.PlayerPlacePiece) && GameTurnManager.Instance.IsTurnChanging)
        {
            MoveToggle();
        }

        if (turnMana.IsCurrentTurn(GameTurnManager.TurnState.OpponentPlacePiece) && GameTurnManager.Instance.IsTurnChanging)
        {
            MoveToggle();
        }
    }

    // 右に動かす
    public void MoveRight()
    {
        moveRight.StartMove();
    }

    // 左に動かす
    public void MoveLeft()
    {
        moveLeft.StartMove();
    }

    // 右左の切り替え
    public void MoveToggle()
    {
        if (moveRightNext)
        {
            MoveRight();
        }
        else
        {
            MoveLeft();
        }

        moveRightNext = !moveRightNext; // 次回は反対の方向に動かす
        
    }
}
