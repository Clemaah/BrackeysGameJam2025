using System;
using UnityEngine;
using UnityEngine.UI;

public class WishesSelectionUI : MonoBehaviour
{
    [SerializeField] private WishItemUI[] wishesItems;

    private void OnEnable()
    {
        WishSO[] wishes = GameManager.WishesManager.GetRandomWishes(wishesItems.Length);

        for (int i = 0; i < wishesItems.Length; i++)
        {
            if (i >= wishes.Length)
            {
                wishesItems[i].HideWish();
                continue;
            }
            
            WishSO currentWish = wishes[i];
            wishesItems[i].UpdateWish(currentWish);
        }
    }
}
