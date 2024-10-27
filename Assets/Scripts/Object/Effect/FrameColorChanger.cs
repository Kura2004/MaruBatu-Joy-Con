using UnityEngine;
using DG.Tweening;

public class FrameColorChanger : MonoBehaviour
{
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color changeColor = Color.red;
    [SerializeField] private float colorChangeDuration = 1.0f;
    public bool isAnimating { get; private set; } = false;

    private Renderer objectRenderer;
    private Tween colorTween;

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        objectRenderer.material.color = normalColor;
        isAnimating = false;
    }

    private bool ShouldAbortColorChange()
    {
        return isAnimating &&
            GameStateManager.Instance.IsBoardSetupComplete;
    }

    public void ChangeColor(Color targetColor)
    {
        if (ShouldAbortColorChange())
        {
            colorTween?.Kill();
            return;
        }

        isAnimating = true;

        colorTween?.Kill();
        colorTween = objectRenderer.material.DOColor(targetColor, colorChangeDuration)
            .OnComplete(() => isAnimating = false);
    }

    public void ChangeColorNormal()
    {
        isAnimating = true;

        colorTween?.Kill();
        colorTween = objectRenderer.material.DOColor(normalColor, colorChangeDuration)
            .OnComplete(() => isAnimating = false);
    }

    public void ChangeColorChange()
    {
        if (ShouldAbortColorChange())
        {
            colorTween?.Kill();
            return;
        }

        isAnimating = true;

        colorTween?.Kill();
        colorTween = objectRenderer.material.DOColor(changeColor, colorChangeDuration)
            .OnComplete(() => isAnimating = false);
    }
}
