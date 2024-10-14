using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening; // DOTween���g�p���邽�߂̖��O���

public class DebugButtonSelector : MonoBehaviour
{
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
                        break;
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
        // Enter�L�[�������ꂽ�Ƃ��Ɍ��݂̃{�^�������s����
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (currentIndex >= 0 && currentIndex < buttonList.Count && buttonList[currentIndex] != null)
            {
                buttonList[currentIndex].onClick.Invoke();
            }
            StartCooldown(); // �N�[���_�E���J�n
        }

        if (!canInput || buttonManager == null || buttonManager.onGuide) return;

        // Horizontal��Vertical�̓��͂��擾
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // �{�^����؂�ւ��鏈��
        if (verticalInput > 0.5f)
        {
            SelectPreviousButton();
            StartCooldown(); // �N�[���_�E���J�n
        }
        else if (verticalInput < -0.5f)
        {
            SelectNextButton();
            StartCooldown(); // �N�[���_�E���J�n
        }

        // Enter�L�[�������ꂽ�Ƃ��Ɍ��݂̃{�^�������s����
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (currentIndex >= 0 && currentIndex < buttonList.Count && buttonList[currentIndex] != null)
            {
                buttonList[currentIndex].onClick.Invoke();
            }
            StartCooldown(); // �N�[���_�E���J�n
        }

        // �{�����[���R���g���[������
        if (volume != null)
        {
            if (!volume.onSeVolume)
                volume.AddBgmVolume(horizontalInput * 0.01f);
            else
                volume.AddSeVolume(horizontalInput * 0.01f);
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