using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;  // DOTween�̖��O���

public class FallingImagesManager : MonoBehaviour
{
    // �I�u�W�F�N�g�̐������J�n����
    private void StartGeneratingImages()
    {
        // ��莞�Ԃ��ƂɃI�u�W�F�N�g�𐶐�����
        InvokeRepeating(nameof(GenerateMultipleImages), 0f, spawnInterval);
    }

    [SerializeField] private Image imagePrefab;   // Image��Prefab
    [SerializeField] private float minX = -100f;  // ���������X���W�̍ŏ��l
    [SerializeField] private float maxX = 100f;   // ���������X���W�̍ő�l
    [SerializeField] private float startY = 200f; // ���������Y���W
    [SerializeField] private float fallDuration = 2f;  // �����A�j���[�V�����̍Đ�����
    [SerializeField] private float rotateSpeed = 360f; // ��]���x�i�x/�b�j
    [SerializeField] private int objectCount = 5;  // ��������I�u�W�F�N�g�̑���
    [SerializeField] private float spawnInterval = 0.5f;  // �����Ԋu

    [SerializeField] private Transform parentTransform; // �e�ɂ���I�u�W�F�N�g��Transform

    private int currentImageIndex = 0;  // ���݂̉摜�C���f�b�N�X

    private void Start()
    {
        // �I�u�W�F�N�g�̐������J�n
        StartGeneratingImages();
    }

    // ��x�ɕ����̉摜�𐶐����A�A�j���[�V������ݒ肷�郁�\�b�h
    private void GenerateMultipleImages()
    {
        // 1��̐����ŕ����̃I�u�W�F�N�g�𐶐�
        for (int i = 0; i < objectCount; i++)
        {
            GenerateAndAnimateImage();
        }
    }

    // �摜�𐶐����A�A�j���[�V������ݒ肷�郁�\�b�h
    private void GenerateAndAnimateImage()
    {
        // Image�̃C���X�^���X�𐶐�
        Image newImage = Instantiate(imagePrefab, parentTransform);

        RectTransform imageRectTransform = newImage.GetComponent<RectTransform>();

        // X���W�𓙊Ԋu�ɐݒ�
        float spacing = (maxX - minX) / objectCount + Random.Range(-50f, 50f);  // ���Ԋu�̌v�Z
        float baseX = minX + (spacing * (currentImageIndex % objectCount));  // ��ƂȂ�X���W
        float randomYOffset = Random.Range(-100f, 100f);  // Y���W�ɑ��������_���Ȓl

        imageRectTransform.anchoredPosition = new Vector2(baseX, startY + randomYOffset); // Y���W�͐ݒ肵���l�Ƀ����_���ȃI�t�Z�b�g������������

        // �t�F�[�h�A�E�g�̂��߂ɁA�����ȐF�ɃA�j���[�V����
        newImage.DOColor(new Color(imagePrefab.color.r, imagePrefab.color.g, imagePrefab.color.b, 0f), fallDuration)
            .SetEase(Ease.Linear);

        // �����Ɖ�]�̃A�j���[�V������DOTween�Ŏ��s
        AnimateFallingAndRotating(imageRectTransform);

        currentImageIndex++; // �C���f�b�N�X�𑝂₷
    }



    // DOTween���g���ė����Ɖ�]�̃A�j���[�V���������s���郁�\�b�h
    private void AnimateFallingAndRotating(RectTransform imageRectTransform)
    {

        // Y���W��ς��A���Ԃ���ɃA�j���[�V����
        imageRectTransform.DOAnchorPosY(imageRectTransform.anchoredPosition.y - 600f, fallDuration)
            .SetEase(Ease.Linear)  // ���`�̃X���[�Y�ȃA�j���[�V����
            .OnComplete(() =>
            {
                Destroy(imageRectTransform.gameObject);  // �A�j���[�V����������ɍ폜
                currentImageIndex--;
            });

        // ��]����]���x�ɉ�����fallDuration�b�ԂŃ��[�v������
        imageRectTransform.DORotate(new Vector3(0, 0, rotateSpeed * fallDuration), fallDuration, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear);  // ���`�̃X���[�Y�ȉ�]
    }

}
