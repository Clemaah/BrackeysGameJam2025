using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WishesManager : MonoBehaviour
{
    public event Action<WishSO> OnWishAdded;
    public event Action<WishSO> OnWishRemoved;
        
    [SerializeField] private List<WishSO> _allWishes;
    
    private List<WishSO> _remainingWishes;
    public List<WishSO> currentWishes;

    public void Start()
    {
        _remainingWishes = new List<WishSO>(_allWishes);
        currentWishes = new List<WishSO>();
    }
    
    public WishSO[] GetRandomWishes(int numberOfWishes)
    {
        numberOfWishes = Mathf.Min(numberOfWishes, _remainingWishes.Count);
        List<WishSO> unselectedWishes = new List<WishSO>(_remainingWishes);
        WishSO[] selectedWishes = new WishSO[numberOfWishes];
        
        for (int i = 0; i < numberOfWishes; i++)
        {
            int wishId = Random.Range(0, unselectedWishes.Count);
            selectedWishes[i] = unselectedWishes[wishId];
            unselectedWishes.RemoveAt(wishId);
        }

        return selectedWishes;
    }
    
    public void SelectWish(WishSO wish)
    {
        OnWishAdded?.Invoke(wish);
        wish.Apply();
        currentWishes.Add(wish);
        _remainingWishes.Remove(wish);
    }
    
    public void RemoveWish(WishSO wish)
    {
        OnWishRemoved?.Invoke(wish);
        // Desapply wish.Apply();
        _remainingWishes.Add(wish);
        currentWishes.Remove(wish);
    }

    public void Reset()
    {
        _remainingWishes = new List<WishSO>(_allWishes);
        ResetEvent();
    }

    public void ResetEvent()
    {
        OnWishAdded = null;
        OnWishRemoved = null;
    }
}