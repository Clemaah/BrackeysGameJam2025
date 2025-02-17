using System;
using UnityEngine;

public enum MenuType {
    None, Pause, WishesSelection
}

[Serializable]
public struct MenuPair
{
    public MenuType type;
    public GameObject gameObject;
}

public class UIManager : MonoBehaviour
{
    [SerializeField] private MenuPair[] _menuList;
    
    private MenuType _activeUI = MenuType.None;

    
    public void DisplayUI(MenuType UIToDisplay)
    {
        if (_activeUI != MenuType.None)
            HideUI();
        
        Time.timeScale = 0f;
        _activeUI = UIToDisplay;
        GetActiveUI().SetActive(true);
    }
    
    public void HideUI()
    {
        GetActiveUI().SetActive(false);
        _activeUI = MenuType.None;
        //GameManager.PauseGame(false);
        Time.timeScale = 1f;
    }

    private GameObject GetActiveUI()
    {
        foreach (MenuPair menu in _menuList)
        {
            if (menu.type != _activeUI) continue;
            
            return menu.gameObject;
        }

        return null;
    }
}
