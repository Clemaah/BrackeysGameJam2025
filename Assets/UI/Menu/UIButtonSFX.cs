using UnityEngine;
using UnityEngine.UI;


public class UIButtonSFX : MonoBehaviour
{
    
    private Button _button;
    
    public AudioClip hoverSound;
    public AudioClip clickSound;
        
    void Start()
    {
        _button = GetComponent<Button>();
        if (!_button) return;
        _button.onClick.AddListener(() => AudioSource.PlayClipAtPoint(clickSound, Camera.main.transform.position));
    }

    public void OnHover()
    {
        AudioSource.PlayClipAtPoint(hoverSound, Camera.main.transform.position);
    }

}
