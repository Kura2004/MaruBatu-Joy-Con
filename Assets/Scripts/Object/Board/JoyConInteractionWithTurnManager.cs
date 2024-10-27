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

        isClicked = false;
    }

    public bool IsInteractionBlocked()
    {
        var stateManager = GameStateManager.Instance;
        return !stateManager.IsBoardSetupComplete ||
               stateManager.IsRotating ||
               GameTurnManager.Instance.IsTurnChanging;
    }

    private bool isWithinTrigger = false;  // �^�O�ɓ������Ă��邩�ǂ�����\���t���O

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
            Debug.Log("�}�X���I������܂���");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // �^�O����v����I�u�W�F�N�g���痣�ꂽ�ꍇ
        if (other.CompareTag("MassSelecter"))
        {
            isWithinTrigger = false;  // ���ꂽ���Ƀt���O��false��
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
