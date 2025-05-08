using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TestTools;
using UnityEngine.UI;

public class InputManagerTests
{
    private class MockInputProvider : IInputProvider
    {
        public Vector3 mousePosition = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        public float horizontal = 1.0f;
        public float vertical = 2.0f;

        public Vector3 MousePosition => mousePosition;

        public float GetAxis(string axisName)
        {
            if (axisName == "Horizontal")
            {
                return horizontal;
            }
            else if (axisName == "Vertical")
            {
                return vertical;
            }
            throw new System.NotImplementedException($"Axis '{axisName}' is not implemented.");
        }
        
        public bool GetKeyDown(KeyCode key)
        {
            if (key == KeyCode.Escape)
            {
                return true;
            }
            return false;
        }

        public bool GetMouseButtonDown(int button)
        {
            if (button == 0)
            {
                return true;
            }
            return false;
        }
        public bool GetMouseButtonUp(int button)
        {
            if (button == 0)
            {
                return true;
            }
            return false;
        }
        public bool GetMouseButton(int button)
        {
            if (button == 0)
            {
                return true;
            }
            return false;
        }
    }

    private InputManager inputManager;
    private MockInputProvider mockInputProvider;
    private GameObject eventSystemObject;

    [UnitySetUp]
    public IEnumerator Setup()
    {
        GameObject go = new GameObject();
        inputManager = go.AddComponent<InputManager>();

        mockInputProvider = new MockInputProvider();
        inputManager.InputProvider = mockInputProvider;

        GameObject cameraObject = new GameObject("MainCamera");
        Camera camera = cameraObject.AddComponent<Camera>();
        camera.transform.position = new Vector3(0, 10, -10);
        camera.transform.LookAt(Vector3.zero);

        GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        plane.layer = LayerMask.NameToLayer("Plane");
        plane.transform.position = Vector3.zero;

        inputManager.mainCamera = camera;
        inputManager.groundMask = LayerMask.GetMask("Plane");
        inputManager.animalMask = LayerMask.GetMask("Animal");

        eventSystemObject = new GameObject("EventSystem");
        eventSystemObject.AddComponent<EventSystem>();
        eventSystemObject.AddComponent<StandaloneInputModule>();
        yield return null;
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        Object.Destroy(inputManager);
        Object.Destroy(eventSystemObject);
        yield return null;
    }

    [UnityTest]
    public IEnumerator InputManagerOnMouseClickValidClickTest()
    {
        bool eventInvoked = false;
        Vector3? mousePosition = null;
        inputManager.OnMouseClick += pos => { eventInvoked = true; mousePosition = pos; };
        
        yield return null;

        Assert.IsTrue(eventInvoked);
        Assert.AreEqual(Vector3.zero, mousePosition);
    }
    
    [UnityTest]
    public IEnumerator InputManagerOnMouseClickNotValidClickTest()
    {
        bool eventInvoked = false;
        Vector3? mousePosition = null;
        mockInputProvider.mousePosition = Vector3.zero;
        inputManager.OnMouseClick += pos => { eventInvoked = true; mousePosition = pos; };
        
        yield return null;

        Assert.IsFalse(eventInvoked);
        Assert.AreEqual(null, mousePosition);
    }
    
    [UnityTest]
    public IEnumerator InputManagerOnMouseHoldTest()
    {
        bool eventInvoked = false;
        Vector3? mousePosition = null;
        inputManager.OnMouseHold += pos => { eventInvoked = true; mousePosition = pos; };

        yield return null;

        Assert.IsTrue(eventInvoked);
        Assert.AreEqual(Vector3.zero, mousePosition);
        Debug.Log(mousePosition);
    }

    [UnityTest]
    public IEnumerator InputManagerOnMouseUpTest()
    {
        bool eventInvoked = false;
        inputManager.OnMouseUp += () => { eventInvoked = true; };

        yield return null;

        Assert.IsTrue(eventInvoked);
    }

    [UnityTest]
    public IEnumerator InputManagerCameraMovementTest()
    {
        Assert.AreEqual(new Vector2(1.0f, 2.0f), inputManager.CameraMovementVector);
        yield return null;
    }

    [UnityTest]
    public IEnumerator InputManagerCameraNoMovementTest()
    {
        mockInputProvider.horizontal = 0.0f;
        mockInputProvider.vertical = 0.0f;
        yield return null;

        Assert.AreEqual(Vector2.zero, inputManager.CameraMovementVector);
        yield return null;
    }

    [UnityTest]
    public IEnumerator InputManagerInputFieldActiveTest()
    {
        GameObject canvasObject = new GameObject("Canvas");
        Canvas canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        GameObject inputFieldObject = new GameObject("InputField");
        inputFieldObject.transform.SetParent(canvasObject.transform);
        InputField inputField = inputFieldObject.AddComponent<InputField>();

        EventSystem.current.SetSelectedGameObject(inputFieldObject);

        yield return null;

        Assert.AreEqual(Vector2.zero, inputManager.CameraMovementVector);
    }

    [UnityTest]
    public IEnumerator InputManagerOnAnimalClickTest()
    {
        GameObject animal = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        animal.layer = LayerMask.NameToLayer("Animal");
        animal.name = "animal";
        animal.transform.position = Vector3.zero;

        bool eventInvoked = false;
        GameObject clickedAnimal = null;
        inputManager.OnAnimalClick += a => { eventInvoked = true; clickedAnimal = a; };

        yield return null;

        Assert.IsTrue(eventInvoked);
        Assert.AreEqual("animal", clickedAnimal.name);
    }
}