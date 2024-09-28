using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening; // DOTween���g�p���邽�߂̖��O���

public class JoyConButtonSelector : MonoBehaviour
{
    private List<Joycon> joycons; // JoyCon�̃��X�g
    private Joycon joyconL; // ��JoyCon�̃C���X�^���X
    private Joycon joyconR;

    [SerializeField] private ButtonManager buttonManager; // ButtonManager�̎Q��
    [SerializeField] private float inputCooldown = 0.5f; // �{�^���؂�ւ���̃N�[���_�E������
    [SerializeField] private float colorTransitionDuration = 0.3f; // �{�^���̐F���ς��܂ł̎���
    [SerializeField] private float scaleMultiplier = 1.2f; // �I�����̃{�^���̃X�P�[���{��
    [SerializeField] private Color selectedColor = Color.green; // �I������Ă���Ƃ��̃{�^���̐F
    [SerializeField] private Color defaultColor = Color.white; // �f�t�H���g�̃{�^���̐F
    [SerializeField] private List<ButtonTagPair> buttonTagPairs; // �C���f�b�N�X�ƃ^�O�̃y�A���X�g

    private int currentIndex = 0; // ���ݑI������Ă���{�^���̃C���f�b�N�X
    private bool canInput = true; // ���͉\���ǂ������Ǘ�
    private Dictionary<int, Vector3> originalScales = new Dictionary<int, Vector3>(); // �{�^���̌��̃��[�J���X�P�[����ۑ�
    private List<Button> buttonList = new List<Button>(); // �{�^�����i�[���郊�X�g

    [SerializeField] VolumeSettings volume;
    private void Start()
    {
        // JoyCon�̃��X�g���擾
        joycons = JoyconManager.Instance.j;

        // JoyCon��������Ȃ������ꍇ�̃G���[���b�Z�[�W
        if (joycons == null || joycons.Count == 0)
        {
            Debug.LogError("JoyCon��������܂���BJoyConManager��ݒ肵�Ă��������B");
            return;
        }

        // ���ƉE��JoyCon���擾
        joyconL = joycons.Find(c => c.isLeft);
        joyconR = joycons.Find(c => !c.isLeft);

        // �e�{�^���̌��̃��[�J���X�P�[����ۑ�
        if (buttonManager != null && buttonManager.buttons.Count > 0)
        {
            // buttonList�����������āAbuttonManager�̃{�^�����R�s�[
            buttonList = new List<Button>(new Button[buttonManager.buttons.Count]);

            // �{�^���ƃ^�O�̑Ή���ݒ�
            foreach (var pair in buttonTagPairs)
            {
                for (int i = 0; i < buttonManager.buttons.Count; i++)
                {
                    if (buttonManager.buttons[i].tag == pair.tag)
                    {
                        buttonList[pair.index] = buttonManager.buttons[i].button;
                    }
                }
            }

            // �e�{�^���̌��̃��[�J���X�P�[����ۑ�
            for (int i = 0; i < buttonList.Count; i++)
            {
                if (buttonList[i] != null)
                {
                    originalScales[i] = buttonList[i].transform.localScale;
                }
            }

            // �����̃{�^����I����Ԃɂ���
            UpdateButtonSelection();
        }
    }

    private void Update()
    {
        if (!canInput) return;

        // JoyCon�̃X�e�B�b�N���͂��擾
        Vector2 stickInput = new Vector2(-joyconL.GetStick()[1], joyconL.GetStick()[0]);

        if (buttonManager.isLocked)
        {
            if (joyconL.GetButtonDown(Joycon.Button.DPAD_LEFT))
            {
                if (currentIndex >= 0 && currentIndex < buttonList.Count && buttonList[currentIndex] != null)
                {
                    buttonList[currentIndex].onClick.Invoke();
                }
                StartCooldown(); // �N�[���_�E���J�n
            }


            // �X�e�B�b�N�̏㉺����Ń{�^����؂�ւ���
            if (stickInput.y > 0.7f && volume.onSeVolume)
            {
                volume.EnableBgmVolumeControl();
                ScenesAudio.ClickSe();
                StartCooldown(); // �N�[���_�E���J�n
            }
            else if (stickInput.y < -0.7f && !volume.onSeVolume)
            {
                volume.EnableSeVolumeControl();
                ScenesAudio.ClickSe();
                StartCooldown(); // �N�[���_�E���J�n
            }

            if (volume != null)
            {
                if (!volume.onSeVolume)
                    volume.AddBgmVolume(stickInput.x * 0.01f);

                else
                    volume.AddSeVolume(stickInput.x * 0.01f);
            }
        }

        if (joyconL == null || buttonManager.isLocked) 
            return;

        // �X�e�B�b�N�̏㉺����Ń{�^����؂�ւ���
        if (stickInput.y > 0.5f)
        {
            SelectPreviousButton();
            StartCooldown(); // �N�[���_�E���J�n
        }
        else if (stickInput.y < -0.5f)
        {
            SelectNextButton();
            StartCooldown(); // �N�[���_�E���J�n
        }

        // �w��{�^���iDPAD_DOWN�j�������ꂽ�Ƃ��Ɍ��݂̃{�^�������s����
        if (joyconL.GetButtonDown(Joycon.Button.DPAD_DOWN)) // �����ŕʂ̃{�^�����g�p�\
        {
            if (currentIndex >= 0 && currentIndex < buttonList.Count && buttonList[currentIndex] != null)
            {
                buttonList[currentIndex].onClick.Invoke();
            }
            StartCooldown(); // �N�[���_�E���J�n
        }

        // �C���X�y�N�^�[�ŉEJoyCon�̏����m�F�ł���悤�ɒl���X�V
        if (joyconR != null)
        {
            Debug.Log($"Right JoyCon Stick: {joyconR.GetStick()}");
        }
    }

    // �O�̃{�^����I��
    private void SelectPreviousButton()
    {
        currentIndex = (currentIndex - 1 + buttonList.Count) % buttonList.Count; // �C���f�b�N�X�͈̔͂��z��
        UpdateButtonSelection();
    }

    // ���̃{�^����I��
    private void SelectNextButton()
    {
        currentIndex = (currentIndex + 1) % buttonList.Count; // �C���f�b�N�X�͈̔͂��z��
        UpdateButtonSelection();
    }

    // ���ݑI������Ă���{�^���̕\�����X�V
    private void UpdateButtonSelection()
    {
        if (buttonList == null || buttonList.Count == 0) return; // Null �`�F�b�N

        for (int i = 0; i < buttonList.Count; i++)
        {
            Button button = buttonList[i];
            if (button == null) continue; // Null �`�F�b�N

            Transform buttonTransform = button.transform;

            // ���݂̃{�^����I����Ԃɂ���
            if (i == currentIndex)
            {
                button.GetComponent<Image>().DOColor(selectedColor, colorTransitionDuration);
                Vector3 targetScale = originalScales[i] * scaleMultiplier;
                buttonTransform.DOScale(targetScale, colorTransitionDuration);
            }
            else
            {
                button.GetComponent<Image>().DOColor(defaultColor, colorTransitionDuration);
                buttonTransform.DOScale(originalScales[i], colorTransitionDuration);
            }
        }
    }

    // ���͂���莞�Ԗ����ɂ���
    private void StartCooldown()
    {
        canInput = false; // ���͂𖳌��ɂ���
        DOVirtual.DelayedCall(inputCooldown, () =>
        {
            canInput = true; // �N�[���_�E�����I����������͂��ēx�L���ɂ���
        });
    }
}

// �C���f�b�N�X�ƃ^�O�̃y�A���`����N���X
[System.Serializable]
public class ButtonTagPair
{
    public int index;
    public string tag;
}
