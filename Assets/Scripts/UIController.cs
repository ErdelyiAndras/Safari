using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Action JeepButtonPressed;
    public Action RoadButtonPressed;
    public Action Carnivore1ButtonPressed;
    public Action Carnivore2ButtonPressed;
    public Action Herbivore1ButtonPressed;
    public Action Herbivore2ButtonPressed;
    public Action Plant1ButtonPressed;
    public Action Plant2ButtonPressed;
    public Action Plant3ButtonPressed;
    public Action LakeButtonPressed;
    public Action PauseButtonPressed;
    public Action HourButtonPressed;
    public Action DayButtonPressed;
    public Action WeekButtonPressed;

    public Button JeepButton;
    public Button RoadButton;
    public Button Carnivore1Button;
    public Button Carnivore2Button;
    public Button Herbivore1Button;
    public Button Herbivore2Button;
    public Button Plant1Button;
    public Button Plant2Button;
    public Button Plant3Button;
    public Button LakeButton;
    public Button PauseButton;
    public Button HourButton;
    public Button DayButton;
    public Button WeekButton;

    public Color outlineColor;

    private List<Button> buttonList;

    private void Awake()
    { 
        buttonList = new List<Button>
        {
            JeepButton,
            RoadButton,
            Carnivore1Button,
            Carnivore2Button,
            Herbivore1Button,
            Herbivore2Button,
            Plant1Button,
            Plant2Button,
            Plant3Button,
            LakeButton,
            PauseButton,
            HourButton,
            DayButton,
            WeekButton
        };

        JeepButton.onClick.AddListener(JeepButtonPressedListener);
        RoadButton.onClick.AddListener(RoadButtonPressedListener);
        Carnivore1Button.onClick.AddListener(Carnivore1ButtonPressedListener);
        Carnivore2Button.onClick.AddListener(Carnivore2ButtonPressedListener);
        Herbivore1Button.onClick.AddListener(Herbivore1ButtonPressedListener);
        Herbivore2Button.onClick.AddListener(Herbivore2ButtonPressedListener);
        Plant1Button.onClick.AddListener(Plant1ButtonPressedListener);
        Plant2Button.onClick.AddListener(Plant2ButtonPressedListener);
        Plant3Button.onClick.AddListener(Plant3ButtonPressedListener);
        LakeButton.onClick.AddListener(LakeButtonPressedListener);
        PauseButton.onClick.AddListener(PauseButtonPressedListener);
        HourButton.onClick.AddListener(HourButtonPressedListener);
        DayButton.onClick.AddListener(DayButtonPressedListener);
        WeekButton.onClick.AddListener(WeekButtonPressedListener);
    }

    private void JeepButtonPressedListener()
    {
        ResetButtonColor();
        ModifyOutline(JeepButton);
        JeepButtonPressed?.Invoke();
    }

    private void RoadButtonPressedListener()
    {
        ResetButtonColor();
        ModifyOutline(RoadButton);
        RoadButtonPressed?.Invoke();
    }

    private void Carnivore1ButtonPressedListener()
    {
        ResetButtonColor();
        ModifyOutline(Carnivore1Button);
        Carnivore1ButtonPressed?.Invoke();
    }

    private void Carnivore2ButtonPressedListener()
    {
        ResetButtonColor();
        ModifyOutline(Carnivore2Button);
        Carnivore2ButtonPressed?.Invoke();
    }

    private void Herbivore1ButtonPressedListener()
    {
        ResetButtonColor();
        ModifyOutline(Herbivore1Button);
        Herbivore1ButtonPressed?.Invoke();
    }

    private void Herbivore2ButtonPressedListener()
    {
        ResetButtonColor();
        ModifyOutline(Herbivore2Button);
        Herbivore2ButtonPressed?.Invoke();
    }

    private void Plant1ButtonPressedListener()
    {
        ResetButtonColor();
        ModifyOutline(Plant1Button);
        Plant1ButtonPressed?.Invoke();
    }

    private void Plant2ButtonPressedListener()
    {
        ResetButtonColor();
        ModifyOutline(Plant2Button);
        Plant2ButtonPressed?.Invoke();
    }

    private void Plant3ButtonPressedListener()
    {
        ResetButtonColor();
        ModifyOutline(Plant3Button);
        Plant3ButtonPressed?.Invoke();
    }

    private void LakeButtonPressedListener()
    {
        ResetButtonColor();
        ModifyOutline(LakeButton);
        LakeButtonPressed?.Invoke();
    }

    private void PauseButtonPressedListener()
    {
        ResetButtonColor();
        ModifyOutline(PauseButton);
        PauseButtonPressed?.Invoke();
    }

    private void HourButtonPressedListener()
    {
        ResetButtonColor();
        ModifyOutline(HourButton);
        HourButtonPressed?.Invoke();
    }

    private void DayButtonPressedListener()
    {
        ResetButtonColor();
        ModifyOutline(DayButton);
        DayButtonPressed?.Invoke();
    }

    private void WeekButtonPressedListener()
    {
        ResetButtonColor();
        ModifyOutline(WeekButton);
        WeekButtonPressed?.Invoke();
    }

    private void ModifyOutline(Button button)
    {
        var outline = button.GetComponent<Outline>();
        outline.effectColor = outlineColor;
        outline.enabled = true;
    }

    private void ResetButtonColor()
    {
        foreach (Button button in buttonList)
        {
            button.GetComponent<Outline>().enabled = false;
        }
    }
}
