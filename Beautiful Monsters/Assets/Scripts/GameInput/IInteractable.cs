using UnityEngine;
using System.Collections;

public interface IInteractable
{
	void OnPointerDown (MyTouch touch);
	void OnPointerDrag (MyTouch touch);
	void OnPointerUp (MyTouch touch);
	void OnPointerUpAsButton (MyTouch touch);
}
