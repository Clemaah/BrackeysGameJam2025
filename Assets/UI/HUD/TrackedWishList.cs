using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TrackedWishList : MonoBehaviour
{
    [SerializeField] private GameObject TrackedWishItem;
    private List<GameObject> _wishList;
    
    private void Start()
    {
        _wishList = new List<GameObject>();
        GameManager.WishesManager.OnWishAdded += AddWishItem;
        foreach (var wish in GameManager.WishesManager.currentWishes)
        {
            AddWishItem(wish);
        }
    }
    
    private void AddWishItem(WishSO wishData)
    {
        GameObject wish = Instantiate(TrackedWishItem, transform);
        wish.GetComponent<TrackedWishItem>().Initialize(wishData);
        _wishList.Add(wish);
    }
}
