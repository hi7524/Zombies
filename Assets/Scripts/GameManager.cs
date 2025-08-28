using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public UIManager uiManager;
    public ZombieSpawner spawner;

    public bool IsGameOver { get; private set; }

    private int score;


    public void Start()
    {
        var findGo = GameObject.FindGameObjectWithTag(TagManager.Player);
        var playerHealth = findGo.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.OnDeath += EndGame;
        }
    }

    public void AddScore(int add)
    {
        score += add;
        uiManager.SetScoreTxt(score);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void EndGame()
    {
        IsGameOver = true;
        uiManager.SetActiveGameOverUI(true);
        spawner.enabled = false;
    }
}
