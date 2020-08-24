using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneCoordinator : MonoBehaviour
{
    int[] sceneIdsToLoad = { 1, 2 };

    // Start is called before the first frame update
    void Start()
    {
        // Only specifying the sceneName or sceneBuildIndex will load the Scene with the Single mode
        foreach (int id in sceneIdsToLoad)
        {
            SceneManager.LoadScene(id, LoadSceneMode.Additive);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
