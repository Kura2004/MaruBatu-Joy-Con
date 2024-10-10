using UnityEngine;
using DG.Tweening;

public class LightningAnimator : MonoBehaviour
{
    [SerializeField]
    private GameObject lightningPrefab;  // ���̃v���n�u

    [SerializeField]
    private float duration = 0.5f;  // ���̎�������

    [SerializeField]
    private float yOffset = 1.0f;  // ����Y���W�I�t�Z�b�g

    [SerializeField]
    private Camera mainCamera;  // �g�p����J����

    [SerializeField]
    private CameraShaker shaker;  // �J�����V�F�C�J�[

    private void OnDisable()
    {
        shaker.StopAnimation();  // �J�����V�F�C�J�[�̃A�j���[�V�������~
    }
    /// <summary>
    /// ���̃A�j���[�V�������J�n���郁�\�b�h
    /// </summary>
    public void StartLightningAnimation()
    {
        // �J�����̗h����J�n
        shaker.enabled = true;
        shaker.StartAnimation();

        if (lightningPrefab != null && mainCamera != null)
        {
            // �J�����̎���Ɋ�Â��ĉ�ʂ̒������v�Z
            Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(screenCenter);
            worldPosition.y += yOffset;  // Y���W�I�t�Z�b�g��K�p

            // �v���n�u����ʂ̒����ɐ���
            GameObject lightningInstance = Instantiate(lightningPrefab, worldPosition, Quaternion.identity);

            // DOTween ���g�p���ė��̎������Ԍ�Ɏ����I�ɍ폜���A�J�����̃A�j���[�V��������~���鏈��
            DOVirtual.DelayedCall(duration, () =>
            {
                Destroy(lightningInstance);  // �����폜
            });
        }
        else
        {
            if (lightningPrefab == null)
            {
                Debug.LogWarning("���̃v���n�u���ݒ肳��Ă��܂���B");
            }
            if (mainCamera == null)
            {
                Debug.LogWarning("�J�������ݒ肳��Ă��܂���B");
            }
        }
    }
}