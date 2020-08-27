using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    [SerializeField]
    GameObject escapePanel_;

    [SerializeField]
    AudioSource audioSource_;

    VolumeManager volumeManager_;

    [SerializeField]
    GameObject mainMenuWidget_;

    bool isMainMenu = true;

    UIManager uiManager_;

    // Start is called before the first frame update
    void Start()
    {
        audioSource_.Play();
        volumeManager_ = VolumeManager.Instance;
        uiManager_ = UIManager.Instance;
        audioSource_.volume = volumeManager_.BackgroundMusicVolume;

        mainMenuWidget_.SetActive(false);
        escapePanel_.SetActive(false);
    }

    public void StartTheGame()
    {
        UIManager.Instance.ShowMenuScreen(false);
        SceneManager.LoadScene(2, LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync(0);
        isMainMenu = false;
        mainMenuWidget_.SetActive(true);
        audioSource_.Stop();
    }

    public void GoToMainMenu()
    {
        UIManager.Instance.ShowMenuScreen(true);
        UIManager.Instance.ClearGameState();
        SceneManager.LoadScene(0, LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync(2);
        isMainMenu = true;
        escapePanel_.SetActive(false);
        mainMenuWidget_.SetActive(false);
        audioSource_.Play();
    }

    public void Restart()
    {
        UIManager.Instance.ClearGameState();
        SceneManager.LoadScene(2, LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync(2);
        escapePanel_.SetActive(false);
        mainMenuWidget_.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        audioSource_.volume = volumeManager_.BackgroundMusicVolume;

        if (Input.GetKeyDown(KeyCode.Escape)
            && !isMainMenu)
        {
            ToggleEscapePanel();
        }
    }

    public void ToggleEscapePanel()
    {
        escapePanel_.SetActive(!escapePanel_.activeSelf);
        mainMenuWidget_.SetActive(!mainMenuWidget_.activeSelf);
        uiManager_.DisableDialogueInput = !uiManager_.DisableDialogueInput;
    }
}
