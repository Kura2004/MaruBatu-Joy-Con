using UnityEngine;
using DG.Tweening;
using System.Collections;

public class HeadRotationAnimator : MonoBehaviour
{
    private RectTransform rectTransform;

    // �C���X�y�N�^�[�Őݒ�\�ȃp�����[�^
    [Header("Rotation Settings")]
    [SerializeField] private Vector3 rotationAxis = Vector3.up;  // ��]��
    [SerializeField] private float[] rotationAngles;
    [SerializeField] private float duration = 1f;                // �⊮����
    [SerializeField] private Ease easing = Ease.InOutSine;       // �C�[�W���O
    [SerializeField] private float delayBetweenLoops = 0.5f;     // �ҋ@����

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        StartCoroutine(Rotate());
    }

    private IEnumerator Rotate()
    {
        while (true)
        {
            for (int i = 0; i < rotationAngles.Length; i++)
            {
                // �^�[�Q�b�g��]���v�Z
                Vector3 targetRotation = rectTransform.localEulerAngles + rotationAxis * rotationAngles[i];

                // ��]�A�j���[�V���������s
                rectTransform.DOLocalRotate(targetRotation, duration, RotateMode.FastBeyond360)
                             .SetEase(easing);

                // ��]���I���܂őҋ@
                yield return new WaitForSeconds(duration + delayBetweenLoops);
            }
        }
    }
}