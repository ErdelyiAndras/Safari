using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    public Action<Vector3Int> OnMouseClick, OnMouseHold;
    public Action OnMouseUp;
	private Vector2 cameraMovementVector;
	private ScrollRect scrollRect;

    [SerializeField]
	Camera mainCamera;

	public LayerMask groundMask;

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
        InvertScrollDirection();
        CheckArrowInput();
    }

	private Vector3Int? RaycastGround()
	{
		RaycastHit hit;
		Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
		if(Physics.Raycast(ray, out hit, Mathf.Infinity, groundMask))
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
            cameraMovementVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
		}
	}

	private void InvertScrollDirection()
	{
		float scrollInput = Input.GetAxis("Mouse ScrollWheel");
		if (scrollInput != 0)
		{
			scrollRect.horizontalNormalizedPosition += scrollInput * -1f;
        }
    }

	private void OnClickHold()
	{
		if(Input.GetMouseButton(0) && EventSystem.current.IsPointerOverGameObject() == false)
		{
			var position = RaycastGround();
			if (position != null)
				OnMouseHold?.Invoke(position.Value);

		}
	}

	private void OnClickUp()
	{
		if (Input.GetMouseButtonUp(0) && EventSystem.current.IsPointerOverGameObject() == false)
		{
			OnMouseUp?.Invoke();

		}
	}

	private void OnClickDown()
	{
		if (Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject() == false)
		{
			var position = RaycastGround();
			Debug.Log(position);
			if (position != null)
				OnMouseClick?.Invoke(position.Value);

		}
	}
}
