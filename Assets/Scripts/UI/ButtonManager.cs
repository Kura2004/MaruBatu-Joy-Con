using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;

[System.Serializable]
public class ButtonData
{
    // �{�^���{�̂̎Q��
    public Button button;

    // �{�^���̃^�O
    public string tag;
}

public class ButtonManager : MonoBehaviour
{
    // �����̃{�^���ƃ^�O�̃y�A���C���X�y�N�^�[�Őݒ�ł���悤�ɂ��郊�X�g
    public List<ButtonData> buttons = new List<ButtonData>();

    // �{�^�������b�N����Ă��邩�ǂ������Ǘ�����t���O
    public bool isLocked = false;

    public bool onGuide = false;

    [SerializeField] string specialTag;

    void Start()
    {
        // �o�^���ꂽ�e�{�^���Ƀ��X�i�[��ݒ�
        foreach (ButtonData buttonData in buttons)
        {
            if (buttonData.button != null)
            {
                // �{�^�����N���b�N���ꂽ�Ƃ���OnButtonClicked���\�b�h���Ăяo��
                buttonData.button.onClick.AddListener(() => OnButtonClicked(buttonData));
            }
        }

        onGuide = false;
    }

    // �{�^�����N���b�N���ꂽ�Ƃ��̏���
    private void OnButtonClicked(ButtonData buttonData)
    {
        // �{�^�������b�N����Ă���ꍇ�A�N���b�N�����𖳎�
        if (isLocked)
        {
            Debug.Log("Buttons are locked, no action performed.");
            return;
        }

        // ���b�N����Ă��Ȃ��ꍇ�Ƀ{�^�����N���b�N���ꂽ���̏��������s
        Debug.Log($"Button {buttonData.button.name} with tag {buttonData.tag} clicked!");
        // �����Ń{�^�����ƂɈقȂ鏈������������
    }

    // �{�^�������b�N���郁�\�b�h
    public void LockButtons()
    {
        isLocked = true;
        foreach (ButtonData buttonData in buttons)
        {
            buttonData.button.interactable = buttonData.tag == specialTag;
        }
    }

    // �{�^���̃��b�N���������郁�\�b�h
    public void UnlockButtons()
    {
        isLocked = false;
        foreach (ButtonData buttonData in buttons)
        {
            buttonData.button.interactable = true; // �{�^���̃C���^���N�V������L���ɂ���
        }
    }

    // �{�^���̃��b�N��Ԃ��g�O�����郁�\�b�h
    public void ToggleLockButtons()
    {
        if (isLocked)
        {
            UnlockButtons(); // ���b�N����
        }
        else
        {
            LockButtons(); // �{�^�������b�N
        }
    }

    public void ToggleGuide()
    {
        onGuide = !onGuide;
    }
}
