using System.Collections.Generic;
using UnityEngine;

public class WishesManager : MonoBehaviour
{
    [SerializeField] private List<WishSO> _allWishes;
    
    private List<WishSO> _remainingWishes;
    private List<WishSO> _currentWishes;

    public void Start()
    {
        _remainingWishes = new List<WishSO>(_allWishes);
        _currentWishes = new List<WishSO>();
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
        wish.Apply();
        _currentWishes.Add(wish);
        _remainingWishes.Remove(wish);
    }

    public void Reset()
    {
        _remainingWishes = new List<WishSO>(_allWishes);
    }
}