using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    [SerializeField] private MenuType defaultMenu;
    [SerializeField] private MenuPair[] prefabMenuList;
    
    private List<MenuPair> _menuList;
    public MenuType currentMenu { get; private set; }
    private MenuType _lastMenu = MenuType.None;

    private void Start()
    {
        currentMenu = defaultMenu;
        _menuList = new List<MenuPair>();
        foreach (MenuPair menu in prefabMenuList)
        {
            GameObject instantiatedMenu = Instantiate(menu.gameObject, transform);
            instantiatedMenu.GetComponent<MenuBehaviour>().Initialize(this, menu.type == defaultMenu);
            _menuList.Add(new MenuPair(menu.type, instantiatedMenu));
        }
    }
    
    public void OpenMenu(MenuType menuToDisplay)
    {
        if (currentMenu != MenuType.None)
            GetMenu(currentMenu).SetActive(false);
            
        switch (menuToDisplay)
        {
            case MenuType.None:
                if (currentMenu == MenuType.Settings)
                {
                    GetMenu(_lastMenu).SetActive(true);
                    (_lastMenu, currentMenu) = (currentMenu, _lastMenu);
                    return;
                }
                break;
                
            case MenuType.MainMenu:
                SceneManager.LoadScene("Menu");
                GetMenu(menuToDisplay).SetActive(true);
                break;
                
            default:
                GetMenu(menuToDisplay).SetActive(true);
                break;
        }
        
        _lastMenu = currentMenu;
        currentMenu = menuToDisplay;
        GameManager.Instance.PauseGame(currentMenu != MenuType.None);
    }

    private GameObject GetMenu(MenuType menuType)
    {
        foreach (MenuPair menu in _menuList)
        {
            if (menu.type != menuType) continue;
            return menu.gameObject;
        }

        return null;
    }
    
    public void OpenMenuByString(String menuName)
    {
        MenuType menuToOpen = (MenuType)Enum.Parse(typeof(MenuType), menuName);
        OpenMenu(menuToOpen);
    }
}



// ModifierDrawer
[CustomPropertyDrawer(typeof(MenuPair))]
public class MenuPairDrawer : PropertyDrawer
{
    // Draw the property inside the given rect
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
        EditorGUI.BeginProperty(position, label, property);

        // Draw a foldout header instead of "Element 0", "Element 1"...
        property.isExpanded = EditorGUI.Foldout(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), false, "", false);

        // Draw label
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        // Don't make child fields be indented
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        // Calculate rects
        var menuTypeRect = new Rect(position.x, position.y, position.width / 2 - 20, position.height);
        var menuRefRect = new Rect(position.x + position.width / 2, position.y, position.width / 2, position.height);

        // Draw fields - pass GUIContent.none to each so they are drawn without labels
        EditorGUI.PropertyField(menuTypeRect, property.FindPropertyRelative("type"), GUIContent.none);
        EditorGUI.PropertyField(menuRefRect, property.FindPropertyRelative("gameObject"), GUIContent.none);

        // Set indent back to what it was
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }
}