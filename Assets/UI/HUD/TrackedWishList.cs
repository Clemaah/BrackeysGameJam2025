using System;
using TMPro;
using UnityEngine;

public class TrackedWishList : MonoBehaviour
{
    [SerializeField] private GameObject TrackedWishItem;
    
    private void Start()
    {
        GameManager.WishesManager.OnWishAdded += AddWishItem;
        foreach (var wish in GameManager.WishesManager.currentWishes)
        {
            AddWishItem(wish);
        }
    }
    
    private void AddWishItem(WishSO wishData)
    {
        Instantiate(TrackedWishItem, transform);
        //TrackedWishItem.GetComponent<TrackedWishItem>().Initialize(wishData);
    }
}
