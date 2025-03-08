using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public event EventHandler JeepButtonPressed;
    public event EventHandler RoadButtonPressed;
    public event EventHandler Carnivore1ButtonPressed;
    public event EventHandler Carnivore2ButtonPressed;
    public event EventHandler Herbivore1ButtonPressed;
    public event EventHandler Herbivore2ButtonPressed;
    public event EventHandler Plant1ButtonPressed;
    public event EventHandler Plant2ButtonPressed;
    public event EventHandler Plant3ButtonPressed;
    public event EventHandler LakeButtonPressed;
    public event EventHandler PauseButtonPressed;
    public event EventHandler HourButtonPressed;
    public event EventHandler DayButtonPressed;
    public event EventHandler WeekButtonPressed;

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

    private void Start()
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
        JeepButtonPressed?.Invoke(this, EventArgs.Empty);
    }

    private void RoadButtonPressedListener()
    {
        ResetButtonColor();
        ModifyOutline(RoadButton);
        RoadButtonPressed?.Invoke(this, EventArgs.Empty);
    }

    private void Carnivore1ButtonPressedListener()
    {
        ResetButtonColor();
        ModifyOutline(Carnivore1Button);
        Carnivore1ButtonPressed?.Invoke(this, EventArgs.Empty);
    }

    private void Carnivore2ButtonPressedListener()
    {
        ResetButtonColor();
        ModifyOutline(Carnivore2Button);
        Carnivore2ButtonPressed?.Invoke(this, EventArgs.Empty);
    }

    private void Herbivore1ButtonPressedListener()
    {
        ResetButtonColor();
        ModifyOutline(Herbivore1Button);
        Herbivore1ButtonPressed?.Invoke(this, EventArgs.Empty);
    }

    private void Herbivore2ButtonPressedListener()
    {
        ResetButtonColor();
        ModifyOutline(Herbivore2Button);
        Herbivore2ButtonPressed?.Invoke(this, EventArgs.Empty);
    }

    private void Plant1ButtonPressedListener()
    {
        ResetButtonColor();
        ModifyOutline(Plant1Button);
        Plant1ButtonPressed?.Invoke(this, EventArgs.Empty);
    }

    private void Plant2ButtonPressedListener()
    {
        ResetButtonColor();
        ModifyOutline(Plant2Button);
        Plant2ButtonPressed?.Invoke(this, EventArgs.Empty);
    }

    private void Plant3ButtonPressedListener()
    {
        ResetButtonColor();
        ModifyOutline(Plant3Button);
        Plant3ButtonPressed?.Invoke(this, EventArgs.Empty);
    }

    private void LakeButtonPressedListener()
    {
        ResetButtonColor();
        ModifyOutline(LakeButton);
        LakeButtonPressed?.Invoke(this, EventArgs.Empty);
    }

    private void PauseButtonPressedListener()
    {
        ResetButtonColor();
        ModifyOutline(PauseButton);
        PauseButtonPressed?.Invoke(this, EventArgs.Empty);
    }

    private void HourButtonPressedListener()
    {
        ResetButtonColor();
        ModifyOutline(HourButton);
        HourButtonPressed?.Invoke(this, EventArgs.Empty);
    }

    private void DayButtonPressedListener()
    {
        ResetButtonColor();
        ModifyOutline(DayButton);
        DayButtonPressed?.Invoke(this, EventArgs.Empty);
    }

    private void WeekButtonPressedListener()
    {
        ResetButtonColor();
        ModifyOutline(WeekButton);
        WeekButtonPressed?.Invoke(this, EventArgs.Empty);
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
