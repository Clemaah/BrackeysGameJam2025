using System;
using TMPro;
using UnityEngine;

public class TrackedWishItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI wishText;
    private WishSO _wish;

    public void Initialize(WishSO wish)
    {
        _wish = wish;
        wishText.text = wish.name;
    }
}
