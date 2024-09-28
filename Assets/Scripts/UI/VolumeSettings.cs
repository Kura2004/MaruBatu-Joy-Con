using UnityEngine;
using UnityEngine.UI;
using TMPro; // TextMeshPro�̖��O���
using Saito.SoundManager;
using DG.Tweening; // DOTween�̖��O���

public class VolumeSettings : MonoBehaviour
{
    [Header("���ʒ����X���C�_�[")]
    [SerializeField] private Slider bgmVolumeSlider;
    [SerializeField] private Slider seVolumeSlider;

    [Header("���ʃe�L�X�g")]
    [SerializeField] private TextMeshProUGUI bgmVolumeText; // BGM���ʃe�L�X�g
    [SerializeField] private TextMeshProUGUI seVolumeText; // SE���ʃe�L�X�g

    [Header("�e�L�X�g�g��ݒ�")]
    [SerializeField] private float scaleFactor = 1.2f; // �g��{��
    [SerializeField] private float scaleDuration = 0.3f; // �g��ɂ����鎞��

    private Vector3 initialBgmVolumeTextScale; // BGM�e�L�X�g�̏����X�P�[��
    private Vector3 initialSeVolumeTextScale; // SE�e�L�X�g�̏����X�P�[��

    public bool onSeVolume = false;

    private void Start()
    {
        // SoundManager���特�ʂ̏����l���擾���A�X���C�_�[�ɐݒ�
        bgmVolumeSlider.value = SoundManager.Instance.bgmMasterVolume;
        seVolumeSlider.value = SoundManager.Instance.seMasterVolume;

        // �X���C�_�[�̒l�ύX�C�x���g�Ƀ��X�i�[��ǉ�
        bgmVolumeSlider.onValueChanged.AddListener(OnBgmVolumeChanged);
        seVolumeSlider.onValueChanged.AddListener(OnSeVolumeChanged);

        // �e�L�X�g�̏����X�P�[����ۑ�
        initialBgmVolumeTextScale = bgmVolumeText.transform.localScale;
        initialSeVolumeTextScale = seVolumeText.transform.localScale;

        EnableBgmVolumeControl();
    }

    /// <summary>
    /// BGM���ʃX���C�_�[�̒l���ύX���ꂽ���̏���
    /// </summary>
    /// <param name="value">�V�������ʒl</param>
    public void OnBgmVolumeChanged(float value)
    {
        SoundManager.Instance.SetBgmMasterVolume(value);
    }

    /// <summary>
    /// SE���ʃX���C�_�[�̒l���ύX���ꂽ���̏���
    /// </summary>
    /// <param name="value">�V�������ʒl</param>
    public void OnSeVolumeChanged(float value)
    {
        SoundManager.Instance.SetSeMasterVolume(value);
    }

    /// <summary>
    /// BGM���ʂɎw�肵���l�𑫂����\�b�h
    /// </summary>
    /// <param name="amount">�������ʒl</param>
    public void AddBgmVolume(float amount)
    {
        float newVolume = SoundManager.Instance.bgmMasterVolume + amount;
        // ���ʂ͈̔͂𐧌�����i��: 0����1�̊ԁj
        newVolume = Mathf.Clamp(newVolume, 0f, 1f);
        SoundManager.Instance.SetBgmMasterVolume(newVolume);
        bgmVolumeSlider.value = newVolume; // �X���C�_�[���X�V
    }

    /// <summary>
    /// SE���ʂɎw�肵���l�𑫂����\�b�h
    /// </summary>
    /// <param name="amount">�������ʒl</param>
    public void AddSeVolume(float amount)
    {
        float newVolume = SoundManager.Instance.seMasterVolume + amount;
        // ���ʂ͈̔͂𐧌�����i��: 0����1�̊ԁj
        newVolume = Mathf.Clamp(newVolume, 0f, 1f);
        SoundManager.Instance.SetSeMasterVolume(newVolume);
        seVolumeSlider.value = newVolume; // �X���C�_�[���X�V
    }

    /// <summary>
    /// SE�{�����[��������L���ɂ��郁�\�b�h
    /// </summary>
    public void EnableSeVolumeControl()
    {
        onSeVolume = true;
        ScaleText(seVolumeText, scaleFactor, scaleDuration);
        ResetTextScale(bgmVolumeText, scaleDuration);
    }

    /// <summary>
    /// BGM�{�����[��������L���ɂ��郁�\�b�h
    /// </summary>
    public void EnableBgmVolumeControl()
    {
        onSeVolume = false;
        ScaleText(bgmVolumeText, scaleFactor, scaleDuration);
        ResetTextScale(seVolumeText, scaleDuration);
    }

    /// <summary>
    /// �e�L�X�g���g�傷�郁�\�b�h
    /// </summary>
    /// <param name="text">�g�傷��e�L�X�g</param>
    /// <param name="scaleFactor">�g��{��</param>
    /// <param name="duration">�g��ɂ����鎞��</param>
    public void ScaleText(TextMeshProUGUI text, float scaleFactor, float duration)
    {
        text.transform.DOScale(scaleFactor, duration).OnComplete(() =>
        {
            // �������̏����i�K�v�ɉ����Ēǉ��j
        });
    }

    /// <summary>
    /// �e�L�X�g�����̑傫���ɖ߂����\�b�h
    /// </summary>
    /// <param name="text">�߂��e�L�X�g</param>
    /// <param name="duration">�߂��̂ɂ����鎞��</param>
    public void ResetTextScale(TextMeshProUGUI text, float duration)
    {
        // �����X�P�[�����g�p���Ė߂�
        if (text == bgmVolumeText)
        {
            text.transform.DOScale(initialBgmVolumeTextScale, duration);
        }
        else if (text == seVolumeText)
        {
            text.transform.DOScale(initialSeVolumeTextScale, duration);
        }
    }
}
