using UnityEngine;
using DG.Tweening;

public class HeadRotationAnimator : MonoBehaviour
{
    [SerializeField] private RectTransform headRectTransform;
    [SerializeField] private float headRotationAngle = 90f;  // ��]����p�x
    [SerializeField] private float headRotationDuration = 1f;  // �⊮���� (�b)
    [SerializeField] private Ease headEaseType = Ease.Linear;  // �C�[�W���O�^�C�v

    // Z���̉�]��ݒ肵���x�N�g���̒�����n�b�ŕ⊮�I�ɕω������郁�\�b�h
    public void RotateHeadZAxis(bool isPositive)
    {
        float targetAngle = isPositive ? headRotationAngle : -headRotationAngle;

        // ���݂̉�]�p�x���擾���AZ���݂̂�ݒ�
        Vector3 currentRotation = headRectTransform.localEulerAngles;
        float newZRotation = currentRotation.z + targetAngle;

        // Z���̉�]��⊮�I�ɕω�
        headRectTransform.DOLocalRotate(new Vector3(0, 0, newZRotation), headRotationDuration)
            .SetEase(headEaseType);
    }

    // ��]�𔽓]�����郁�\�b�h
    public void ToggleHeadRotationDirection()
    {
        // ���݂̉�]�����𔻒肵�A���]�����ĉ�]
        bool isCurrentlyPositive = headRectTransform.localEulerAngles.z % 360 < 180;
        RotateHeadZAxis(!isCurrentlyPositive);
    }
}
