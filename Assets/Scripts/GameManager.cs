using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// press f12 to bring up external, then shift+f12 for more
// 3:45 ~ https://www.youtube.com/watch?v=QplEeEAJxck
public class GameManager : MonoBehaviour
{
    // once set it will not change - singleton pattern
    public static GameManager Instance;

    public int MaxNumberOfShots = 3;
    [SerializeField] private float _secondsToWaitBeforeDeathCheck = 3f;
    [SerializeField] private GameObject _restartScreenObject;
    [SerializeField] private SlingShotHandler _slingShotHandler;

    private int _usedNumberOfShots;

    private IconHandler _iconHandler;

    private List<Enemy> _enemyList = new List<Enemy>();

    private void Awake()
    {
        if (Instance == null)
        {
            // set the instance of the GameManager script
            Instance = this;
        }

        _iconHandler = GameObject.FindObjectOfType<IconHandler>();

        Enemy[] enemyArray = FindObjectsOfType<Enemy>();
        for (int i = 0; i< enemyArray.Length; i++)
        {
            _enemyList.Add(enemyArray[i]);
        }
    }

    public void UseShot()
    {
        _usedNumberOfShots++;
        _iconHandler.UseShot(_usedNumberOfShots);

        CheckForLastShot();
    }

    public bool HasEnoughShots()
    {
        if (_usedNumberOfShots < MaxNumberOfShots)
        {
            return true;
        }

        return false;
    }

    public void CheckForLastShot()
    {
        if (_usedNumberOfShots == MaxNumberOfShots)
        {
            StartCoroutine(CheckAfterWaitTime());
        }
    }

    private IEnumerator CheckAfterWaitTime()
    {
        Debug.Log("CheckAfterWaitTime()");
        yield return new WaitForSeconds(_secondsToWaitBeforeDeathCheck);

        if (_enemyList.Count == 0)
        {
            WinGame();
        }
        else
        {
            RestartGame();
        }
    }

    public void RemoveEnemy(Enemy enemy)
    {
        _enemyList.Remove(enemy);
        CheckForAllDeadEnemies();
    }

    private void CheckForAllDeadEnemies()
    {
        if (_enemyList.Count == 0)
        {
            WinGame();
        }
    }

    #region Win / Lose

    private void WinGame()
    {
        Debug.Log("WIN GAME");
        // use .SetActive for GameObject ~ disables the entire game object

        _restartScreenObject.SetActive(true);

        // use .enabled for Script ~ disables just the script
        _slingShotHandler.enabled = false;
    }

    public void RestartGame()
    {
        Debug.Log("LOSE GAME");

        // turn back on the sling shot handler
        _slingShotHandler.enabled = true;
        
        // reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    #endregion
}
