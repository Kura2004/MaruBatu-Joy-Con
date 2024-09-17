using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class EffectMapping
{
    public CameraPostProcessingEffect.EffectType effectType;
    public Material material;
}

[ExecuteInEditMode]
public class CameraPostProcessingEffect : MonoBehaviour
{
    public enum EffectType
    {
        None,
        Grayscale,
        Sepia,
        InvertColors,
        Pencil,
        Noise,
        Test,
        Pixel,
        // ここに他のエフェクトタイプを追加できます
    }

    [SerializeField] protected List<EffectMapping> effectMappings = new List<EffectMapping>();
    [SerializeField] protected EffectType currentEffect = EffectType.None;

    [SerializeField] public bool applyEffect = true; // 追加: エフェクトを適用するかどうかを制御する変数

    protected Dictionary<EffectType, Material> effectMaterials = new Dictionary<EffectType, Material>();

    protected virtual void Start()
    {
        InitializeMaterials();
    }

    protected void InitializeMaterials()
    {
        effectMaterials.Clear();
        foreach (EffectMapping mapping in effectMappings)
        {
            if (mapping.material != null)
            {
                effectMaterials[mapping.effectType] = mapping.material;
            }
        }
    }

    protected virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (applyEffect && effectMaterials.ContainsKey(currentEffect) && effectMaterials[currentEffect] != null)
        {
            Graphics.Blit(source, destination, effectMaterials[currentEffect]);
        }
        else
        {
            Graphics.Blit(source, destination);
        }
    }

    public void SetEffect(EffectType effect, bool apply = true)
    {
        currentEffect = effect;
        applyEffect = apply;
    }

    public void ToggleEffect(bool apply)
    {
        applyEffect = apply;
    }
}

