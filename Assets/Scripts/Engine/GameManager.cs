using SVS;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private GameObject Plant1, Plant2, Plant3;
    private GameObject Hill;

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

    private void Start()
    {
        Plant1 = placementManager.prefabManager.Plant1;
        Plant2 = placementManager.prefabManager.Plant2;
        Plant3 = placementManager.prefabManager.Plant3;
        Hill = placementManager.prefabManager.Hill;

        SetSpeedMultiplierOfEntities();

        InitUIData();
        UIControllerEventSubscription();
        EconomyManagerEventSubscription();
        TouristManagerEventSubscription();
        AnimalManagerEventSubscription();
        TimeManagerEventSubscription();
        InputManagerEventSubscription();
    }

    private void Update()
    {
        if (PersistenceManager.MainMenuLoad)
        {
            LoadGame();
            PersistenceManager.MainMenuLoad = false;
        }
        cameraMovement.MoveCamera(new Vector3(inputManager.CameraMovementVector.x, 0, inputManager.CameraMovementVector.y));
    }


    private void ClearInputActions()
    {
        inputManager.OnMouseClick = null;
        inputManager.OnMouseHold = null;
        inputManager.OnMouseUp = null;
        inputManager.OnAnimalClick = null;
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
            }
        };
    }

    private void Carnivore1PurchaseHandler()
    {
        ClearInputActions();
        if (economyManager.HasEnoughMoney(economyManager.UnitCostOfCarnivore))
        {
            animalManager.BuyCarnivore1();
            economyManager.SpendMoney(economyManager.UnitCostOfCarnivore);
        }
    }

    private void Carnivore2PurchaseHandler()
    {
        ClearInputActions();
        if (economyManager.HasEnoughMoney(economyManager.UnitCostOfCarnivore))
        {
            animalManager.BuyCarnivore2();
            economyManager.SpendMoney(economyManager.UnitCostOfCarnivore);
        }
    }

    private void Herbivore1PurchaseHandler()
    {
        ClearInputActions();
        if (economyManager.HasEnoughMoney(economyManager.UnitCostOfHerbivore))
        {
            animalManager.BuyHerbivore1();
            economyManager.SpendMoney(economyManager.UnitCostOfHerbivore);
        }
    }

    private void Herbivore2PurchaseHandler()
    {
        ClearInputActions();
        if (economyManager.HasEnoughMoney(economyManager.UnitCostOfHerbivore))
        {
            animalManager.BuyHerbivore2();
            economyManager.SpendMoney(economyManager.UnitCostOfHerbivore);
        }
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

    private void SellAnimalHandler(bool isCancellation)
    {
        ClearInputActions();
        if (isCancellation)
        {
            return;
        }

        inputManager.OnAnimalClick += animalManager.SellAnimal;
        inputManager.OnAnimalClick += animal =>
        {
            economyManager.EarnMoney(100); // TODO: balance and query from Constatnts
        };
    }

    private void PauseButtonHandler()
    {
        timeManager.TogglePause();
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

    private void SetSpeedMultiplierOfEntities() => Entity.SpeedMultiplier = timeManager.EntitySpeedMultiplier;
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
        inputManager.IsArrowInputActive = false;
        inputManager.IsGameOver = true;
        uiController.ShowPauseMenu(false);
        uiController.ShowPopupWindow(isGameWon);
    }

    private void StartNewGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    private void ExitToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void ResumeGame()
    {
        if (!uiController.IsPaused)
        {
            timeManager.TogglePause();
        }
        inputManager.IsArrowInputActive = true;
        uiController.ShowPauseMenu(false);
    }

    private void SaveGame()
    {
        PersistenceManager.Difficulty = DifficultySelector.SelectedDifficulty;
        PersistenceManager.TimeData = timeManager.SaveData();
        PersistenceManager.EconomyManagerData = economyManager.SaveData();
        PersistenceManager.AnimalManagerData = animalManager.SaveData();
        PersistenceManager.TouristManagerData = touristManager.SaveData();
        PersistenceManager.PlacementManagerData = placementManager.SaveData();

        PersistenceManager.Save("save.json");
    }

    private void LoadGame()
    {
        if (!PersistenceManager.SaveExists("save.json")) // TODO: optionally could disable load button if save file does not exist
        {
            Debug.Log("Save file not found");
            return;
        }

        PersistenceManager.Load("save.json");
        DifficultySelector.SelectedDifficulty = PersistenceManager.Difficulty;
        timeManager.LoadData(PersistenceManager.TimeData);
        economyManager.LoadData(PersistenceManager.EconomyManagerData);
        placementManager.LoadData(PersistenceManager.PlacementManagerData);
        for (int i = 0; i < placementManager.width; ++i)
        {
            for (int j = 0; j < placementManager.height; ++j)
            {
                switch (placementManager.placementGrid[i, j])
                {
                    case CellType.Road:
                        placementManager.PlaceStructure(new Vector3Int(i, 0, j), placementManager.prefabManager.DeadEnd, CellType.Road);
                        roadManager.roadFixer.FixRoadAtPosition(placementManager, new Vector3Int(i, 0, j));
                        break;
                    case CellType.Water:
                        placementManager.PlaceStructure(new Vector3Int(i, 0, j), placementManager.prefabManager.Water, CellType.Water);
                        break;
                    case CellType.Nature:
                        switch (PersistenceManager.PlacementManagerData.StructureDictionaryNature[new Vector3Int(i, 0, j)])
                        {
                            case NatureType.Bush1:
                                placementManager.PlaceStructure(new Vector3Int(i, 0, j), Plant1, CellType.Nature);
                                break;
                            case NatureType.Tree2:
                                placementManager.PlaceStructure(new Vector3Int(i, 0, j), Plant2, CellType.Nature);
                                break;
                            case NatureType.Tree4:
                                placementManager.PlaceStructure(new Vector3Int(i, 0, j), Plant3, CellType.Nature);
                                break;
                            default:
                                Debug.LogError("Unknown nature type");
                                break;
                        }
                        break;
                    case CellType.Hill:
                        placementManager.PlaceStructure(new Vector3Int(i, 0, j), Hill, CellType.Hill);
                        break;
                    case CellType.Empty:
                        break;
                    default:
                        Debug.LogError("unknown celltype");
                        break;
                }
            }
        }
        touristManager.LoadData(PersistenceManager.TouristManagerData, placementManager);
        animalManager.LoadData(PersistenceManager.AnimalManagerData, placementManager);
        InitUIData();
    }
    
    private void InitUIData()
    {
        uiController.UpdateMoneyPanel(economyManager.Money);
        uiController.UpdateAdmissionFeePanel(economyManager.AdmissionFee);
        uiController.UpdateDatePanel(timeManager.CurrentTime);
        uiController.UpdateJeepPanel(touristManager.JeepCount);
        uiController.UpdateCarnivore1Panel(animalManager.Carnivore1Count);
        uiController.UpdateCarnivore2Panel(animalManager.Carnivore2Count);
        uiController.UpdateHerbivore1Panel(animalManager.Herbivore1Count);
        uiController.UpdateHerbivore2Panel(animalManager.Herbivore2Count);
        uiController.UpdateJeepPanel(touristManager.JeepCount);
        uiController.UpdateSatisfactionPanel(touristManager.Satisfaction);
        uiController.UpdateTouristsPanel(touristManager.TouristsInQueue);
    }

    private void UIControllerEventSubscription()
    {
        uiController.admissionFeeEndEdit += admissionFee =>
        {
            if (admissionFee != 0)
            {
                economyManager.AdmissionFee = admissionFee;
            }
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

        uiController.popupWindowNewGameButtonPressed += () => StartNewGame();
        uiController.popupWindowQuitButtonPressed += () => ExitToMainMenu();

        uiController.pauseMenuResumeButtonPressed += () => ResumeGame();
        uiController.pauseMenuSaveButtonPressed += () => SaveGame();
        uiController.pauseMenuLoadButtonPressed += () => LoadGame();
        uiController.pauseMenuQuitButtonPressed += () => ExitToMainMenu();
    }

    private void EconomyManagerEventSubscription()
    {
        economyManager.GoneBankrupt += () => GameOverHandler(false);
        economyManager.moneyChanged += money => uiController.UpdateMoneyPanel(money);
    }

    private void TimeManagerEventSubscription()
    {
        timeManager.Elapsed += () => economyManager.DailyMaintenance();
        timeManager.Elapsed += () => touristManager.ManageTick();
        timeManager.Elapsed += () => animalManager.ManageTick();
        timeManager.Elapsed += () => uiController.UpdateDatePanel(timeManager.CurrentTime);

        timeManager.TimeIntervalChanged += () => SetSpeedMultiplierOfEntities();
    }

    private void TouristManagerEventSubscription()
    {
        touristManager.TouristsInQueueChanged += tourists => uiController.UpdateTouristsPanel(tourists);
        touristManager.SatisfactionChanged += satisfaction => uiController.UpdateSatisfactionPanel(satisfaction);
        touristManager.JeepCountChanged += jeepCount => uiController.UpdateJeepPanel(jeepCount);
    }

    private void AnimalManagerEventSubscription()
    {
        animalManager.Carnivore1Changed += count => uiController.UpdateCarnivore1Panel(count);
        animalManager.Carnivore2Changed += count => uiController.UpdateCarnivore2Panel(count);
        animalManager.Herbivore1Changed += count => uiController.UpdateHerbivore1Panel(count);
        animalManager.Herbivore2Changed += count => uiController.UpdateHerbivore2Panel(count);
    }

    private void InputManagerEventSubscription()
    {
        inputManager.Paused += () =>
        {
            if (uiController.IsPauseMenuActive)
            {
                if (!uiController.IsPaused)
                {
                    timeManager.TogglePause();
                }
                inputManager.IsArrowInputActive = true;
                uiController.ShowPauseMenu(false);
            }
            else
            {
                if (!uiController.IsPaused)
                {
                    timeManager.TogglePause();
                }
                inputManager.IsArrowInputActive = false;
                uiController.ShowPauseMenu(true);
            }
        };
    }
}
