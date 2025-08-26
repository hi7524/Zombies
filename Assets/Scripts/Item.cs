using UnityEngine;

public class Item : MonoBehaviour, IItem
{
    public enum Types
    {
        Coin,
        Ammo,
        Health
    }

    public Types itemType;
    public int value = 20;


    public void Use(GameObject go)
    {
        switch (itemType)
        {
            case Types.Coin:
                {
                    Debug.Log("Coin");
                }
                break;

            case Types.Ammo:
                {
                    var shooter = go.GetComponent<PlayerShooter>();

                    if (shooter != null)
                        shooter.gun.AddAmmo(value);
                }
                break;

            case Types.Health:
                {
                    var playerHealth = go.GetComponent<PlayerHealth>();
                    playerHealth?.Heal(value);
                }
                break;

            default:
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagManager.Player)) // **
        {
            Use(other.gameObject);
            Destroy(gameObject);
        }
    }
}
