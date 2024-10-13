using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GuideScaler : MonoBehaviour
{
    [SerializeField] private Image guideImage;       // �K�C�h�Ƃ��Đݒ肷��Image
    [SerializeField] private float duration = 1f;    // �X�P�[���̕⊮����
    [SerializeField] private Ease easing = Ease.InOutQuad;  // �C�[�W���O���C���X�y�N�^�[�Őݒ�
    [SerializeField] private float scaleMultiplier = 1.5f;  // �g�厞�̔{�����C���X�y�N�^�[�Ŏw��

    private Vector3 initialScale; // �����̃X�P�[����ۑ�
    private bool isGuideVisible = true; // �K�C�h���\������Ă��邩�ǂ����̃t���O

    private void Start()
    {
        if (guideImage == null)
        {
            Debug.LogError("�K�C�hImage���ݒ肳��Ă��܂���I");
            return;
        }

        // �����X�P�[����ۑ�
        initialScale = guideImage.transform.localScale;
        guideImage.enabled = false;
        isGuideVisible = false;
    }

    // n�b�ŃK�C�h�̃X�P�[�����g�傷�郁�\�b�h
    public void ShowGuide()
    {
        guideImage.enabled = true;
        if (guideImage != null)
        {
            Vector3 targetScale = initialScale * scaleMultiplier;  // �g��{����K�p
            guideImage.transform.localScale = Vector3.zero;
            guideImage.transform.DOScale(targetScale, duration).SetEase(easing);
        }
    }

    // n�b�ŃK�C�h�̃X�P�[���������l�ɖ߂��i��\����ԁj���\�b�h
    public void HideGuide()
    {
        if (guideImage != null)
        {
            guideImage.transform.DOScale(Vector3.zero, duration).SetEase(easing).OnComplete(() =>
            {
                guideImage.enabled = false;
            });
        }
    }

    // �K�C�h�̕\��/��\�����g�O�����郁�\�b�h
    public void ToggleGuide()
    {
        if (guideImage != null)
        {
            if (isGuideVisible)
            {
                HideGuide();
            }
            else
            {
                ShowGuide();
            }
            isGuideVisible = !isGuideVisible; // �t���O�𔽓]
        }
    }

    // �K�C�h��n�b�Ԋu�Ŏ����I�Ƀg�O���������郁�\�b�h
    public void StartAutoToggleGuide()
    {
        if (guideImage != null)
        {
            DOTween.Sequence()
                .Append(guideImage.transform.DOScale(initialScale * scaleMultiplier, duration).SetEase(easing)) // �g��
                .Append(guideImage.transform.DOScale(Vector3.zero, duration).SetEase(easing)) // �k��
                .SetLoops(-1, LoopType.Yoyo); // �������[�v�Ō��݂ɐ؂�ւ�
        }
    }
}
