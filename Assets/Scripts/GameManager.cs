using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private int currentPlayerHP = 5;
    private int currentEnemyHP = 3;

    [SerializeField]
    private float timeForDamageDelay = 5;
    [SerializeField]
    private float playerAttackSpeed;
    [SerializeField]
    private float playerJumpForce;
    [SerializeField]
    private float playerMoveSpeed;
    [SerializeField]
    private float playerSpeedSlide;

    [SerializeField]
    private float enemyAttackSpeed;

    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameObject enemy;
    [SerializeField]
    private GameObject restartButton;

    [SerializeField]
    private Transform pointPlayer;
    [SerializeField]
    private Transform pointEnemy;
    [SerializeField]
    private Transform playerHealthContainer;
    [SerializeField]
    private Transform enemyHealthContainer;


    [SerializeField]
    private HealthCell playerHealthCell;
    [SerializeField]
    private HealthCell enemyHealthCell;

    private Stack<HealthCell> playerHealthBar = new Stack<HealthCell>();
    private Stack<HealthCell> enemyHealthBar = new Stack<HealthCell>();

    public void OnEnable()
    {
        Weapon.EnemyDamaged += EnemyDamaged;
        Weapon.PlayerDamaged += PlayerDamaged;
        HealthCell.BlindOutOfTime += PlayerLostHP;
    }

    public void OnDisable()
    {
        Weapon.EnemyDamaged -= EnemyDamaged;
        Weapon.PlayerDamaged -= PlayerDamaged;
        HealthCell.BlindOutOfTime -= PlayerLostHP;
    }

    private void Start()
    {
        Time.timeScale = 1;
        var enemyGO = Instantiate(enemy, pointEnemy);
        var playerGO = Instantiate(player, pointPlayer);
        Enemy.player = playerGO;

        playerGO.GetComponent<Player>().Initialize(playerAttackSpeed, playerJumpForce,
            playerMoveSpeed, playerSpeedSlide);
        enemyGO.GetComponent<Enemy>().Initialize(enemyAttackSpeed);

        InitializeHealthBar();
    }

    private void InitializeHealthBar()
    {
        for (int i = 0; i < currentPlayerHP; i++)
        {
            HealthCell cell = Instantiate(playerHealthCell, playerHealthContainer);
            playerHealthBar.Push(cell);
        }
        Debug.Log(playerHealthBar.Count);
        for (int i = 0; i < currentEnemyHP; i++)
        {
            HealthCell cell = Instantiate(enemyHealthCell, enemyHealthContainer);
            enemyHealthBar.Push(cell);
        }
    }

    private void PlayerDamaged()
    {
        if (playerHealthBar.Peek().blinking == null) playerHealthBar.Peek().Blink(timeForDamageDelay);
        else
        {
            PlayerLostHP();
            PlayerLostHP();
        }
    }

    private void PlayerLostHP()
    {
        if (playerHealthBar.Count > 0) playerHealthBar.Pop().DestroyMe();
        else EndGame();
    }

    private void EnemyDamaged()
    {
        Debug.Log(enemyHealthBar.Peek());
        enemyHealthBar.Pop().DestroyMe();
        if (enemyHealthBar.Count == 0) EndGame();
        playerHealthBar.Peek().StopBlink();
    }

    private void EndGame()
    {
        Time.timeScale = 0;
        restartButton.SetActive(true);
    }

    public void OnClickButtonRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
