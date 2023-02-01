using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tactics.Core{
public class GameOverSystem : MonoBehaviour
{
    public static GameOverSystem Instance;

    private void Awake() 
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one GameOverSystem! " + transform + "-" + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void GameOver()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
}