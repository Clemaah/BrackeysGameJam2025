using System;
using System.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public static class Tween
{

    public enum EaseType
    {
        Linear,
        EaseInCubic,
        EaseOutCubic,
        EaseOutBack
    }

    // https://easings.net/
    public static float Ease(float t, EaseType easeType)
    {
        return easeType switch
        {
            EaseType.Linear => t,
            EaseType.EaseInCubic => t * t * t,
            EaseType.EaseOutCubic => 1 - (1 - t) * (1 - t) * (1 - t),
            EaseType.EaseOutBack => 1.0f + (1.70158f + 1.0f) * Mathf.Pow(t - 1.0f, 3.0f) + 1.70158f * Mathf.Pow(t - 1.0f, 2.0f),
            _ => t
        };
    }
    
    // https://stackoverflow.com/questions/71380486/generic-lerp-for-unity
    static T Lerp<T>(T a, T b, float t) where T : struct
    {
        object oa = a;
        object ob = b;

        object oc = null;

        // Dispatch to the correct implementation for T.
        if (typeof(T) == typeof(float))
        {
            oc = Mathf.LerpUnclamped((float)oa, (float)ob, t);
        }
        else if (typeof(T) == typeof(Vector2))
        {
            oc = Vector2.LerpUnclamped((Vector2)oa, (Vector2)ob, t);
        }
        else if (typeof(T) == typeof(Vector3))
        {
            oc = Vector3.LerpUnclamped((Vector3)oa, (Vector3)ob, t);
        }
        else if (typeof(T) == typeof(Vector4))
        {
            oc = Vector4.LerpUnclamped((Vector4)oa, (Vector4)ob, t);
        }
        else if (typeof(T) == typeof(Quaternion))
        {
            oc = Quaternion.LerpUnclamped((Quaternion)oa, (Quaternion)ob, t);
        }

        // Unbox the result.
        return (T)oc;
    }
    
    public static IEnumerator To<T>(float duration, T from, T to, Action<T> onUpdate, Action onComplete = null, EaseType easeType = EaseType.Linear)  where T : struct
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            t = Ease(t, easeType);
            T value = Lerp(from, to, t);
            onUpdate?.Invoke(value);
            yield return null;
        }

        onComplete?.Invoke();
    }
}
