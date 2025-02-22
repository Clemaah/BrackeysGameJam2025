using UnityEngine;

public class ActivationState : MonoBehaviour
{
    public BoolSO isActivated;
    public bool inverse;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameObject.SetActive(isActivated.value);
        isActivated.OnValueChanged += (newValue) => gameObject.SetActive(newValue ^ inverse);
    }
}
