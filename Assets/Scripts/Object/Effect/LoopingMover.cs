using UnityEngine;
using DG.Tweening;

public class LoopingMover : MonoBehaviour
{
    [SerializeField] private Vector3 movementDistance = new Vector3(5f, 0f, 0f); // �ړ�����
    [SerializeField] private float moveDuration = 1f; // �ړ��ɂ����鎞��
    [SerializeField] private Ease easeType = Ease.Linear; // �C�[�W���O�̎��
    private Vector3 originalPosition; // ���̈ʒu

    private void Start()
    {
        originalPosition = transform.localPosition; // ���̈ʒu���L�^
        StartLoopingMovement(); // ���[�v����ړ����J�n
    }

    // ���[�v����ړ����J�n
    private void StartLoopingMovement()
    {
        MoveInPositiveDirection();
    }

    // ���̕����ֈړ�
    private void MoveInPositiveDirection()
    {
        Vector3 targetPosition = originalPosition + movementDistance; // �ړ���̍��W

        transform.DOLocalMove(targetPosition, moveDuration)
            .SetEase(easeType)
            .OnComplete(() => MoveInNegativeDirection()); // �ړ�������A���̕����ֈړ�
    }

    // ���̕����ֈړ�
    private void MoveInNegativeDirection()
    {
        Vector3 targetPosition = originalPosition - movementDistance; // ���̈ړ���̍��W

        transform.DOLocalMove(targetPosition, moveDuration)
            .SetEase(easeType)
            .OnComplete(() => MoveInPositiveDirection()); // �ړ�������A���̕����ֈړ�
    }
}
