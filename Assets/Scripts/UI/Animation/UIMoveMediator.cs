using UnityEngine;

public class UIMoveMediator : MonoBehaviour
{
    [SerializeField] private UIMoveRight moveRight; // 右に動かすクラス
    [SerializeField] private UIMoveLeft moveLeft;   // 左に動かすクラス

    [SerializeField] Color onColor;
    [SerializeField] Color offColor;

    [SerializeField] PlayerImageAnimator animator;

    [SerializeField] bool moveRightNext; // 次にどちらに動かすかを管理するフラグ

    private int toggleCounter = 0;

    private void Start()
    {
        toggleCounter = 0;
    }
    private void LateUpdate()
    {
        if (!GameStateManager.Instance.IsBoardSetupComplete) return;

        var turnMana = GameTurnManager.Instance;
        if (turnMana.IsCurrentTurn(GameTurnManager.TurnState.PlayerPlacePiece) && GameTurnManager.Instance.IsTurnChanging)
        {
            toggleCounter++;

            if (toggleCounter == 3)
                MoveToggle();
        }

        if (turnMana.IsCurrentTurn(GameTurnManager.TurnState.OpponentPlacePiece) && GameTurnManager.Instance.IsTurnChanging)
        {
            toggleCounter++;

            if (toggleCounter == 3)
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
        animator.ChangeSpritesColor(moveRightNext ? offColor : onColor, 0.3f);
        toggleCounter = 0;

    }
}
