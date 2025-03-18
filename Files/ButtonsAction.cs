using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonsAction : MonoBehaviour
{
    public void OpenGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void LoadLose()
    {
        SceneManager.LoadScene("Lose");
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Starter");
    }

    public void LoadWin()
    {
        SceneManager.LoadScene("Win");
    }

    public void CloseGame()
    { 
        Application.Quit();
    }
}
