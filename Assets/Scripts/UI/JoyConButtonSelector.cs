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

        if (joycons == null || joycons.Count == 0)
        {
            Debug.LogError("JoyCon��������܂���BJoyConManager��ݒ肵�Ă��������B");
            return;
        }

        currentIndex = 0;

        joyconL = joycons.Find(c => c.isLeft);
        joyconR = joycons.Find(c => !c.isLeft);

        if (buttonManager != null && buttonManager.buttons.Count > 0)
        {
            buttonList = new List<Button>(new Button[buttonManager.buttons.Count]);

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

            for (int i = 0; i < buttonList.Count; i++)
            {
                if (buttonList[i] != null)
                {
                    originalScales[i] = buttonList[i].transform.localScale;
                }
            }

            UpdateButtonSelection();
        }
    }

    private void SoundSetting(Vector2 stickInput)
    {
        if (joyconL.GetButtonDown(Joycon.Button.DPAD_LEFT))
        {
            if (currentIndex >= 0 && currentIndex < buttonList.Count && buttonList[currentIndex] != null)
            {
                buttonList[currentIndex].onClick.Invoke();
            }
            StartCooldown();
            return;
        }

        if (stickInput.y > 0.7f && volume.onSeVolume)
        {
            volume.EnableBgmVolumeControl();
            ScenesAudio.ClickSe();
            StartCooldown();
            return;
        }
        else if (stickInput.y < -0.7f && !volume.onSeVolume)
        {
            volume.EnableSeVolumeControl();
            ScenesAudio.ClickSe();
            StartCooldown();
            return;
        }

        if (volume != null)
        {
            if (!volume.onSeVolume)
                volume.AddBgmVolume(stickInput.x * 0.01f);

            else
                volume.AddSeVolume(stickInput.x * 0.01f);
        }
    }

    public static bool onGuide = false;

    private void Update()
    {
        if (!canInput || buttonManager == null || joyconL == null || onGuide) return;

        Vector2 stickInput = new Vector2(-joyconL.GetStick()[1], joyconL.GetStick()[0]);

        if (buttonManager.isLocked)
        {
            SoundSetting(stickInput);
            return;
        }

        if (stickInput.y > 0.5f)
        {
            SelectPreviousButton();
            StartCooldown();
        }
        else if (stickInput.y < -0.5f)
        {
            SelectNextButton();
            StartCooldown();
            return;
        }

        if (joyconL.GetButtonDown(Joycon.Button.DPAD_DOWN))
        {
            if (currentIndex >= 0 && currentIndex < buttonList.Count && buttonList[currentIndex] != null)
            {
                buttonList[currentIndex].onClick.Invoke();
            }
            StartCooldown();
            return;
        }
    }

    // �O�̃{�^����I��
    private void SelectPreviousButton()
    {
        currentIndex = (currentIndex - 1 + buttonList.Count) % buttonList.Count;
        UpdateButtonSelection();
    }

    // ���̃{�^����I��
    private void SelectNextButton()
    {
        currentIndex = (currentIndex + 1) % buttonList.Count;
        UpdateButtonSelection();
    }

    private void UpdateButtonSelection()
    {
        for (int i = 0; i < buttonList.Count; i++)
        {
            Button button = buttonList[i];
            Transform buttonTransform = button.transform;

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

    private void StartCooldown()
    {
        canInput = false;
        DOVirtual.DelayedCall(inputCooldown, () =>
        {
            canInput = true;
        });
    }
}

[System.Serializable]
public class ButtonTagPair
{
    public int index;
    public string tag;
}
