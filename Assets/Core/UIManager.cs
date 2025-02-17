using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum MenuType {
    None, Pause, WishesSelection
}

[Serializable]
public struct MenuPair
{
    public MenuPair(MenuType newType, GameObject newGameObject)
    {
        type = newType;
        gameObject = newGameObject;
    }
    
    public MenuType type;
    public GameObject gameObject;
}

public class UIManager : MonoBehaviour
{
    [SerializeField] private MenuPair[] prefabMenuList;
    
    public List<MenuPair> _menuList;
    private MenuType _activeUI = MenuType.None;

    private void Start()
    {
        foreach (var menu in prefabMenuList)
        {
            _menuList.Add(new MenuPair(menu.type, Instantiate(menu.gameObject, transform)));
        }
    }
    
    public void DisplayUI(MenuType menuToDisplay)
    {
        if (_activeUI != MenuType.None)
            HideUI();
        
        GameManager.Instance.PauseGame(true);
        _activeUI = menuToDisplay;
        GetActiveUI().SetActive(true);
    }
    
    public void HideUI()
    {
        GetActiveUI().SetActive(false);
        _activeUI = MenuType.None;
        GameManager.Instance.PauseGame(false);
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
