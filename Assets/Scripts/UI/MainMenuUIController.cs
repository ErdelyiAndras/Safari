using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUIController : MonoBehaviour
{
    public Action startNewGameButtonPressed;
    public Action loadGameButtonPressed;
    public Action quitButtonPressed;
    public Action easyButtonPressed;
    public Action normalButtonPressed;
    public Action hardButtonPressed;

    public Button startNewGameButton;
    public Button loadGameButton;
    public Button quitButton;
    public Button easyButton;
    public Button normalButton;
    public Button hardButton;

    public Color outlineColor;

    private List<Button> difficultyButtonList;

    private Button selectedDifficultyButton = null;

    private void Awake()
    {
        difficultyButtonList = new List<Button>
        {
            easyButton,
            normalButton,
            hardButton
        };

        startNewGameButton.onClick.AddListener(OnStartNewGameButtonPressed);
        loadGameButton.onClick.AddListener(OnLoadGameButtonPressed);
        quitButton.onClick.AddListener(OnQuitButtonPressed);
        easyButton.onClick.AddListener(OnEasyButtonPressed);
        normalButton.onClick.AddListener(OnNormalButtonPressed);
        hardButton.onClick.AddListener(OnHardButtonPressed);
    }

    private void Start()
    {
        selectedDifficultyButton = normalButton;
        UpdateOutlines(selectedDifficultyButton);
    }

    private void OnStartNewGameButtonPressed()
    {
        startNewGameButtonPressed?.Invoke();
    }

    private void OnLoadGameButtonPressed()
    {
        loadGameButtonPressed?.Invoke();
    }

    private void OnQuitButtonPressed()
    {
        quitButtonPressed?.Invoke();
    }

    private void OnEasyButtonPressed()
    {
        UpdateOutlines(easyButton);
        easyButtonPressed?.Invoke();
    }

    private void OnNormalButtonPressed()
    {
        UpdateOutlines(normalButton);
        normalButtonPressed?.Invoke();
    }

    private void OnHardButtonPressed()
    {
        UpdateOutlines(hardButton);
        hardButtonPressed?.Invoke();
    }

    private void UpdateOutlines(Button button)
    {
        ResetButtonColor();
        ModifyOutline(button);
    }

    private void ModifyOutline(Button button)
    {
        var outline = button.GetComponent<Outline>();
        outline.effectColor = outlineColor;
        outline.enabled = true;
    }

    private void ResetButtonColor()
    {
        foreach (Button button in difficultyButtonList)
        {
            button.GetComponent<Outline>().enabled = false;
        }
    }
}
