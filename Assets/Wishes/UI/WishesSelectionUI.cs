using System;
using UnityEngine;
using UnityEngine.UI;

public class WishesSelectionUI : MonoBehaviour
{
    [SerializeField] private UIManager _UIManager;
    [SerializeField] private WishesManager _wishesManager;
    [SerializeField] private WishItemUI[] _wishesItems;

    private void OnEnable()
    {
        WishSO[] wishes = _wishesManager.GetRandomWishes(_wishesItems.Length);

        for (int i = 0; i < _wishesItems.Length; i++)
        {
            if (i >= wishes.Length)
            {
                _wishesItems[i].HideWish();
                continue;
            }
            
            WishSO currentWish = wishes[i];
            _wishesItems[i].UpdateWish(currentWish);
            _wishesItems[i].GetComponent<Button>().onClick.RemoveAllListeners();
            _wishesItems[i].GetComponent<Button>().onClick.AddListener(() => _wishesManager.SelectWish(currentWish));
            _wishesItems[i].GetComponent<Button>().onClick.AddListener(() => _UIManager.HideUI());
        }
    }
}
