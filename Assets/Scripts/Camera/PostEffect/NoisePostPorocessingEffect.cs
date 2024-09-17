using UnityEngine;
using DG.Tweening; // DOTweenを使用するための名前空間

public class NoisePostProcessingEffect : CameraPostProcessingEffect
{
    private Material noiseMaterial;
    // シングルトンのインスタンス
    public static NoisePostProcessingEffect Instance => SingletonMonoBehaviour<NoisePostProcessingEffect>.Instance;
    protected override void Start()
    {
        base.Start();

        // ノイズエフェクト用のマテリアルを取得
        if (effectMaterials.ContainsKey(EffectType.Noise))
        {
            noiseMaterial = effectMaterials[EffectType.Noise];
        }
    }

    protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (applyEffect && currentEffect == EffectType.Noise && noiseMaterial != null)
        {
            ApplyNoiseEffect(source, destination);
        }
        else
        {
            base.OnRenderImage(source, destination);
        }
    }

    private void ApplyNoiseEffect(RenderTexture source, RenderTexture destination)
    {
        noiseMaterial.SetFloat("_OffsetX", Random.Range(0f, 1.2f));
        noiseMaterial.SetFloat("_OffsetY", Random.Range(0f, 1.2f));
        Graphics.Blit(source, destination, noiseMaterial);
    }

    // _Intensityの値をDOTweenで補完的に減少させるメソッド
    public void AnimateIntensity(float targetIntensity, float duration)
    {
        if (noiseMaterial != null)
        {
            // DOTweenを使って_Intensityの値を補間する
            DOTween.To(() => noiseMaterial.GetFloat("_Intensity"),
                       x => noiseMaterial.SetFloat("_Intensity", x),
                       targetIntensity,
                       duration);
        }
    }

    private void OnDisable()
    {
        if (noiseMaterial != null)
        {
            noiseMaterial.SetFloat("_Intensity", 0.98f);
        }
    }
}
