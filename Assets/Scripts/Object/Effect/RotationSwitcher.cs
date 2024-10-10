using DG.Tweening;
using UnityEngine;

public class RotationSwitcher : MonoBehaviour
{
    public enum RotationAxis
    {
        X,
        Y,
        Z
    }

    [SerializeField] private RotationAxis rotationAxis = RotationAxis.X; // 回転する軸
    [SerializeField] private float rotationAngle = 45f; // 回転角度
    [SerializeField] private float duration = 2f; // 回転にかかる時間
    [SerializeField] private Ease rotationEase = Ease.Linear; // 回転のイージング

    private float currentRotation;
    private bool isRotating = false;
    private int toggleCounter = 0;

    [SerializeField] DrawAnimationMover DrawAnimation;
    [SerializeField] UIMoveMediator[] moveMediator;
    private void Start()
    {
        currentRotation = 0f;
        toggleCounter = 0;
    }

    private void StartRotation()
    {
        if (isRotating)
            return;

        isRotating = true;
        Rotate();
    }

    private void Rotate()
    {
        float endRotation = currentRotation + rotationAngle;
        Tween rotationTween = CreateRotationTween(endRotation);

        rotationTween.OnComplete(() =>
        {
            // 正負を切り替える
            rotationAngle = -rotationAngle;
            currentRotation = endRotation;
            isRotating = false;
        });

        GameTurnManager.Instance.SetTurnChange(false);
        toggleCounter = 0;
    }

    private Tween CreateRotationTween(float endRotation)
    {
        Vector3 rotationVector = Vector3.zero;

        switch (rotationAxis)
        {
            case RotationAxis.X:
                rotationVector = new Vector3(rotationAngle, 0, 0);
                break;
            case RotationAxis.Y:
                rotationVector = new Vector3(0, rotationAngle, 0);
                break;
            case RotationAxis.Z:
                rotationVector = new Vector3(0, 0, rotationAngle);
                break;
        }

        return transform.DORotate(rotationVector, duration, RotateMode.LocalAxisAdd)
            .SetEase(rotationEase);
    }

    private void LateUpdate()
    {

        if (GameWinnerManager.Instance.IsCurrentWinner(GameWinnerManager.Winner.Draw))
        {
            ResetRotation();
        }

        if (!GameStateManager.Instance.IsBoardSetupComplete) return;

        var turnMana = GameTurnManager.Instance;
        if (turnMana.IsCurrentTurn(GameTurnManager.TurnState.PlayerPlacePiece) && GameTurnManager.Instance.IsTurnChanging)
        {
            toggleCounter++;

            if (toggleCounter == 2)
            {
                Debug.Log("1Pのターンです");
                for (int i = 0; i < moveMediator.Length; i++)
                    moveMediator[i].MoveToggle();
                StartRotation();
            }
        }

        if (turnMana.IsCurrentTurn(GameTurnManager.TurnState.OpponentPlacePiece) && GameTurnManager.Instance.IsTurnChanging)
        {
            toggleCounter++;

            if (toggleCounter == 2)
            {
                Debug.Log("相手のターンです");
                for (int i = 0; i < moveMediator.Length; i++)
                    moveMediator[i].MoveToggle();
                StartRotation();
            }
        }
    }

    private void ResetRotation()
    {
        if (isRotating)
            return;

        isRotating = true;
        float addAngle = currentRotation + rotationAngle / 2.0f;
        addAngle *= rotationAngle > 0 ? 1 : -1;

        // 回転を0度にリセットする
        transform.DORotate(new Vector3(0, 0, addAngle), duration
            , RotateMode.LocalAxisAdd).SetEase(rotationEase)
            .OnComplete(() =>
            {
                DrawAnimation.MoveOutward();
            });
    }
}