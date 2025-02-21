using TMPro;
using UnityEngine;

public class WishItemUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI wishText;
    [SerializeField] private TextMeshProUGUI  genieCommentaryText;
    private WishSO _wish;
    
    public void UpdateWish(WishSO newWish)
    {
        gameObject.SetActive(true);
        _wish = newWish;
        wishText.text = _wish.wish;
        genieCommentaryText.text = _wish.genieCommentary;
    }

    public void HideWish()
    {
        gameObject.SetActive(false);
        _wish = null;
    }
}
