using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public MainMenuUIController mainMenuUIController;

    private void Awake()
    {
        mainMenuUIController.easyButtonPressed += () => { DifficultySelector.SelectedDifficulty = Difficulty.Easy; };
        mainMenuUIController.normalButtonPressed += () => { DifficultySelector.SelectedDifficulty = Difficulty.Normal; };
        mainMenuUIController.hardButtonPressed += () => { DifficultySelector.SelectedDifficulty = Difficulty.Hard; };
    }
}
