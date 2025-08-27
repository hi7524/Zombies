using UnityEngine;

public class PlayerData : MonoBehaviour
{
    private int coin;

    public UIManager uiManager;


    public void AddCoin(int amount)
    {
        coin += amount;
        uiManager.SetScoreTxt(coin);
    }
}
