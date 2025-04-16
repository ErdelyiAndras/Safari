using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public MainMenuUIController mainMenuUIController;

    private void Start()
    {
        mainMenuUIController.easyButtonPressed += () => { DifficultySelector.SelectedDifficulty = Difficulty.Easy; };
        mainMenuUIController.normalButtonPressed += () => { DifficultySelector.SelectedDifficulty = Difficulty.Normal; };
        mainMenuUIController.hardButtonPressed += () => { DifficultySelector.SelectedDifficulty = Difficulty.Hard; };

        mainMenuUIController.startNewGameButtonPressed += () => NewGameHandler();
        mainMenuUIController.loadGameButtonPressed += () => LoadGameHandler();
        mainMenuUIController.quitButtonPressed += () => QuitHandler();
    }

    private void NewGameHandler()
    {
        SceneManager.LoadScene("GameScene");
    }

    private void LoadGameHandler()
    {
        if (!PersistenceManager.SaveExists("save.json"))
        {
            Debug.Log("No save file found. Please start a new game first.");
            return;
        }

        PersistenceManager.MainMenuLoad = true;
        SceneManager.LoadScene("GameScene");
    }

    private void QuitHandler()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
