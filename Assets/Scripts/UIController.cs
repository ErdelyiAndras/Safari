using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    private enum ButtonGroup
    {
        Placement,
        Time,
        Remove
    }

    public Action JeepButtonPressed;
    public Action<bool> RoadButtonPressed;
    public Action Carnivore1ButtonPressed;
    public Action Carnivore2ButtonPressed;
    public Action Herbivore1ButtonPressed;
    public Action Herbivore2ButtonPressed;
    public Action<bool> Plant1ButtonPressed;
    public Action<bool> Plant2ButtonPressed;
    public Action<bool> Plant3ButtonPressed;
    public Action<bool> LakeButtonPressed;

    public Action PauseButtonPressed;
    public Action HourButtonPressed;
    public Action DayButtonPressed;
    public Action WeekButtonPressed;

    public Action<bool> SellButtonPressed;
    public Action<bool> RemoveButtonPressed;


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

    public Button SellButton;
    public Button RemoveButton;


    public Color placementButtonOutlineColor;
    public Color timeButtonOutlineColor;
    public Color removeButtonOutlineColor;


    private List<Button> placementButtonList;
    private List<Button> timeButtonList;
    private List<Button> removeButtonList;


    private Button selectedPlacementButton = null;
    private Button selectedTimeButton = null;
    private Button selectedRemoveButton = null;


    private bool isPaused = false;


    public Text moneyText;

    private void Awake()
    {
        placementButtonList = new List<Button>
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
            LakeButton
        };

        timeButtonList = new List<Button>
        {
            PauseButton,
            HourButton,
            DayButton,
            WeekButton
        };

        removeButtonList = new List<Button>
        {
            SellButton,
            RemoveButton
        };

        JeepButton.onClick.AddListener(() => UncancelablePlacementButtonPressedListener(JeepButtonPressed));

        RoadButton.onClick.AddListener(() => CancelableButtonPressedListener(
            ButtonGroup.Placement,
            RoadButton,
            RoadButtonPressed)
        );

        Carnivore1Button.onClick.AddListener(() => UncancelablePlacementButtonPressedListener(Carnivore1ButtonPressed));

        Carnivore2Button.onClick.AddListener(() => UncancelablePlacementButtonPressedListener(Carnivore2ButtonPressed));
        
        Herbivore1Button.onClick.AddListener(() => UncancelablePlacementButtonPressedListener(Herbivore1ButtonPressed));
        
        Herbivore2Button.onClick.AddListener(() => UncancelablePlacementButtonPressedListener(Herbivore2ButtonPressed));
        
        Plant1Button.onClick.AddListener(() => CancelableButtonPressedListener(
            ButtonGroup.Placement,
            Plant1Button,
            Plant1ButtonPressed)
        );
        
        Plant2Button.onClick.AddListener(() => CancelableButtonPressedListener(
            ButtonGroup.Placement,
            Plant2Button,
            Plant2ButtonPressed)
        );
        
        Plant3Button.onClick.AddListener(() => CancelableButtonPressedListener(
            ButtonGroup.Placement,
            Plant3Button,
            Plant3ButtonPressed)
        );
        
        LakeButton.onClick.AddListener(() => CancelableButtonPressedListener(
            ButtonGroup.Placement,
            LakeButton,
            LakeButtonPressed)
        );


        PauseButton.onClick.AddListener(PauseButtonPressedListener);
        HourButton.onClick.AddListener(() => TimeButtonPressedListener(HourButton, HourButtonPressed));
        DayButton.onClick.AddListener(() => TimeButtonPressedListener(DayButton, DayButtonPressed));
        WeekButton.onClick.AddListener(() => TimeButtonPressedListener(WeekButton, WeekButtonPressed));


        SellButton.onClick.AddListener(() => CancelableButtonPressedListener(
            ButtonGroup.Remove,
            SellButton,
            SellButtonPressed)
        );

        RemoveButton.onClick.AddListener(() => CancelableButtonPressedListener(
            ButtonGroup.Remove,
            RemoveButton,
            RemoveButtonPressed)
        );
    }

    private void Start()
    {
        InitTimeButtons(HourButton);
    }

    public void UpdateMoneyPanel(int money)
    {
        if (moneyText != null)
        {
            moneyText.text = $"$ {money.ToString()}";
        }
    }

    private void CancelableButtonPressedListener(
        ButtonGroup buttonGroup,
        Button currentButton,
        Action<bool> actionOfCurrentButton)
    {
        ref Button alreadySelectedButton = ref selectedPlacementButton;
        ref Button alreadySelectedAlternativeButton = ref selectedRemoveButton;
        List<Button> buttonsInGroup;
        List<Button> alternativeButtonsInGroup;
        Color groupColor;

        switch (buttonGroup)
        {
            case ButtonGroup.Placement:
                alreadySelectedButton = ref selectedPlacementButton;
                alreadySelectedAlternativeButton = ref selectedRemoveButton;
                buttonsInGroup = placementButtonList;
                alternativeButtonsInGroup = removeButtonList;
                groupColor = placementButtonOutlineColor;
                break;
            case ButtonGroup.Time:
                alreadySelectedButton = ref selectedTimeButton;
                alreadySelectedAlternativeButton = ref selectedTimeButton;
                buttonsInGroup = timeButtonList;
                alternativeButtonsInGroup = null;
                groupColor = timeButtonOutlineColor;
                break;
            case ButtonGroup.Remove:
                alreadySelectedButton = ref selectedRemoveButton;
                alreadySelectedAlternativeButton = ref selectedPlacementButton;
                buttonsInGroup = removeButtonList;
                alternativeButtonsInGroup = placementButtonList;
                groupColor = removeButtonOutlineColor;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(buttonGroup), buttonGroup, "Invalid button group");
        }

        ResetButtonColorOfGroup(buttonsInGroup);
        if (alreadySelectedAlternativeButton != null)
        {
            ResetButtonColorOfGroup(alternativeButtonsInGroup);
            alreadySelectedAlternativeButton = null;
        }
        if (alreadySelectedButton != null && alreadySelectedButton.GetInstanceID() == currentButton.GetInstanceID())
        {
            alreadySelectedButton = null;
            actionOfCurrentButton?.Invoke(true);
        }
        else
        {
            alreadySelectedButton = currentButton;
            ModifyOutline(currentButton, groupColor);
            actionOfCurrentButton?.Invoke(false);
        }

    }


    private void UncancelablePlacementButtonPressedListener(Action action)
    {
        ResetButtonColorOfGroup(placementButtonList);
        ResetButtonColorOfGroup(removeButtonList);
        selectedPlacementButton = null;
        selectedRemoveButton = null;
        action?.Invoke();
    }



    private void PauseButtonPressedListener()
    {
        isPaused = !isPaused;
        Text text = PauseButton.GetComponentInChildren<Text>();
        if (text != null)
        {
            text.text = isPaused ? "Resume" : "Pause";
        }
        PauseButtonPressed?.Invoke();
    }

    private void TimeButtonPressedListener(Button button, Action action)
    {
        ResetButtonColorOfGroup(timeButtonList);
        ModifyOutline(button, timeButtonOutlineColor);
        selectedTimeButton = button;
        action?.Invoke();
    }


    private void InitTimeButtons(Button button)
    {
        ModifyOutline(button, timeButtonOutlineColor);
        selectedTimeButton = button;
    }

    private void ModifyOutline(Button button, Color outlineColor)
    {
        var outline = button.GetComponent<Outline>();
        outline.effectColor = outlineColor;
        outline.enabled = true;
    }

    private void ResetButtonColorOfGroup(List<Button> buttonList)
    {
        foreach (Button button in buttonList)
        {
            button.GetComponent<Outline>().enabled = false;
        }
    }
}