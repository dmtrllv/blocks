using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{

	enum RotationState
	{
		Left,
		Right,
		None
	}

	public enum LookDirection
	{
		North, // 0 deg
		East, // 90 deg
		South, // 180 deg
		West, // 270 deg

	}

	[SerializeField]
	float rotationSpeed = 10f;

	[SerializeField]
	float zoomSpeed = 10f;

	[SerializeField]
	float zoomStep = 1f;

	[SerializeField]
	Transform player;

	[SerializeField]
	float followSpeed = 1f;

	RotationState state = RotationState.None;
	Quaternion targetRotation = new Quaternion();

	public LookDirection lookDirection
	{
		get
		{
			switch (Mathf.FloorToInt(targetRotation.eulerAngles.y % 360))
			{
				case 0: return LookDirection.North;
				case 90: return LookDirection.East;
				case 180: return LookDirection.South;
				case 270: return LookDirection.West;
			}
			throw new Exception("Invalid LookDirection rotation!");
		}
	}

	float zoomTarget = 1.0f;

	void Start()
	{
		targetRotation = transform.rotation;
		if (player == null)
		{
			player = GameObject.Find("player")?.transform;
			if (player == null)
			{
				Debug.LogError("Could not get player!");
			}
		}
	}

	void Update()
	{
		UpdateState();

		transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

		if (Input.mouseScrollDelta.y != 0)
		{
			zoomTarget = Mathf.Clamp(transform.localScale.x - (Input.mouseScrollDelta.y / 100.0f * transform.localScale.x * zoomStep), 0.5f, 2.5f);
		}

		transform.localScale = Vector3.Slerp(transform.localScale, new Vector3(zoomTarget, zoomTarget, zoomTarget), Time.deltaTime * zoomSpeed);
		transform.position = Vector3.Slerp(transform.position, new Vector3(player.position.x, player.position.y + 1, player.position.z), Time.deltaTime * followSpeed);
	}

	void UpdateState()
	{
		if (state != RotationState.None)
		{
			var a = transform.rotation.eulerAngles.y % 90;
			var b = 90 - a;
			if (a < 1 || b < 1)
			{
				state = RotationState.None;
			}
			else
			{
				return;
			}
		}

		var q = Input.GetKey(KeyCode.Q);
		var e = Input.GetKey(KeyCode.E);

		if (q && !e)
		{
			state = RotationState.Left;
			targetRotation *= Quaternion.Euler(0, 90f, 0);
		}
		else if (!q && e)
		{
			state = RotationState.Right;
			targetRotation *= Quaternion.Euler(0, -90f, 0);
		}
		else
		{
			state = RotationState.None;
		}
	}
}
