using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneCoordinator : MonoBehaviour
{

    static int startCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        startCount++;
        if (startCount == 1)
            SceneManager.LoadScene(1, LoadSceneMode.Additive);
    }

    public void StartGame()
    {
        UIManager.Instance.ShowMenuScreen(false);
        SceneManager.UnloadSceneAsync(0);
        SceneManager.LoadScene(2, LoadSceneMode.Additive);
    }
}
