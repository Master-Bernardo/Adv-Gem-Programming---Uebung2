using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {

    public GameObject controlsPanel;
    public GameObject mainMenuPanel;
    public GameObject controlsBackgroundPrefab;
    GameObject currentlyInstantiatedControls;

	public void GoToControls()
    {
        controlsPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
        currentlyInstantiatedControls = Instantiate(controlsBackgroundPrefab);
    }

    public void GoToMainMenu()
    {
        controlsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
        Destroy(currentlyInstantiatedControls);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
