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
    public AnimalManager animalManager;

    public Difficulty gameDifficulty;

    private void Start()
    {
        economyManager.InitMoney(gameDifficulty);
        //uiController.JeepButtonPressed += Jeep;
        uiController.RoadButtonPressed += RoadPlacementHandler;
        uiController.Carnivore1ButtonPressed += () => animalManager.SpawnAnimal(animalManager.carnivore1Prefab);
        uiController.Carnivore2ButtonPressed += () => animalManager.SpawnAnimal(animalManager.carnivore2Prefab);
        uiController.Herbivore1ButtonPressed += () => animalManager.SpawnAnimal(animalManager.herbivore1Prefab);
        uiController.Herbivore2ButtonPressed += () => animalManager.SpawnAnimal(animalManager.herbivore2Prefab);
        uiController.Plant1ButtonPressed += () => NaturePlacementHandler(Plant1);
        uiController.Plant2ButtonPressed += () => NaturePlacementHandler(Plant2);
        uiController.Plant3ButtonPressed += () => NaturePlacementHandler(Plant3);
        uiController.LakeButtonPressed += WaterPlacementHandler;
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


    private void RoadPlacementHandler()
    {
        ClearInputActions();

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

    private void NaturePlacementHandler(GameObject type)
    {
        ClearInputActions();

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

    private void WaterPlacementHandler()
    {
        ClearInputActions();

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
