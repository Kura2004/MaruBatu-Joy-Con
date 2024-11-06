using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;

public class GuideScaler : MonoBehaviour
{
    [SerializeField] private List<Image> guideImages;       // �K�C�h�Ƃ��Đݒ肷��Image
    [SerializeField] private float duration = 1f;           // �X�P�[���̕⊮����
    [SerializeField] private Ease easing = Ease.InOutQuad;  // �C�[�W���O���C���X�y�N�^�[�Őݒ�
    [SerializeField] private float scaleMultiplier = 1.5f;  // �g�厞�̔{�����C���X�y�N�^�[�Ŏw��

    [SerializeField] private Image GuideBack;

    private List<Vector3> initialScales = new List<Vector3>();
    private bool isGuideVisible = true; // �K�C�h���\������Ă��邩�ǂ����̃t���O
    private bool isAnimating = false;

    private bool isGuideChanging = false;
    private int currentGuideIndex = 0;

    private Joycon leftJoycon;
    private Joycon rightJoycon;

    [SerializeField] private Button guideButton;

    private void Start()
    {
        // �K�C�hImage�̏�����
        if (guideImages == null || guideImages.Count == 0)
        {
            Debug.LogError("�K�C�hImage���ݒ肳��Ă��܂���I");
            return;
        }

        foreach (var image in guideImages)
        {
            initialScales.Add(image.transform.localScale);
            image.enabled = false;
        }

        isGuideVisible = false;
        currentGuideIndex = 0;

        // Joy-Con �̏�����
        var joycons = JoyconManager.Instance.j;
        if (joycons == null || joycons.Count == 0)
        {
            Debug.LogWarning("Joy-Con ��������܂���B");
            return;
        }

        if (joycons.Count >= 2)
        {
            leftJoycon = joycons.Find(c => c.isLeft);
            rightJoycon = joycons.Find(c => !c.isLeft);
        }

        guideButton.onClick.AddListener(() => {
            JoyConButtonSelector.onGuide = true;
        });
    }

    // n�b�ŃK�C�h�̃X�P�[�����g�傷�郁�\�b�h
    public void ShowGuide()
    {
        if (currentGuideIndex != 0)
        {
            Debug.LogWarning("�K�C�h�̃C���f�N�X���ُ�" + currentGuideIndex);
            return;
        }

        isAnimating = true;
        guideImages[currentGuideIndex].enabled = true;
        guideImages[currentGuideIndex].DOFade(1f, duration);

        Vector3 targetScale = initialScales[currentGuideIndex] * scaleMultiplier;  // �g��{����K�p
        guideImages[currentGuideIndex].transform.localScale = Vector3.zero;
        guideImages[currentGuideIndex].transform.DOScale(targetScale, duration).SetEase(easing).OnComplete(() =>
        {
            isAnimating = false;
            for (int i = 1; i < guideImages.Count; i++)
            {
                guideImages[i].rectTransform.localScale = Vector3.one;
            }
        });
    }

    // n�b�ŃK�C�h�̃X�P�[���������l�ɖ߂��i��\����ԁj���\�b�h
    public void HideGuide()
    {
        bool indexCheck = currentGuideIndex == guideImages.Count - 1 ||
            currentGuideIndex == 0;

        if (!indexCheck)
        {
            Debug.LogWarning("�K�C�h�̃C���f�N�X���ُ�" + currentGuideIndex);
            return;
        }

        isAnimating = true;
        guideImages[currentGuideIndex].transform.DOScale(Vector3.zero, duration).SetEase(easing).OnComplete(() =>
        {
            isAnimating = false;
            guideImages[currentGuideIndex].enabled = false;
            currentGuideIndex = 0;
            JoyConButtonSelector.onGuide = false;
        });
    }

    // �K�C�h�̕\��/��\�����g�O�����郁�\�b�h
    public void ToggleGuide()
    {
        ScenesAudio.ClickSe();

        if (isGuideVisible)
        {
            GuideBack.color = Color.clear;
            HideGuide();
        }
        else
        {
            GuideBack.DOColor(Color.black, 0.2f);
            ShowGuide();
        }
        isGuideVisible = !isGuideVisible;
    }

    // ���݂̃K�C�hImage���t�F�[�h�A�E�g���Ď��̃K�C�hImage���t�F�[�h�C�����郁�\�b�h
    private void ShowNextGuideImage()
    {
        if (currentGuideIndex + 1 == guideImages.Count)
        {
            guideButton.onClick?.Invoke();
            return;
        }

        ScenesAudio.ClickSe();
        isGuideChanging = true;
        int prevIndex = currentGuideIndex;

        guideImages[currentGuideIndex].DOFade(0f, duration).SetEase(easing).OnComplete(() =>
        {
            guideImages[prevIndex].enabled = false;
        });

        currentGuideIndex = (currentGuideIndex + 1) % guideImages.Count;

        guideImages[currentGuideIndex].enabled = true;
        guideImages[currentGuideIndex].DOFade(1f, duration).SetEase(easing).OnComplete(() =>
        {
            isGuideChanging = false;
        });
    }

    private void ShowPrevGuideImage()
    {
        if (currentGuideIndex - 1 < 0)
        {
            guideButton.onClick?.Invoke();
            return;
        }

        ScenesAudio.ClickSe();

        isGuideChanging = true;
        int prevIndex = currentGuideIndex;

        guideImages[currentGuideIndex].DOFade(0f, duration).SetEase(easing).OnComplete(() =>
        {
            guideImages[prevIndex].enabled = false;
        });

        currentGuideIndex = (currentGuideIndex - 1) % guideImages.Count;

        guideImages[currentGuideIndex].enabled = true;
        guideImages[currentGuideIndex].DOFade(1f, duration).SetEase(easing).OnComplete(() =>
        {
            isGuideChanging = false;
        });
    }

    private void LateUpdate()
    {
        if (!isGuideVisible || isAnimating || isGuideChanging) return;

        if (leftJoycon.GetButtonDown(Joycon.Button.DPAD_DOWN))
        {
            ShowNextGuideImage();
            return;
        }

        if (leftJoycon.GetButtonDown(Joycon.Button.DPAD_LEFT))
        {
            ShowPrevGuideImage();
            return;
        }
    }
}
