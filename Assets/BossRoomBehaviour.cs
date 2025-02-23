using Unity.VisualScripting;
using UnityEngine;

public class BossRoomBehaviour : MonoBehaviour
{
    public GameObject boss;
    private bool _isMenuOpened = false;

    void Update()
    {
        if (!boss.IsDestroyed() || _isMenuOpened) return;

        _isMenuOpened = true;
        GameManager.UIManager.OpenMenu(MenuType.VictoryMenu);
    }
}
