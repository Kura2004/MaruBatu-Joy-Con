using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class RotatingButtonRight : MonoBehaviour
{
    [SerializeField]
    private RotatingMassObjectManager rotatingManager;
    [SerializeField]
    string selecterTag = "Def";

    private List<Joycon> joycons;
    private Joycon leftJoycon;
    private Joycon rightJoycon;

    private void Start()
    {

        // Joy-Con�̏�����
        joycons = JoyconManager.Instance.j;
        if (joycons.Count >= 1)
        {
            leftJoycon = joycons.Find(c => c.isLeft);
            rightJoycon = joycons.Find(c => !c.isLeft);
        }
    }

    private bool IsInteractionBlocked()
    {
        return //CanvasBounce.isBlocked ||
               //TimeControllerToggle.isTimeStopped ||
               !GameStateManager.Instance.IsBoardSetupComplete;
    }

    private void OnTriggerStay(Collider other)
    {
        if (IsInteractionBlocked() || !other.CompareTag(selecterTag))
        {
            return;
        }

        // 2P��SR�{�^���ŉ�]
        if (GameTurnManager.Instance.IsCurrentTurn(GameTurnManager.TurnState.OpponentRotateGroup) &&
            rightJoycon != null && rightJoycon.GetButtonDown(Joycon.Button.SR))
        {
            HandleClickInteraction();
        }

        // 1P��SR�{�^���ŉ�]
        if (GameTurnManager.Instance.IsCurrentTurn(GameTurnManager.TurnState.PlayerRotateGroup) &&
            leftJoycon != null && leftJoycon.GetButtonDown(Joycon.Button.SR))
        {
            HandleClickInteraction();
        }

        // �e�X�g�p
        if (Input.GetKeyDown(KeyCode.Z))
        {
            HandleClickInteraction();
        }
    }

    private void HandleClickInteraction()
    {
        if (!rotatingManager.AnyMassClicked() ||
               !rotatingManager.isSelected)
        {
            ScenesAudio.BlockedSe();
            return;
        }

        TimeLimitController.Instance.StopTimer();
        rotatingManager.StartRotationRight();
    }
}
