using UnityEngine;
using DG.Tweening;

public class ArmRotationAnimator : MonoBehaviour
{
    [SerializeField] private RectTransform targetRectTransform;
    [SerializeField] private float rotationAngle = 90f;  // ��]����p�x
    [SerializeField] private float duration = 1f;        // �⊮���� (�b)
    [SerializeField] private Ease easeType = Ease.Linear; // �C�[�W���O�^�C�v


    // Z���̉�]��ݒ肵���x�N�g���̒�����n�b�ŕ⊮�I�ɕω������郁�\�b�h
    public void RotateZAxis(bool isPositive)
    {
        float targetAngle = isPositive ? rotationAngle : -rotationAngle;

        // ���݂̉�]�p�x���擾���AZ���݂̂�ݒ�
        Vector3 currentRotation = targetRectTransform.localEulerAngles;
        float newZRotation = currentRotation.z + targetAngle;

        // Z���̉�]��⊮�I�ɕω�
        targetRectTransform.DOLocalRotate(new Vector3(0, 0, newZRotation), duration)
            .SetEase(easeType);
    }

    // ��]�𔽓]�����郁�\�b�h
    public void ToggleRotationDirection()
    {
        // ���݂̉�]�����𔻒肵�A���]�����ĉ�]
        bool isCurrentlyPositive = targetRectTransform.localEulerAngles.z % 360 < 180;
        RotateZAxis(!isCurrentlyPositive);
    }
}
