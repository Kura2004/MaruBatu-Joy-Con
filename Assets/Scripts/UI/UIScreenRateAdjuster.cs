using UnityEngine;
using UnityEngine.UI;

public class UIScreenRateAdjuster : MonoBehaviour
{
    private const int FullWidth = 1920;
    private const int FullHeight = 1080;
    private const int HalfWidth = 960;
    private const int HalfHeight = 540;

    [Header("Adjust UI on Enable")]
    [Tooltip("Enable this to call AdjustUIForResolution when the object is enabled.")]
    public bool AdjustUIOnEnable = true;

    private CanvasScaler canvasScaler;
    private Vector2 lastScreenResolution;

    private void Awake()
    {
        canvasScaler = GetComponent<CanvasScaler>();
        if (canvasScaler == null)
        {
            Debug.LogWarning("CanvasScaler component not found on this GameObject.");
        }
    }

    private void OnEnable()
    {
        if (AdjustUIOnEnable)
        {
            LockResolution();
            AdjustUIForResolution();
        }
    }

    private void LockResolution()
    {
        // 現在の解像度を取得
        Vector2 currentResolution = new Vector2(Screen.width, Screen.height);

        // 解像度を1920x1080またはその半分にロック
        if (currentResolution.x >= FullWidth && currentResolution.y >= FullHeight)
        {
            Screen.SetResolution(FullWidth, FullHeight, Screen.fullScreenMode);
        }
        else
        {
            Screen.SetResolution(HalfWidth, HalfHeight, Screen.fullScreenMode);
        }
    }

    public void AdjustUIForResolution()
    {
        if (canvasScaler != null)
        {
            float scaleFactor = (float)Screen.width / FullWidth;
            canvasScaler.scaleFactor = Mathf.Clamp(scaleFactor, 0.1f, 10f);
        }
    }
}


