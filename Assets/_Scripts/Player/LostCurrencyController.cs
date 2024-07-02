using UnityEngine;

public class LostCurrencyController : MonoBehaviour
{
    public int currency;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            //Debug.Log("Picked up currency");
            PlayerManager.Instance.currentCurrency += currency;
            Destroy(this.gameObject);
        }
    }
}
