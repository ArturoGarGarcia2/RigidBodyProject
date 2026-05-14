using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject mainPanel;
    public GameObject levelsPanel;

    void Start()
    {
        GoToMain();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void GoToLevels()
    {
        mainPanel.SetActive(false);
        levelsPanel.SetActive(true);
    }

    public void GoToMain()
    {
        mainPanel.SetActive(true);
        levelsPanel.SetActive(false);
    }

    public void GoToLevel(string targetLevel) => SceneManager.LoadScene("Level" + targetLevel);
}
