using UnityEngine;
using UnityEngine.Events;


[System.Serializable]
public class MyTouchEvent : UnityEvent<MyTouch> {}

public class InputReceiver : MonoBehaviour, IInteractable
{

	public MyTouchEvent onPointerDown ;
	public MyTouchEvent onPointerDrag ;
	public MyTouchEvent onPointerUp ;
	public MyTouchEvent onPointerUpAsButton ;

	public void OnPointerDown (MyTouch touch)
	{
		onPointerDown.Invoke(touch);
	}

	public void OnPointerDrag (MyTouch touch)
	{
		onPointerDrag.Invoke(touch);
	}

	public void OnPointerUp (MyTouch touch)
	{
		onPointerUp.Invoke(touch);
	}

	public void OnPointerUpAsButton (MyTouch touch)
	{
		onPointerUpAsButton.Invoke(touch);
	}
}
