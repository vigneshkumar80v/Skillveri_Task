using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public void OpenProblem1()
    {
        SceneManager.LoadScene("Artifact_Scene");
    }

    public void OpenProblem2()
    {
        SceneManager.LoadScene("Enemy_Scene");
    }

    public void OpenProblem3()
    {
        SceneManager.LoadScene("Table_Scene");
    }

    public void ReloadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
