using UnityEngine;

public class RotateAroundYAxis : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 30f; // ��]���x�i�x/�b�j
    private Vector3 lastPosition; // �O�t���[���̈ʒu���L�^����ϐ�

    private void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        lastPosition = transform.position;
    }
}



