using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public interface IInputProvider
{
    public Vector3 MousePosition { get; }
    public float GetAxis(string axisName);
    public bool GetKeyDown(KeyCode key);
    public bool GetMouseButtonDown(int button);
    public bool GetMouseButtonUp(int button);
    public bool GetMouseButton(int button);
}

public class InputManager : MonoBehaviour
{
    private class DefaultInputProvider : IInputProvider
    {
        public Vector3 MousePosition => Input.mousePosition;

        public float GetAxis(string axisName)
        {
            if (axisName == "Horizontal")
            {
                return Input.GetAxis("Horizontal");
            }
            else if (axisName == "Vertical")
            {
                return Input.GetAxis("Vertical");
            }
            throw new NotImplementedException($"Axis '{axisName}' is not implemented.");
        }

        public bool GetKeyDown(KeyCode key) => Input.GetKeyDown(key);

        public bool GetMouseButton(int button) => Input.GetMouseButton(button);

        public bool GetMouseButtonDown(int button) => Input.GetMouseButtonDown(button);

        public bool GetMouseButtonUp(int button) => Input.GetMouseButtonUp(button);
    }

    public IInputProvider InputProvider { get; set; } = new DefaultInputProvider();

    public Action<Vector3Int> OnMouseClick, OnMouseHold;
    public Action OnMouseUp;
    public Action<GameObject> OnAnimalClick;
    public Action Paused;
    private Vector2 cameraMovementVector;
    private ScrollRect scrollRect;

    public bool IsArrowInputActive { get; set; } = true;
    public bool IsGameOver { get; set; } = false;

    public Camera mainCamera;

    public LayerMask groundMask;
    public LayerMask animalMask;

    public Vector2 CameraMovementVector
    {
        get { return cameraMovementVector; }
    }

    private void Start()
    {
        // disable scrollbar usage with arrow keys
        scrollRect = FindFirstObjectByType<ScrollRect>();

        if (scrollRect != null && scrollRect.horizontalScrollbar != null)
        {
            scrollRect.horizontalScrollbar.navigation = new Navigation
            {
                mode = Navigation.Mode.None
            };
        }
    }

    private void Update()
    {
        OnClickDown();
        OnClickUp();
        OnClickHold();
        OnClickDown_Animal();
        if (IsArrowInputActive == true)
        {
            CheckArrowInput();
        }
        else
        {
            cameraMovementVector = Vector2.zero;
        }
        if (!IsGameOver)
        {
            CheckPauseInput();
        }
    }

    private Vector3Int? RaycastGround()
    {
        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(InputProvider.MousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundMask))
        {
            Vector3Int positionInt = Vector3Int.RoundToInt(hit.point);
            return positionInt;
        }
        return null;
    }

    private void CheckArrowInput()
    {
        // disable movement when input field is selected
        if (EventSystem.current.currentSelectedGameObject == null ||
            EventSystem.current.currentSelectedGameObject.GetComponent<InputField>() == null)
        {
            cameraMovementVector = new Vector2(InputProvider.GetAxis("Horizontal"), InputProvider.GetAxis("Vertical"));
        }
        else
        {
            cameraMovementVector = Vector2.zero;
        }
    }

    private void CheckPauseInput()
    {
        if (InputProvider.GetKeyDown(KeyCode.Escape))
        {
            Paused?.Invoke();
        }
    }

    private void OnClickHold()
    {
        if (InputProvider.GetMouseButton(0) && EventSystem.current.IsPointerOverGameObject() == false)
        {
            var position = RaycastGround();
            if (position != null)
                OnMouseHold?.Invoke(position.Value);

        }
    }

    private void OnClickUp()
    {
        if (InputProvider.GetMouseButtonUp(0) && EventSystem.current.IsPointerOverGameObject() == false)
        {
            OnMouseUp?.Invoke();

        }
    }

    private void OnClickDown()
    {
        if (InputProvider.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject() == false)
        {
            var position = RaycastGround();
            if (position != null)
                OnMouseClick?.Invoke(position.Value);

        }
    }

    private void OnClickDown_Animal()
    {
        if (InputProvider.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject() == false)
        {
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(InputProvider.MousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, animalMask))
            {
                GameObject animal = hit.collider.gameObject;
                OnAnimalClick?.Invoke(animal);
            }
        }
    }
}
