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
    
    [Header("Special Wishes")]
    public WishSO randomWish;
    public WishSO threeMoreWishes;
    public WishSO backInTime;
    public WishSO boringGame;

    public void Start()
    {
        Reset();
        randomWish.OnWishApplied += () => SelectRandomWishes(1);
        threeMoreWishes.OnWishApplied += () => SelectRandomWishes(3);
        backInTime.OnWishApplied += () => GameManager.Instance.GoToLevel(GameManager.Instance.CurrentLevel);
        boringGame.OnWishApplied += () => GameManager.Instance.GoToLevel(4);
    }

    public void SelectRandomWishes(int numberOfWishes)
    {
        WishSO[] randomWishes = GetRandomWishes(numberOfWishes);
        

        foreach (WishSO wish in randomWishes) 
        {
            currentWishes.Add(wish);
            _remainingWishes.Remove(wish);
        }

        foreach (WishSO wish in randomWishes)
        {
            wish.Apply();
            OnWishAdded?.Invoke(wish);
        }
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
        currentWishes.Add(wish);
        _remainingWishes.Remove(wish);
        OnWishAdded?.Invoke(wish);
        wish.Apply();
    }
    
    public void RemoveWish(WishSO wish)
    {
        OnWishRemoved?.Invoke(wish);
        // Disapply wish.Apply();
        _remainingWishes.Add(wish);
        currentWishes.Remove(wish);
    }

    public void Reset()
    {
        _remainingWishes = new List<WishSO>(_allWishes);
        currentWishes = new List<WishSO>();
        ResetEvent();
    }
    public void ResetEvent()
    {
        OnWishAdded = null;
        OnWishRemoved = null;
    }
}