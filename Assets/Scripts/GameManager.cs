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
    public TimeManager timeManager;
    public TouristManager touristManager;

    public Difficulty gameDifficulty;

    private void Start()
    {
        economyManager.InitMoney(gameDifficulty);

        uiController.UpdateMoneyPanel(economyManager.Money);
        uiController.UpdateAdmissionFeePanel(economyManager.AdmissionFee);

        uiController.admissionFeeEndEdit += admissionFee =>
        {
            economyManager.AdmissionFee = admissionFee;
            uiController.UpdateAdmissionFeePanel(economyManager.AdmissionFee);
        };

        uiController.JeepButtonPressed += JeepPurchaseHandler;
        uiController.RoadButtonPressed += isCancellation => RoadPlacementHandler(isCancellation);
        uiController.Carnivore1ButtonPressed += () => Carnivore1PurchaseHandler();
        uiController.Carnivore2ButtonPressed += () => Carnivore2PurchaseHandler();
        uiController.Herbivore1ButtonPressed += () => Herbivore1PurchaseHandler();
        uiController.Herbivore2ButtonPressed += () => Herbivore2PurchaseHandler();
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

        economyManager.GoneBankrupt += () => GameOverHandler(false);


        timeManager.Resume();        
        timeManager.Elapsed += () => TimeManagerElapsedHandler();
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
        bool result = economyManager.HasEnoughMoney(economyManager.UnitCostOfJeep);
        if (result)
        {
            touristManager.AcquireNewJeep();
            economyManager.SpendMoney(economyManager.UnitCostOfJeep);
        }
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
        if (economyManager.HasEnoughMoney(economyManager.UnitCostOfCarnivore))
        {
            animalManager.SpawnAnimal(animalManager.carnivore1Prefab);
            economyManager.SpendMoney(economyManager.UnitCostOfCarnivore);
            uiController.UpdateMoneyPanel(economyManager.Money);
        }
        //animalManager.SpawnAnimal(animalManager.carnivore1Prefab);
    }

    private void Carnivore2PurchaseHandler()
    {
        ClearInputActions();
        if (economyManager.HasEnoughMoney(economyManager.UnitCostOfCarnivore))
        {
            animalManager.SpawnAnimal(animalManager.carnivore2Prefab);
            economyManager.SpendMoney(economyManager.UnitCostOfCarnivore);
            uiController.UpdateMoneyPanel(economyManager.Money);
        }
        //animalManager.SpawnAnimal(animalManager.carnivore2Prefab);
    }

    private void Herbivore1PurchaseHandler()
    {
        ClearInputActions();
        if (economyManager.HasEnoughMoney(economyManager.UnitCostOfHerbivore))
        {
            animalManager.SpawnAnimal(animalManager.herbivore1Prefab);
            economyManager.SpendMoney(economyManager.UnitCostOfHerbivore);
            uiController.UpdateMoneyPanel(economyManager.Money);
        }
        //animalManager.SpawnAnimal(animalManager.herbivore1Prefab);
    }

    private void Herbivore2PurchaseHandler()
    {
        ClearInputActions();
        if (economyManager.HasEnoughMoney(economyManager.UnitCostOfHerbivore))
        {
            animalManager.SpawnAnimal(animalManager.herbivore2Prefab);
            economyManager.SpendMoney(economyManager.UnitCostOfHerbivore);
            uiController.UpdateMoneyPanel(economyManager.Money);
        }
        //animalManager.SpawnAnimal(animalManager.herbivore2Prefab);
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
        timeManager.SetTimeInterval(TimeInterval.HOUR);
    }

    private void DayButtonHandler()
    {
        timeManager.SetTimeInterval(TimeInterval.DAY);
    }

    private void WeekButtonHandler()
    {
        timeManager.SetTimeInterval(TimeInterval.WEEK);
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

    private void TimeManagerElapsedHandler()
    {
        Debug.Log("Elapsed");
        economyManager.DailyMaintenance();
        uiController.UpdateMoneyPanel(economyManager.Money);
    }

    private void GameOverHandler(bool isGameWon)
    {
        if (isGameWon)
        {
            //TODO
        }
        else
        {
            //TODO
            Debug.Log("Lose");
        }
    }
    
}
