using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class MainGameOverManager : MonoBehaviour
{
    public static bool loadGameOver = false;
    int GameEndCounter = 0;
    [SerializeField] CanvasFader[] fadeUI;
    private List<Joycon> joycons; // JoyCon�̃��X�g
    private Joycon joyconL; // ��JoyCon�̃C���X�^���X
    private Joycon joyconR;


    private void OnEnable()
    {
        GameEndCounter = 0;
        loadGameOver = false;
        // Joy-Con�̏�����
        joycons = JoyconManager.Instance.j;
        if (joycons.Count >= 1)
        {
            joyconL = joycons.Find(c => c.isLeft);
            joyconR = joycons.Find(c => !c.isLeft);
        }

        //Invoke(nameof(ExecutePlayerWin), 7.0f);
    }

    private void ExecutePlayerWin()
    {
        MoveHorizontally.Instance.MoveRight();
        VictoryCameraAnimator.Instance.MoveCameraLeftToResetVictory();
        GameWinnerManager.Instance.SetWinner(GameWinnerManager.Winner.Player1);
        ExecuteGameOver();
    }

    private void ExecuteOpponentWin()
    {
        MoveHorizontally.Instance.MoveLeft();
        VictoryCameraAnimator.Instance.MoveCameraRightForVictory();
        GameWinnerManager.Instance.SetWinner(GameWinnerManager.Winner.Player2);
        ExecuteGameOver();
    }

    private void ExecuteDraw()
    {
        GameWinnerManager.Instance.SetWinner(GameWinnerManager.Winner.Draw);
        ExecuteGameOver();
    }

    private void ExecuteGameOver()
    {
        loadGameOver = true;
        for (int i = 0; i < fadeUI.Length; i++)
            fadeUI[i].HideCanvas();
        GameStateManager.Instance.ResetBoardSetup();
        TimeLimitController.Instance.ResetEffect();
        TimeLimitController.Instance.StopTimer();

        // �U��������0.1�b��Ɏ��s
        Invoke(nameof(TriggerJoyconRumble), 0.1f);
        // ScenesAudio.WinSe();
    }

    private void TriggerJoyconRumble()
    {
        joyconL.SetRumble(160, 320, 10, 3500);
        joyconR.SetRumble(160, 320, 10, 3500);
    }


    // �v���C���[�Ƒ���̏�Ԃ��m�F���ăQ�[���I�[�o�[���Ǘ�
    private void Update()
    {
        if (loadGameOver) return; // �Q�[���I�[�o�[�����Ɏ��s����Ă����珈�����Ȃ�
        var gameState = GameStateManager.Instance;

        // �v���C���[�����������ꍇ
        if (gameState.IsPlayerWin)
        {
            ExecutePlayerWin();
            return;
        }

        // ���肪���������ꍇ
        if (gameState.IsOpponentWin)
        {
            ExecuteOpponentWin();
            return;
        }

        if (GameTurnManager.Instance.IsGameEnd())
        {
            GameEndCounter++;
            if (GameEndCounter == 2)
            {
                ExecuteDraw();
                return;
            }
        }
    }

    // ������������Ԃ̏ꍇ�A�Q�[���I�[�o�[�����s
    private void LateUpdate()
    {
        if (loadGameOver) return; // �Q�[���I�[�o�[�����Ɏ��s����Ă����珈�����Ȃ�
        var gameState = GameStateManager.Instance;

        if (gameState.AreBothPlayersWinning())
        {
            ExecuteDraw();
            return;
        }
    }

    // ��A�N�e�B�u�ɂȂ�Ƃ���loadGameOver��false�Ƀ��Z�b�g
    private void OnDisable()
    {
        loadGameOver = false;
    }
}
