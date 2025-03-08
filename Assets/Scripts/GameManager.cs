using SVS;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject waterPrefab, Plant1, Plant2, Plant3;

    public CameraMovement cameraMovement;
    public InputManager inputManager;
    public RoadManager roadManager;
    public NatureManager natureManager;
    public WaterManager waterManager;
    public EconomyManager economyManager;

    private void Start()
    {
        uiController.OnRoadPlacement += RoadPlacementHandler; 
        uiController.OnWaterPlacement += WaterPlacementHandler();
        uiController.OnPlant1Placement += NaturePlacementHandler(Plant1);
        uiController.OnPlant2Placement += NaturePlacementHandler(Plant2);
        uiController.OnPlant3Placement += NaturePlacementHandler(Plant3);
        economyManager.GameOver += GameOverHandler;

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
        bool result = economyManager.SpendMoney(roadManager.Cost);
        inputManager.OnMouseUp += () => roadManager.FinalizeObject(result);
    }

    private void NaturePlacementHandler(GameObject type)
    {
        ClearInputActions();

        inputManager.OnMouseClick += natureManager.PlaceObject;
        bool result = economyManager.SpendMoney(natureManager.Cost);
        inputManager.OnMouseClick += position => natureManager.FinalizeObject(result, type);
    }
    private void WaterPlacementHandler()
    {
        ClearInputActions();

        inputManager.OnMouseClick += waterManager.PlaceObject;
        bool result = economyManager.SpendMoney(waterManager.Cost);
        inputManager.OnMouseClick += position => waterManager.FinalizeObject(result);
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
