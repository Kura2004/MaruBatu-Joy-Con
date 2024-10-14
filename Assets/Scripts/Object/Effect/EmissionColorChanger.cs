using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using System.Collections;

public class EmissionColorChanger : MonoBehaviour
{
    [SerializeField]
    private Color originalColor = Color.white; // ���̐F

    [SerializeField]
    public Color hoverAndClickColor = Color.red; // �G�ꂽ�Ƃ��̐F

    [SerializeField]
    private float colorChangeDuration = 1f; // �F�ύX�̕⊮����

    [SerializeField]
    private string targetTag = "Player"; // �^�O�w��

    private Renderer objectRenderer; // �I�u�W�F�N�g��Renderer
    private Tween colorTween; // �F�⊮��Tween
    private bool isChanging = false;

    protected virtual void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        if (objectRenderer != null)
        {
            objectRenderer.material.color = originalColor; // �����F��ݒ�
        }
        isChanging = false;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag) && objectRenderer != null && !isChanging)
        {
            isChanging = true;
            objectRenderer.material.EnableKeyword("_EMISSION"); // �G�~�b�V������L���ɂ���

            // �F�⊮���t���[�����Ƃɏ�������
            StartCoroutine(ChangeEmissionColor(objectRenderer.material, objectRenderer.material.GetColor("_EmissionColor"), hoverAndClickColor));
        }
    }

    private IEnumerator ChangeEmissionColor(Material mat, Color startColor, Color endColor)
    {
        float time = 0f;
        while (time < colorChangeDuration)
        {
            time += Time.deltaTime;
            Color lerpedColor = Color.Lerp(startColor, endColor, time / colorChangeDuration);
            mat.SetColor("_EmissionColor", lerpedColor);
            yield return null;
        }
        isChanging = false;
    }


    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(targetTag) && objectRenderer != null)
        {
            colorTween = objectRenderer.material.DOColor(originalColor, "_EmissionColor", colorChangeDuration); // ���̐F�ɖ߂�
        }
    }

    public void ChangeHoverColor(Color newColor)
    {
        hoverAndClickColor = newColor; // �F���O������ύX�ł���悤��
    }

    private bool ShouldChangeColorOnTrigger()
    {
        return !GameStateManager.Instance.IsRotating && !isChanging; // �J���[�ύX���s������
    }
}
