using Unity.VisualScripting;
using UnityEngine;

public class BossRoomBehaviour : MonoBehaviour
{
    public GameObject boss;


    void Update()
    {
        if (!boss.IsDestroyed()) return;

        GameManager.UIManager.OpenMenu(MenuType.VictoryMenu);
    }
}
