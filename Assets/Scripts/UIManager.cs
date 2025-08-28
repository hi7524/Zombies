using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text ammoText;
    public Text scoreTxt;
    public Text waveTxt;

    public GameObject gameOverUi;


    public void OnEnable()
    {
        SetScoreTxt(0);
        SetEnemyWaveTxt(0, 0);
        SetActiveGameOverUI(false);
    }

    public void SetAmmoTxt(int ammo, int remainAmmo)
    {
        ammoText.text = $"{ammo} / {remainAmmo}";
    }

    public void SetScoreTxt(int score)
    {
        scoreTxt.text = $"SCORE: {score:N0}";
    }

    public void SetEnemyWaveTxt(int wave, int leftEnemy)
    {
        waveTxt.text = $"Wave: {wave}\n" +
                       $"Enemy Left: {leftEnemy}";
    }

    public void SetActiveGameOverUI(bool active)
    {
        gameOverUi.SetActive(active);
    }

    public void OnClickRestart()
    {
        // 게임 재시작
    }
}