using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class JoyConInteractionWithTurnManager : MonoBehaviour
{
    [SerializeField]
    private ObjectColorChanger colorChanger;

    private List<Joycon> joycons;
    private Joycon leftJoycon;
    private Joycon rightJoycon;

    [SerializeField] FrameColorChanger frame;
    [SerializeField] ObjectScaler frameScaler;

    public bool isClicked { get; private set; } = false;

    private void Start()
    {
        if (colorChanger == null)
        {
            Debug.LogError("ObjectColorChanger コンポーネントが設定されていません");
            return;
        }

        colorChanger.ChangeHoverColor(GlobalColorManager.Instance.currentColor);

        // Joy-Conの初期化
        joycons = JoyconManager.Instance.j;
        if (joycons.Count >= 1)
        {
            leftJoycon = joycons.Find(c => c.isLeft);
            rightJoycon = joycons.Find(c => !c.isLeft);
        }

        isClicked = false;
    }

    public bool IsInteractionBlocked()
    {
        var stateManager = GameStateManager.Instance;
        return !stateManager.IsBoardSetupComplete ||
               stateManager.IsRotating ||
               GameTurnManager.Instance.IsTurnChanging;
    }

    private bool isWithinTrigger = false;  // タグに当たっているかどうかを表すフラグ

    private void OnTriggerEnter(Collider other)
    {
        if (GameTurnManager.Instance.IsCurrentTurn(GameTurnManager.TurnState.OpponentRotateGroup) ||
GameTurnManager.Instance.IsCurrentTurn(GameTurnManager.TurnState.PlayerRotateGroup) ||
IsInteractionBlocked()) return;

        if (other.CompareTag("MassSelecter"))
        {
            isWithinTrigger = true;
            frame.ChangeColorChange();
            frameScaler.EnlargeObject();
            Debug.Log("マスが選択されました");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // タグが一致するオブジェクトから離れた場合
        if (other.CompareTag("MassSelecter"))
        {
            isWithinTrigger = false;  // 離れた時にフラグをfalseに
            frame.ChangeColorNormal();
            frameScaler.ResetObjectSize();
        }
    }

    private void LateUpdate()
    {
        if (isClicked) return;

        if (colorChanger.hoverAndClickColor != GlobalColorManager.Instance.currentColor)
        {
            colorChanger.ChangeHoverColor(GlobalColorManager.Instance.currentColor);
        }
    }

    [SerializeField] private int rumbleDuration = 0;
    [SerializeField] private float rumbleAmp = 0;
    private void Update()
    {
        if (GameTurnManager.Instance.IsCurrentTurn(GameTurnManager.TurnState.OpponentRotateGroup) ||
    GameTurnManager.Instance.IsCurrentTurn(GameTurnManager.TurnState.PlayerRotateGroup) ||
    GameTurnManager.Instance.IsTurnChanging) return;

        if (isWithinTrigger)
        {
            if (GameTurnManager.Instance.IsCurrentTurn(GameTurnManager.TurnState.OpponentPlacePiece) &&
                rightJoycon != null && rightJoycon.GetButtonDown(Joycon.Button.DPAD_UP))
            {
                rightJoycon.SetRumble(160, 320, rumbleAmp, rumbleDuration);
                HandleInteraction();
            }

            if (GameTurnManager.Instance.IsCurrentTurn(GameTurnManager.TurnState.PlayerPlacePiece) &&
                leftJoycon != null && leftJoycon.GetButtonDown(Joycon.Button.DPAD_DOWN))
            {
                leftJoycon.SetRumble(160, 320, rumbleAmp, rumbleDuration);
                HandleInteraction();
            }
        }
    }

    private void HandleInteraction()
    {
        if (IsInteractionBlocked()) return;
        if (isClicked)
        {
            ScenesAudio.BlockedSe();
            return;
        }

        isClicked = true;
        ScenesAudio.ClickSe();
        colorChanger.HandleClick();
        GameTurnManager.Instance.SetTurnChange(true);
        GameTurnManager.Instance.AdvanceTurn();
        UpdateColorBasedOnTurn();
        ObjectStateManager.Instance.MoveFirstObjectUpDown(false);
        ObjectStateManager.Instance.MoveSecondObjectUpDown(true);
        frame.ChangeColorNormal();
    }

    private void UpdateColorBasedOnTurn()
    {
        GlobalColorManager.Instance.UpdateColorBasedOnTurn();
    }
}
