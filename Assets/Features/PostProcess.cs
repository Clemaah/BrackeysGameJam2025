using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcess : MonoBehaviour
{
    public BoolSO visibility;
    public BoolSO saturation;

    private Vignette _vignette;
    private ColorAdjustments _colorAdjustments;
    
    void Start()
    {
        Volume volume = GetComponent<Volume>();
        volume.profile.TryGet(out _vignette);
        _vignette.intensity.overrideState = true;
        _vignette.smoothness.overrideState = true;
        volume.profile.TryGet(out _colorAdjustments);

        SetVisibility(visibility);
        SetSaturation(saturation);

        visibility.OnValueChanged += SetVisibility;
        saturation.OnValueChanged += SetSaturation;
    }

    private void SetVisibility(bool value)
    {
        _vignette.intensity.value = value ? 0.2f : 0.8f;
        _vignette.smoothness.value = value ? 0.4f : 0.6f;
    }
    
    private void SetSaturation(bool value)
    {
        _colorAdjustments.saturation.value = value ? 0.0f : -1.0f;
    }
}
