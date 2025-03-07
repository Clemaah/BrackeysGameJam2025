using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuBehaviour : MonoBehaviour
{
    private UIManager _UIManagerRef;
    
    public void Initialize(UIManager UIManager, bool enable)
    {
        _UIManagerRef = UIManager;
        gameObject.SetActive(enable);
    }
    
    public void OpenMenu(String menuName)
    {
        MenuType menuToOpen = (MenuType)Enum.Parse(typeof(MenuType), menuName);
        _UIManagerRef.OpenMenu(menuToOpen);
    }

    public void StartGame()
    {
        OpenMenu("None");
        SceneManager.LoadScene("DevMaps");
        GameManager.Instance.Reset();
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
}
