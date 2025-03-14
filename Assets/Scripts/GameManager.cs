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
        uiController.UpdateMoneyPanel(economyManager.Money);
        uiController.JeepButtonPressed += JeepPurchaseHandler;
        uiController.RoadButtonPressed += isCancellation => RoadPlacementHandler(isCancellation);
        uiController.Carnivore1ButtonPressed += Carnivore1PurchaseHandler;
        uiController.Carnivore2ButtonPressed += Carnivore2PurchaseHandler;
        uiController.Herbivore1ButtonPressed += Herbivore1PurchaseHandler;
        uiController.Herbivore2ButtonPressed += Herbivore2PurchaseHandler;
        uiController.Plant1ButtonPressed += isCancellation => NaturePlacementHandler(isCancellation, Plant1);
        uiController.Plant2ButtonPressed += isCancellation => NaturePlacementHandler(isCancellation, Plant2);
        uiController.Plant3ButtonPressed += isCancellation => NaturePlacementHandler(isCancellation, Plant3);
        uiController.LakeButtonPressed += isCancellation => WaterPlacementHandler(isCancellation);

        uiController.PauseButtonPressed += PauseButtonHandler;
        uiController.HourButtonPressed += HourButtonHandler;
        uiController.DayButtonPressed += DayButtonHandler;
        uiController.WeekButtonPressed += WeekButtonHandler;

        uiController.SellButtonPressed += isCancellation => SellAnimalHandler(isCancellation);
        uiController.RemoveButtonPressed += isCancellation => RemoveObjectHandler(isCancellation);

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

    private void JeepPurchaseHandler()
    {
        ClearInputActions();
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
                uiController.UpdateMoneyPanel(economyManager.Money);
            }
        };
    }

    private void Carnivore1PurchaseHandler()
    {
        ClearInputActions();
    }

    private void Carnivore2PurchaseHandler()
    {
        ClearInputActions();
    }

    private void Herbivore1PurchaseHandler()
    {
        ClearInputActions();
    }

    private void Herbivore2PurchaseHandler()
    {
        ClearInputActions();
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
                uiController.UpdateMoneyPanel(economyManager.Money);
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
                uiController.UpdateMoneyPanel(economyManager.Money);
            }
        };
    }

    private void SellAnimalHandler(bool isCancellation)
    {
        ClearInputActions();
        if (isCancellation)
        {
            return;
        }
        
        // inputManager.OnMouseClick += ;
    }

    private void PauseButtonHandler()
    {
        
    }

    private void HourButtonHandler()
    {

    }

    private void DayButtonHandler()
    {

    }

    private void WeekButtonHandler()
    {

    }

    private void RemoveObjectHandler(bool isCancellation)
    {
        ClearInputActions();
        if (isCancellation)
        {
            return;
        }

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
