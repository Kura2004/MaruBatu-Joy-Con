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
        joycons = JoyconManager.Instance.j;
        if (joycons.Count >= 1)
        {
            leftJoycon = joycons.Find(c => c.isLeft);
            rightJoycon = joycons.Find(c => !c.isLeft);
        }
    }

    private bool IsInteractionBlocked()
    {
        return !GameStateManager.Instance.IsBoardSetupComplete;
    }

    private bool isWithinTrigger = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(selecterTag))
        {
            isWithinTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(selecterTag))
        {
            isWithinTrigger = false;
        }
    }

    private void Update()
    {
        if (IsInteractionBlocked() || !isWithinTrigger)
        {
            return;
        }

        if (GameTurnManager.Instance.IsCurrentTurn(GameTurnManager.TurnState.OpponentRotateGroup) &&
            rightJoycon != null && rightJoycon.GetButtonDown(Joycon.Button.SR))
        {
            HandleClickInteraction(false);
        }

        if (GameTurnManager.Instance.IsCurrentTurn(GameTurnManager.TurnState.PlayerRotateGroup) &&
            leftJoycon != null && leftJoycon.GetButtonDown(Joycon.Button.SR))
        {
            HandleClickInteraction(true);
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            HandleClickInteraction(false);
        }
    }

    private void HandleClickInteraction(bool isLeft)
    {
        if (!rotatingManager.AnyMassClicked() || !rotatingManager.isSelected)
        {
            ScenesAudio.BlockedSe();
            return;
        }

        TimeLimitController.Instance.StopTimer();
        rotatingManager.StartRotationRight(() => {

            if (isLeft) leftJoycon.SetRumble(160, 320, 10, 50);
            else rightJoycon.SetRumble(160, 320, 10, 50);
        });
    }
}
