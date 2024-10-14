using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using System.Collections;

public class EmissionColorChanger : MonoBehaviour
{
    [SerializeField]
    private Color originalColor = Color.white; // 元の色

    [SerializeField]
    public Color hoverAndClickColor = Color.red; // 触れたときの色

    [SerializeField]
    private float colorChangeDuration = 1f; // 色変更の補完時間

    [SerializeField]
    private string targetTag = "Player"; // タグ指定

    private Renderer objectRenderer; // オブジェクトのRenderer
    private Tween colorTween; // 色補完のTween
    private bool isChanging = false;

    protected virtual void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        if (objectRenderer != null)
        {
            objectRenderer.material.color = originalColor; // 初期色を設定
        }
        isChanging = false;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag) && objectRenderer != null && !isChanging)
        {
            isChanging = true;
            objectRenderer.material.EnableKeyword("_EMISSION"); // エミッションを有効にする

            // 色補完をフレームごとに処理する
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
            colorTween = objectRenderer.material.DOColor(originalColor, "_EmissionColor", colorChangeDuration); // 元の色に戻す
        }
    }

    public void ChangeHoverColor(Color newColor)
    {
        hoverAndClickColor = newColor; // 色を外部から変更できるように
    }

    private bool ShouldChangeColorOnTrigger()
    {
        return !GameStateManager.Instance.IsRotating && !isChanging; // カラー変更を行う条件
    }
}
