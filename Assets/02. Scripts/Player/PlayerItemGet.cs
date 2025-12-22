using UnityEngine;

public class PlayerItemGet : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Coin coin = collision.gameObject.GetComponent<Coin>();
        if (coin != null)
        {
            GameManager.Instance.GetCoin(1);
            coin.ReturnPool();
        }
    }
}
