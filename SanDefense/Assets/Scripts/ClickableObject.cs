using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableObject : MonoBehaviour
{
	Touch touch;
	protected MouseOverState state = MouseOverState.NotOver;

	/// <summary>
	/// Set the state to Over when the mouse moves over the collider
	/// </summary>
	private void OnMouseOver()
	{
		if (state != MouseOverState.Ended)
		{
			state = MouseOverState.Over;
		}
	}

	/// <summary>
	/// Set the state to NotOver when the mouse exits the collider
	/// </summary>
	private void OnMouseExit()
	{
		state = MouseOverState.NotOver;
	}

	/// <summary>
	/// Set the state to Click when the mouse is clicked on top of the collider
	/// </summary>
	private void OnMouseDown()
	{
		OnClick();
	}

	/// <summary>
	/// Set the state to Over when the mouse is unclicked on top of the collider
	/// </summary>
	private void OnMouseUp()
	{
		state = MouseOverState.Ended;
		OnUnclick();
	}

	/// <summary>
	/// Set the state to Drag when the mouse is dragged on top of the collider
	/// </summary>
	private void OnMouseDrag()
	{
		OnDrag(Input.mousePosition);
	}

	/// <summary>
	/// Gets a value indicating whether the mouse is over this <see cref="T:ClickableObject"/>.
	/// </summary>
	/// <value><c>true</c> if is mouse over; otherwise, <c>false</c>.</value>
	public bool IsMouseOver
	{
		get
		{
			return state == MouseOverState.Over;
		}
	}

	private void Update()
	{
		if (Application.isMobilePlatform)
		{
			if (Input.touchCount > 0)
			{
				touch = Input.GetTouch(0);
				Vector3 touchPos = touch.position;
				touchPos = Camera.main.ScreenToWorldPoint(touchPos.SetZ(-Camera.main.transform.position.z));

				RaycastHit[] hits = Physics.RaycastAll(touchPos, Vector3.forward);

				if (touch.phase == TouchPhase.Ended && state != MouseOverState.NotOver)
				{
					state = MouseOverState.Ended;
					OnUnclick();
				}
				foreach (RaycastHit hit in hits)
				{
					if (hit.collider.gameObject == gameObject)
					{
						if (touch.phase == TouchPhase.Began)
						{
							OnClick();
						}
						else if (touch.phase == TouchPhase.Moved)
						{
							OnDrag(touch.position);
						}
						else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
						{
							state = MouseOverState.Ended;
							OnUnclick();
						}

						break;
					}
				}

			}
		}

	}

	private void LateUpdate()
	{
		if (state == MouseOverState.Ended)
		{
			state = MouseOverState.NotOver;
		}
	}

	public virtual void OnDrag(Vector3 vec)
	{

	}

	public virtual void OnClick()
	{

	}

	public virtual void OnUnclick()
	{

	}
}

public enum MouseOverState
{
	NotOver,
	Over,
	Ended
}