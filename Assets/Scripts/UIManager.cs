using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text scoreTxt;
    public Text enemyWaveTxt;
    public Text ammoText;

    public GameObject gameOverUi;


    public void Awake()
    {
        gameOverUi.SetActive(false);
    }

    public void SetScoreTxt(int score)
    {
        scoreTxt.text = $"SCORE: {score}";
    }

    public void SetEnemyWaveTxt(int wave, int leftEnemy)
    {
        enemyWaveTxt.text = $"Wave: {wave}\n" +
                            $"Enemy Left: {leftEnemy}";
    }

    public void SetAmmoTxt(int ammo, int remainAmmo)
    {
        ammoText.text = $"{ammo} / {remainAmmo}";
    }

    public void ActiveGameOverUI()
    {
        gameOverUi.SetActive(true);
    }
}
