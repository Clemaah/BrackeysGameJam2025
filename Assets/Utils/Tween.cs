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
    }

    public static float Ease(float t, EaseType easeType)
    {
        return easeType switch
        {
            EaseType.Linear => t,
            EaseType.EaseInCubic => t * t * t,
            EaseType.EaseOutCubic => 1 - (1 - t) * (1 - t) * (1 - t),
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
            oc = Mathf.Lerp((float)oa, (float)ob, t);
        }
        else if (typeof(T) == typeof(Vector2))
        {
            oc = Vector2.Lerp((Vector2)oa, (Vector2)ob, t);
        }
        else if (typeof(T) == typeof(Vector3))
        {
            oc = Vector3.Lerp((Vector3)oa, (Vector3)ob, t);
        }
        else if (typeof(T) == typeof(Vector4))
        {
            oc = Vector4.Lerp((Vector4)oa, (Vector4)ob, t);
        }
        else if (typeof(T) == typeof(Quaternion))
        {
            oc = Quaternion.Lerp((Quaternion)oa, (Quaternion)ob, t);
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
