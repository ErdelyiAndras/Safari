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
    public PlacementManager placementManager;

    public Difficulty gameDifficulty;

    private void Start()
    {
        economyManager.InitMoney(gameDifficulty);
        //uiController.JeepButtonPressed += Jeep;
        uiController.RoadButtonPressed += isCancellation => RoadPlacementHandler(isCancellation);
        //uiController.Carnivore1ButtonPressed += C1;
        //uiController.Carnivore2ButtonPressed += C2;
        //uiController.Herbivore1ButtonPressed += H1;
        //uiController.Herbivore2ButtonPressed += H2;
        uiController.Plant1ButtonPressed += isCancellation => NaturePlacementHandler(isCancellation, Plant1);
        uiController.Plant2ButtonPressed += isCancellation => NaturePlacementHandler(isCancellation, Plant2);
        uiController.Plant3ButtonPressed += isCancellation => NaturePlacementHandler(isCancellation, Plant3);
        uiController.LakeButtonPressed += isCancellation => WaterPlacementHandler(isCancellation);
        uiController.PauseButtonPressed += RemoveObjectHandler;
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


    private void RoadPlacementHandler(bool isCancellation)
    {
        ClearInputActions();
        if (isCancellation)
        {
            return;
        }

        inputManager.OnMouseClick += roadManager.PlaceObject;
        inputManager.OnMouseHold += roadManager.PlaceObject;
        inputManager.OnMouseUp += () =>
        {
            int cost = roadManager.Count * economyManager.UnitCostOfRoad;
            bool result = economyManager.HasEnoughMoney(cost);
            roadManager.FinalizeObject(result);
            if (result)
            {
                economyManager.SpendMoney(cost);
            }
        };
    }

    private void NaturePlacementHandler(bool isCancellation, GameObject type)
    {
        ClearInputActions();
        if (isCancellation)
        {
            return;
        }

        inputManager.OnMouseClick += natureManager.PlaceObject;
        inputManager.OnMouseUp += () =>
        {
            int cost = natureManager.Count * economyManager.UnitCostOfNature;
            bool result = economyManager.HasEnoughMoney(cost);
            natureManager.FinalizeObject(result, type);
            if (result)
            {
                economyManager.SpendMoney(cost);
            }
        };
    }

    private void WaterPlacementHandler(bool isCancellation)
    {
        ClearInputActions();
        if (isCancellation)
        {
            return;
        }

        inputManager.OnMouseClick += waterManager.PlaceObject;
        inputManager.OnMouseUp += () =>
        {
            int cost = waterManager.Count * economyManager.UnitCostOfWater;
            bool result = economyManager.HasEnoughMoney(cost);
            waterManager.FinalizeObject(result);
            if (result)
            {
                economyManager.SpendMoney(cost);
            }
        };
    }

    private void RemoveObjectHandler()
    {
        ClearInputActions();
        inputManager.OnMouseClick += placementManager.RemoveStructure;
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
