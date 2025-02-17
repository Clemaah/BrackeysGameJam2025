using TMPro;
using UnityEngine;

public class WishItemUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _wishText;
    [SerializeField] private TextMeshProUGUI  _genieCommentaryText;
    private WishSO _wish;
    
    public void UpdateWish(WishSO newWish)
    {
        _wish = newWish;
        _wishText.text = _wish.name;
        _genieCommentaryText.text = _wish.name;
    }

    public void HideWish()
    {
        _wish = null;
        gameObject.SetActive(false);
    }
}
