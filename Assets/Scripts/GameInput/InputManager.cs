using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


[System.Serializable]
public class MyTouch
{
	public Vector2 			position = Vector2.zero;
	public TouchPhase 		phase ;
	public int 				fingerId ;
	public int 				tapCount = 1;
	public TouchType		type = TouchType.Direct;
	public float 			pressure = 1f;
	public float 			initTime = 0f;
	public float 			tapDuration = 0f;
	public Vector2 			initPos = Vector2.zero;
	public float 			travelled = 0f;
	public GameObject 		draggedGameObject ;
	public IInteractable 	interactableInterface ;
	public Vector3			initWorldPos = Vector3.zero;

	public MyTouch(int touchID)
	{
		phase = TouchPhase.Canceled;
		fingerId = touchID;
		draggedGameObject = null;
		interactableInterface = null;
	}
}

public class InputManager : MonoBehaviour
{
	public Camera inputCamera ;
	
	public static MyTouch[] 	touches;
	public static int 			touchCount = 0;
	
	#if UNITY_EDITOR
	public MyTouch[] 	debugTouches ;
	public KeyCode 		pauseKey = KeyCode.Space ;
	#endif
	
	public static int 			SupportedTouches
	{
		get {
			return supportedTouches;
		}
		set {
			supportedTouches = value;
			touches = new MyTouch[supportedTouches];
			
			inputTouchesToMyTouchesMap = new int[supportedTouches];
			
			mouseClickCounts = new int[supportedTouches];
			mouseClicksLastTimes = new float[supportedTouches];
			mouseClicksLastInitPositions = new Vector2[supportedTouches];
			
			for (int i = 0; i < supportedTouches; i++)
			{
				inputTouchesToMyTouchesMap[i] = -1;
				touches[i] = nullTouch;
				mouseClickCounts[i] = 1;
				mouseClicksLastTimes[i] = Mathf.NegativeInfinity;
				mouseClicksLastInitPositions[i] = -Vector2.one;
			}
		}
	}
	
	public static InputManager 	Instance;
	
	private static int[] 		inputTouchesToMyTouchesMap;
	private static MyTouch 		nullTouch = new MyTouch(-999);
	private static int 			supportedTouches = 1;
	private int 				prevTouchCount = 0;
	private bool 				mousePresent = false;
	
	private static int[] 		mouseClickCounts ;
	private static float[] 		mouseClicksLastTimes ;
	private static Vector2[] 	mouseClicksLastInitPositions ;
	
	public static GraphicRaycaster 	uiRaycaster ;

	void Awake ()
	{
		Instance = this;
		
		SupportedTouches = 2;
		
		#if UNITY_EDITOR
		debugTouches = touches;
		#endif
	}
	
	
	void Start ()
	{
		mousePresent = Input.mousePresent;
		
		if (inputCamera == null) {
			inputCamera = Camera.main;
		}
		
		#if UNITY_EDITOR
		mousePresent = true;
		#endif
	}
	
	
	// Update is called once per frame
	void Update ()
	{
		touchCount = Input.touchCount;
		
		if (mousePresent && touchCount == 0)
		{
			for (int i = 0; i < supportedTouches; i++)
			{
				if (Input.GetMouseButtonDown(i) || Input.GetMouseButton(i) || Input.GetMouseButtonUp(i)) {
					touchCount++;
				}
			}
		}
		
		if (prevTouchCount != touchCount) {
			UpdateMap();
			DestroyUnmappedTouches();
		}
		
		prevTouchCount = touchCount;
		
		if (Input.touchCount > 0)
		{
			for (int i = 0; i < Mathf.Min(Input.touchCount, supportedTouches); i++)
			{
				if (Input.GetTouch(i).phase == TouchPhase.Began)
				{
					int indexOfFirstNullTouch = IndexOf<MyTouch>(touches, nullTouch);
					
					if (indexOfFirstNullTouch > -1)
					{
						touches[indexOfFirstNullTouch] = new MyTouch(Input.GetTouch(i).fingerId);
						touches[indexOfFirstNullTouch].tapCount = Input.GetTouch(i).tapCount;
						touches[indexOfFirstNullTouch].type = Input.GetTouch(i).type;
						touches[indexOfFirstNullTouch].pressure = Input.GetTouch(i).pressure;
						touches[indexOfFirstNullTouch].initTime = Time.time;
						touches[indexOfFirstNullTouch].initPos = Input.GetTouch(i).position;
						
						UpdateMap();
					}
				}
				
				if (inputTouchesToMyTouchesMap[i] != -1)
				{
					touches[inputTouchesToMyTouchesMap[i]].position = Input.GetTouch(i).position;
					touches[inputTouchesToMyTouchesMap[i]].phase = Input.GetTouch(i).phase;
					touches[inputTouchesToMyTouchesMap[i]].pressure = Input.GetTouch(i).pressure;
					touches[inputTouchesToMyTouchesMap[i]].tapDuration = Time.time - touches[inputTouchesToMyTouchesMap[i]].initTime;
					
					float dist = (Input.GetTouch(i).position - touches[inputTouchesToMyTouchesMap[i]].initPos).magnitude;
					
					if (dist > touches[inputTouchesToMyTouchesMap[i]].travelled) {
						touches[inputTouchesToMyTouchesMap[i]].travelled = dist;
					}
					
					if (touches[inputTouchesToMyTouchesMap[i]].phase == TouchPhase.Began)
					{
						if (GetGraphicGOContainingScreenPos( touches[inputTouchesToMyTouchesMap[i]].position ) == null)
						{
							Ray ray = inputCamera.ScreenPointToRay(touches[inputTouchesToMyTouchesMap[i]].position);
							RaycastHit hit;
							
							if (Physics.Raycast(ray, out hit))
							{
								if (touches[inputTouchesToMyTouchesMap[i]].interactableInterface != null
//									&& touches[inputTouchesToMyTouchesMap[i]].interactableInterface.HasActiveInteraction()
								)
								{
									touches[inputTouchesToMyTouchesMap[i]].interactableInterface.OnPointerUp(touches[inputTouchesToMyTouchesMap[i]]);
								}
								
								GameObject hitGO = hit.collider.gameObject;
								touches[inputTouchesToMyTouchesMap[i]].interactableInterface = hitGO.GetComponent<IInteractable>();
															
								if (touches[inputTouchesToMyTouchesMap[i]].interactableInterface != null)
								{
									touches[inputTouchesToMyTouchesMap[i]].draggedGameObject = hitGO;
									touches[inputTouchesToMyTouchesMap[i]].initWorldPos = hit.point;
									touches[inputTouchesToMyTouchesMap[i]].interactableInterface.OnPointerDown(touches[inputTouchesToMyTouchesMap[i]]);
								}
							}
						}
					}
					
					if (touches[inputTouchesToMyTouchesMap[i]].phase == TouchPhase.Moved || touches[inputTouchesToMyTouchesMap[i]].phase == TouchPhase.Stationary)
					{
						if (touches[inputTouchesToMyTouchesMap[i]].interactableInterface != null) {
							touches[inputTouchesToMyTouchesMap[i]].interactableInterface.OnPointerDrag(touches[inputTouchesToMyTouchesMap[i]]);
						}
					}
					
					if (touches[inputTouchesToMyTouchesMap[i]].phase == TouchPhase.Ended)
					{
						if (touches[inputTouchesToMyTouchesMap[i]].interactableInterface != null)
						{
							touches[inputTouchesToMyTouchesMap[i]].interactableInterface.OnPointerUp(touches[inputTouchesToMyTouchesMap[i]]);
							
							Ray ray = inputCamera.ScreenPointToRay(touches[inputTouchesToMyTouchesMap[i]].position);
							RaycastHit hit;
							Collider coll = touches[inputTouchesToMyTouchesMap[i]].draggedGameObject.GetComponent<Collider>();
							
							if (coll.Raycast(ray, out hit, Mathf.Infinity)) {
								touches[inputTouchesToMyTouchesMap[i]].interactableInterface.OnPointerUpAsButton(touches[inputTouchesToMyTouchesMap[i]]);
							}
						}
						
						touches[inputTouchesToMyTouchesMap[i]].draggedGameObject = null;
						touches[inputTouchesToMyTouchesMap[i]].interactableInterface = null;
					}
				}
			}
		}
		
		if (mousePresent && Input.touchCount == 0)
		{
			for (int i = 0; i < supportedTouches; i++)
			{
				//This order is intentional because Input.GetMouseButton and Input.GetMouseButtonDown are fired in the same frame
				
				
				if (Input.GetMouseButton(i) && touches[i].fingerId == -(i+1)*1000 )
				{
					if (Input.GetAxis("Mouse X") == 0f && Input.GetAxis("Mouse Y") == 0f)
					{
						touches[i].phase = TouchPhase.Stationary;
					}
					else
					{
						touches[i].phase = TouchPhase.Moved;
						touches[i].position = Input.mousePosition;
						
						float dist = (touches[i].position - touches[i].initPos).magnitude;
						
						if (dist > touches[i].travelled) {
							touches[i].travelled = dist;
						}
					}
					
					touches[i].tapDuration = Time.time - touches[i].initTime;
					
					if (touches[i].interactableInterface != null) {
						touches[i].interactableInterface.OnPointerDrag(touches[i]);
					}
				}
				
				
				if (Input.GetMouseButtonDown(i))
				{
					touches[i] = new MyTouch(-(i+1)*1000);
					touches[i].phase = TouchPhase.Began;
					touches[i].position = Input.mousePosition;
					touches[i].initPos = Input.mousePosition;
					touches[i].initTime = Time.time;
					
					if (Time.time - mouseClicksLastTimes[i] < 0.33 && ((Vector2)Input.mousePosition - mouseClicksLastInitPositions[i]).sqrMagnitude < 4f) {
						mouseClickCounts[i]++;
					}
					else mouseClickCounts[i] = 1;
					
					touches[i].tapCount = mouseClickCounts[i];
					
					mouseClicksLastInitPositions[i] = Input.mousePosition;
					mouseClicksLastTimes[i] = Time.time;
					
					
					if (GetGraphicGOContainingScreenPos(touches[i].position) == null)
					{
						Ray ray = inputCamera.ScreenPointToRay(touches[i].position);
				        RaycastHit hit;
				
				        if (Physics.Raycast(ray, out hit))
						{
							GameObject hitGO = hit.collider.gameObject;
							touches[i].interactableInterface = hitGO.GetComponent<IInteractable>();
							
							if (touches[i].interactableInterface != null)
							{
								touches[i].draggedGameObject = hitGO;
								touches[i].initWorldPos = hit.point;
								touches[i].interactableInterface.OnPointerDown(touches[i]);
							}
						}
					}
				}
				
				if (Input.GetMouseButtonUp(i) && touches[i].fingerId == -(i+1)*1000 )
				{
					touches[i].phase = TouchPhase.Ended;
					touches[i].position = Input.mousePosition;
					
					if (touches[i].interactableInterface != null)
					{
						touches[i].interactableInterface.OnPointerUp(touches[i]);
						
						Ray ray = inputCamera.ScreenPointToRay(touches[i].position);
						RaycastHit hit;
						Collider coll = touches[i].draggedGameObject.GetComponent<Collider>();
						
						if (coll.Raycast(ray, out hit, Mathf.Infinity)) {
							touches[i].interactableInterface.OnPointerUpAsButton(touches[i]);
						}
					}
					
					touches[i].draggedGameObject = null;
					touches[i].interactableInterface = null;
				}
			}
		}

		#if UNITY_EDITOR
		if (Input.GetKeyDown(pauseKey))  {
			Debug.Break();
		}
		#endif
	}
	
	
	void UpdateMap ()
	{
		for (int i = 0; i < inputTouchesToMyTouchesMap.Length; i++) {
			inputTouchesToMyTouchesMap[i] = -1;
		}
		
		for (int i = 0; i < Mathf.Min(Input.touchCount, supportedTouches); i++)
		{
			inputTouchesToMyTouchesMap[i] = -1;
			
			for (int j = 0; j < inputTouchesToMyTouchesMap.Length; j++)
			{
				if (touches[j].fingerId == Input.GetTouch(i).fingerId) {
					inputTouchesToMyTouchesMap[i] = j;
				}
			}
		}
	}
	
	
	void DestroyUnmappedTouches ()
	{
		for (int i = 0; i < touches.Length; i++)
		{
			for (int j = 0; j < touches.Length && touches[i] != nullTouch; j++)
			{
				if (touches[i].fingerId == -(j+1) * 1000 && (!Input.GetMouseButtonDown(j) && !Input.GetMouseButton(j) && !Input.GetMouseButtonUp(j))) {
					touches[i] = nullTouch;
				}
			}
			
			if (touches[i] != nullTouch && IndexOf<int>(inputTouchesToMyTouchesMap, i) == -1)
			{
				bool isNullTouch = true;
				
				for (int j = 0; j < touches.Length; j++)
				{
					if (touches[i].fingerId == -(j+1) * 1000) {
						isNullTouch = false;
					}
				}
				
				if (isNullTouch) {
					touches[i] = nullTouch;
				}
			}
		}
	}
	
	
	public static GameObject GetGraphicGOContainingScreenPos (Vector2 screenPos)
	{
		List<RaycastResult> results = new List<RaycastResult>();
		
		PointerEventData eventData = new PointerEventData(EventSystem.current);
		eventData.position = screenPos;
		
		if (uiRaycaster == null)
		{
			uiRaycaster = (GraphicRaycaster) FindObjectOfType(typeof(GraphicRaycaster));
			
			if (uiRaycaster == null)
			{
//				Debug.LogError("There is no Graphic raycaster in the scene!");
				return null;
			}
		}
		
		uiRaycaster.Raycast(eventData, results);
		
		if (results.Count > 0) {
			return results[0].gameObject;
		}
		
		return null;
	}
	
	int IndexOf<T>(T[] array, T element)
	{
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].Equals(element)) {
				return i;
			}
		}
		
		return -1;
	}
}
