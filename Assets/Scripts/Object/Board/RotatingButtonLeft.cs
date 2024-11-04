using DG.Tweening;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RotatingButtonLeft : MonoBehaviour
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
        return !GameStateManager.Instance.IsBoardSetupComplete;
    }

    private bool isWithinTrigger = false;  // �^�O�ɓ������Ă��邩�ǂ�����\���t���O

    private void OnTriggerEnter(Collider other)
    {
        // �^�O����v����I�u�W�F�N�g�ɓ��������ꍇ
        if (other.CompareTag(selecterTag))
        {
            isWithinTrigger = true;  // �����������Ƀt���O��true��
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // �^�O����v����I�u�W�F�N�g���痣�ꂽ�ꍇ
        if (other.CompareTag(selecterTag))
        {
            isWithinTrigger = false;  // ���ꂽ���Ƀt���O��false��
        }
    }

    private void Update()
    {
        if (IsInteractionBlocked() || !isWithinTrigger)
        {
            return;  // �C���^���N�V�������u���b�N����Ă��邩�A�^�O�ɓ������Ă��Ȃ��ꍇ�͏����𒆒f
        }

        // 2P��SL�{�^���ŉ�]
        if (GameTurnManager.Instance.IsCurrentTurn(GameTurnManager.TurnState.OpponentRotateGroup) &&
            rightJoycon != null && rightJoycon.GetButtonDown(Joycon.Button.SL))
        {
            HandleClickInteraction(false);
        }

        // 1P��SL�{�^���ŉ�]
        if (GameTurnManager.Instance.IsCurrentTurn(GameTurnManager.TurnState.PlayerRotateGroup) &&
            leftJoycon != null && leftJoycon.GetButtonDown(Joycon.Button.SL))
        {
            HandleClickInteraction(true);
        }

        // �e�X�g�p
        if (Input.GetKeyDown(KeyCode.X))
        {
            HandleClickInteraction(false);
        }
    }


    private void HandleClickInteraction(bool isLeft)
    {
        if (!rotatingManager.AnyMassClicked() ||
               !rotatingManager.isSelected)
        {
            ScenesAudio.BlockedSe();
            return;
        }

        TimeLimitController.Instance.StopTimer();
        rotatingManager.StartRotationLeft(() => {

            if (isLeft) leftJoycon.SetRumble(160, 320, 10, 50);
            else rightJoycon.SetRumble(160, 320, 10, 50);

            Debug.Log("Left rotation completed.");
        });
    }
}