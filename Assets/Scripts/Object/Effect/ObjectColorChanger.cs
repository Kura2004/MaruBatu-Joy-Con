using UnityEngine;
using DG.Tweening;

public class ObjectColorChanger : MonoBehaviour
{
    [SerializeField]
    private Color originalColor = Color.white; // 元の色をインスペクターで設定できるように

    [SerializeField]
    public Color hoverAndClickColor = Color.red; // オブジェクトが触れたときとクリック時の色

    [SerializeField]
    private float colorChangeDuration = 1f; // 色の補完にかかる時間

    [SerializeField]
    private string targetTag = "Player"; // 触れる対象のタグ

    private Renderer objectRenderer; // オブジェクトのRenderer
    private Tween colorTween; // 色の補完用のTween
    public bool isClicked  = false; // クリック状態を保持するフラグ

    protected virtual void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        if (objectRenderer != null)
        {
            objectRenderer.material.color = originalColor; // オブジェクトの元の色を設定
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag) && objectRenderer != null)
        {
            // タグを持つオブジェクトが触れたときに色を補完的に変える
            colorTween = objectRenderer.material.DOColor(hoverAndClickColor, colorChangeDuration);
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(targetTag) && objectRenderer != null)
        {
            // タグを持つオブジェクトが離れたときに色を元に戻す
            colorTween = objectRenderer.material.DOColor(originalColor, colorChangeDuration);
        }
    }

    public void HandleClick()
    {
        if (objectRenderer != null)
        {
            isClicked = true; // クリック状態を記録
            objectRenderer.material.color = hoverAndClickColor;
            Debug.Log("マスがクリックされました");
        }
    }

    public void ChangeHoverColor(Color newColor)
    {
        hoverAndClickColor = newColor;
    }

    private bool ShouldChangeColorOnTrigger()
    {
        return !GameStateManager.Instance.IsRotating && !isClicked
            && !objectRenderer.material.DOColor(hoverAndClickColor, colorChangeDuration).IsPlaying();
    }
}
