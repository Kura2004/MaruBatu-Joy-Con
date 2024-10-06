using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class MouseInteractionWithTurnManager : MonoBehaviour
{
    [SerializeField]
    private ObjectColorChanger colorChanger;

    private List<Joycon> joycons;
    private Joycon leftJoycon;
    private Joycon rightJoycon;

    private void Start()
    {
        if (colorChanger == null)
        {
            Debug.LogError("ObjectColorChanger �R���|�[�l���g���ݒ肳��Ă��܂���");
            return;
        }

        colorChanger.ChangeHoverColor(GlobalColorManager.Instance.currentColor);

        // Joy-Con�̏�����
        joycons = JoyconManager.Instance.j;
        if (joycons.Count >= 1)
        {
            leftJoycon = joycons.Find(c => c.isLeft);
            rightJoycon = joycons.Find(c => !c.isLeft);
        }
    }

    private void LateUpdate()
    {
        if (colorChanger.isClicked) return;
        if (colorChanger.hoverAndClickColor != GlobalColorManager.Instance.currentColor)
        {
            colorChanger.ChangeHoverColor(GlobalColorManager.Instance.currentColor);
        }
    }

    public bool IsInteractionBlocked()
    {
        var stateManager = GameStateManager.Instance;
        return TimeControllerToggle.isTimeStopped ||
               !stateManager.IsBoardSetupComplete || stateManager.IsRotating;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("MassSelecter") || IsInteractionBlocked()) return;

        // 2P�̌���{�^���iA�{�^���j�ŃC���^���N�V����
        if (GameTurnManager.Instance.IsCurrentTurn(GameTurnManager.TurnState.OpponentPlacePiece) &&
            rightJoycon != null && rightJoycon.GetButtonDown(Joycon.Button.DPAD_UP))
        {
            HandleInteraction();
        }

        // 1P�̌���{�^���iA�{�^���j�ŃC���^���N�V����
        if (GameTurnManager.Instance.IsCurrentTurn(GameTurnManager.TurnState.PlayerPlacePiece) &&
            leftJoycon != null && leftJoycon.GetButtonDown(Joycon.Button.DPAD_DOWN))
        {
            HandleInteraction();
        }

        // �e�X�g�p
        if (Input.GetKeyDown(KeyCode.Return))
        {
            HandleInteraction();
        }
    }

    private void HandleInteraction()
    {
        if (IsInteractionBlocked() || colorChanger.isClicked)
        {
            ScenesAudio.BlockedSe();
            return;
        }

        ScenesAudio.ClickSe();
        colorChanger.HandleClick();
        GameTurnManager.Instance.SetTurnChange(true);
        GameTurnManager.Instance.AdvanceTurn();
        UpdateColorBasedOnTurn();
        ObjectStateManager.Instance.MoveFirstObjectUpDown(false);
        ObjectStateManager.Instance.MoveSecondObjectUpDown(true);
    }

    private void UpdateColorBasedOnTurn()
    {
        GlobalColorManager.Instance.UpdateColorBasedOnTurn();
    }
}
