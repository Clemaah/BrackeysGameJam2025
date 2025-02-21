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
        GameManager.Instance.ResetStats();
        OpenMenu("None");
        SceneManager.LoadScene("DevMaps");
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
}
