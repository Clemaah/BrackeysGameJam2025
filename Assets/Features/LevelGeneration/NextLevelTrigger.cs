using UnityEngine;

public class NextLevelTrigger : MonoBehaviour{
    private void OnTriggerEnter (Collider other)
    {
        if (!other.CompareTag("Player")) return;

        GameManager.Instance.NextLevel();
    }
}
