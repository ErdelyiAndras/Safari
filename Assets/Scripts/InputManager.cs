using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    public Action<Vector3Int> OnMouseClick, OnMouseHold;
    public Action OnMouseUp;
	private Vector2 cameraMovementVector;

	[SerializeField]
	Camera mainCamera;

	public LayerMask groundMask;

	public Vector2 CameraMovementVector
	{
		get { return cameraMovementVector; }
	}

	private void Update()
	{
		OnClickDown();
		OnClickUp();
		OnClickHold();
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
		cameraMovementVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
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
