using SVS;
using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject Plant1, Plant2, Plant3;

    public CameraMovement cameraMovement;
    public InputManager inputManager;
    public RoadManager roadManager;
    public NatureManager natureManager;
    public WaterManager waterManager;
    public EconomyManager economyManager;
    public UIController uiController;

    public Difficulty gameDifficulty;

    private void Start()
    {
        //economyManager.InitMoney(gameDifficulty);
        //uiController.JeepButtonPressed += Jeep;
        uiController.RoadButtonPressed += RoadPlacementHandler;
        //uiController.Carnivore1ButtonPressed += C1;
        //uiController.Carnivore2ButtonPressed += C2;
        //uiController.Herbivore1ButtonPressed += H1;
        //uiController.Herbivore2ButtonPressed += H2;
        uiController.Plant1ButtonPressed += () => NaturePlacementHandler(Plant1);
        uiController.Plant2ButtonPressed += () => NaturePlacementHandler(Plant2);
        uiController.Plant3ButtonPressed += () => NaturePlacementHandler(Plant3);
        uiController.LakeButtonPressed += WaterPlacementHandler;
        //uiController.PauseButtonPressed += Pause;
        //uiController.HourButtonPressed += Hour;
        //uiController.DayButtonPressed += Day;
        //uiController.WeekButtonPressed += Week;
        // economyManager.GameOver += GameOverHandler;

    }
    private void Update()
    {
        cameraMovement.MoveCamera(new Vector3(inputManager.CameraMovementVector.x,0, inputManager.CameraMovementVector.y));
    }


    private void ClearInputActions()
    {
        inputManager.OnMouseClick = null;
        inputManager.OnMouseHold = null;
        inputManager.OnMouseUp = null;
    }


    private void RoadPlacementHandler()
    {
        ClearInputActions();
        inputManager.OnMouseClick += roadManager.PlaceObject;
        inputManager.OnMouseHold += roadManager.PlaceObject;
        // bool result = economyManager.SpendMoney(roadManager.Cost);
        bool result = true;
        inputManager.OnMouseUp += () => roadManager.FinalizeObject(result);
    }

    private void NaturePlacementHandler(GameObject type)
    {
        ClearInputActions();

        inputManager.OnMouseClick += natureManager.PlaceObject;
        // bool result = economyManager.SpendMoney(natureManager.Cost);
        bool result = true;
        inputManager.OnMouseUp += () => natureManager.FinalizeObject(result, type);
    }

    private void WaterPlacementHandler()
    {
        ClearInputActions();

        inputManager.OnMouseClick += waterManager.PlaceObject;
        //bool result = economyManager.SpendMoney(waterManager.Cost);
        bool result = true;
        inputManager.OnMouseUp += () => waterManager.FinalizeObject(result);
    }

    private void GameOverHandler(bool result)
    {
        if (result)
        {
            //TODO
        }
        else
        {
            //TODO
        }
    }
    
}
