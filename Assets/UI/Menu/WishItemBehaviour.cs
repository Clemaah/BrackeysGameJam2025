using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WishItemUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI wishText;
    [SerializeField] private TextMeshProUGUI  genieCommentaryText;
    public WishSO wish;
    
    public void UpdateWish(WishSO newWish)
    {
        gameObject.SetActive(true);
        wish = newWish;
        wishText.text = wish.wish;
        genieCommentaryText.text = wish.genieCommentary;
        GetComponent<Button>().onClick.RemoveAllListeners();
        GetComponent<Button>().onClick.AddListener(() => GameManager.WishesManager.SelectWish(wish));
        GetComponent<Button>().onClick.AddListener(() => GameManager.UIManager.OpenMenu(MenuType.None));
    }

    public void HideWish()
    {
        gameObject.SetActive(false);
        wish = null;
    }
}
