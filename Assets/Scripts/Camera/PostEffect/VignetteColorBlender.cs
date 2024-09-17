using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class VignetteColorBlender : MonoBehaviour
{
    [SerializeField]
    private PostProcessVolume postProcessVolume; // PostProcessingのVolumeの参照

    private Vignette vignette;

    [SerializeField]
    private Color colorA = Color.white; // 1つ目の色
    [SerializeField]
    private Color colorB = Color.black; // 2つ目の色
    [SerializeField, Range(0f, 1f)]
    private float blendFactor = 0.5f;  // 補完の割合（0がcolorA、1がcolorB）

    [SerializeField]
    private Color blendedColor; // 補完された色

    [SerializeField]
    private Material savedMaterial; // 保存用のマテリアル

    private void Start()
    {
        // PostProcessing Volume から Vignette エフェクトを取得
        if (postProcessVolume.profile.TryGetSettings(out Vignette vignetteEffect))
        {
            vignette = vignetteEffect;
        }
        else
        {
            Debug.LogWarning("Vignette effect is not set in the PostProcessVolume.");
        }

        // 初期値として中間色を設定
        UpdateVignetteColor();
    }

    private void Update()
    {
        // 実行中に補完割合が変更された場合、中間色を更新する
        UpdateVignetteColor();
    }

    private void OnEnable()
    {
        // 保存したマテリアルから colorA, colorB, blendFactor を初期化
        if (savedMaterial != null)
        {
            // マテリアルのプロパティから色と補完割合を取得
            if (savedMaterial.HasProperty("_ColorA"))
            {
                colorA = savedMaterial.GetColor("_ColorA");
            }
            if (savedMaterial.HasProperty("_ColorB"))
            {
                colorB = savedMaterial.GetColor("_ColorB");
            }
            if (savedMaterial.HasProperty("_BlendFactor"))
            {
                blendFactor = savedMaterial.GetFloat("_BlendFactor");
            }

            // 取得した値に基づいて色を更新
            blendedColor = Color.Lerp(colorA, colorB, blendFactor);

            // VignetteのColorプロパティに適用
            if (vignette != null)
            {
                vignette.color.value = blendedColor;
            }
        }
        else
        {
            Debug.LogWarning("Saved Material is not set.");
        }
    }

    private void OnDisable()
    {
        // colorA, colorB, blendFactor, blendedColor を保存用のマテリアルに適用
        if (savedMaterial != null)
        {
            savedMaterial.SetColor("_ColorA", colorA);
            savedMaterial.SetColor("_ColorB", colorB);
            savedMaterial.SetFloat("_BlendFactor", blendFactor);
            savedMaterial.color = blendedColor; // 保存用の色も更新
        }
        else
        {
            Debug.LogWarning("Saved Material is not set.");
        }
    }

    /// <summary>
    /// 2色の中間色をVignetteに適用し、インスペクターに表示
    /// </summary>
    public void UpdateVignetteColor()
    {
        if (vignette != null)
        {
            // Color.Lerpを使用して2つの色の中間色を取得
            blendedColor = Color.Lerp(colorA, colorB, blendFactor);

            // VignetteのColorプロパティに適用
            vignette.color.value = blendedColor;
        }
    }
}
