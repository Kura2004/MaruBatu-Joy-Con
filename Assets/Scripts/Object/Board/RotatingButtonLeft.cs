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

        // Joy-Conの初期化
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

    private bool isWithinTrigger = false;  // タグに当たっているかどうかを表すフラグ

    private void OnTriggerEnter(Collider other)
    {
        // タグが一致するオブジェクトに当たった場合
        if (other.CompareTag(selecterTag))
        {
            isWithinTrigger = true;  // 当たった時にフラグをtrueに
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // タグが一致するオブジェクトから離れた場合
        if (other.CompareTag(selecterTag))
        {
            isWithinTrigger = false;  // 離れた時にフラグをfalseに
        }
    }

    private void Update()
    {
        if (IsInteractionBlocked() || !isWithinTrigger)
        {
            return;  // インタラクションがブロックされているか、タグに当たっていない場合は処理を中断
        }

        // 2PのSLボタンで回転
        if (GameTurnManager.Instance.IsCurrentTurn(GameTurnManager.TurnState.OpponentRotateGroup) &&
            rightJoycon != null && rightJoycon.GetButtonDown(Joycon.Button.SL))
        {
            HandleClickInteraction(false);
        }

        // 1PのSLボタンで回転
        if (GameTurnManager.Instance.IsCurrentTurn(GameTurnManager.TurnState.PlayerRotateGroup) &&
            leftJoycon != null && leftJoycon.GetButtonDown(Joycon.Button.SL))
        {
            HandleClickInteraction(true);
        }

        // テスト用
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