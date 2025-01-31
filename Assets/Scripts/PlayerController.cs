using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using LookDir = CameraController.LookDirection;

public class PlayerController : MonoBehaviour
{
	enum RotationState
	{
		Left,
		Right,
		Forward,
		Backward,
		None
	}

	[SerializeField]
	List<Transform> corners = new List<Transform>();

	[SerializeField]
	float rotateSpeed = 50.0f;

	[SerializeField]
	LayerMask groundLayer;

	List<Transform> GetGrounded()
	{
		List<Transform> list = new List<Transform>();
		foreach (var c in corners)
		{
			var col = Physics.OverlapSphere(c.position, 0.1f, groundLayer);
			if (col.Length > 0)
			{
				list.Add(c);
			}
		}
		return list;
	}

	[SerializeField]
	CameraController cameraController;

	RotationState state = RotationState.None;
	Vector3 rotatePoint = Vector3.zero;
	Vector3 rotateAxis = Vector3.zero;
	float rotated = 0.0f;

	void Update()
	{
		if (state != RotationState.None)
		{
			return;
		}

		var l = Input.GetKey(KeyCode.A);
		var r = Input.GetKey(KeyCode.D);
		var f = Input.GetKey(KeyCode.W);
		var b = Input.GetKey(KeyCode.S);

		if (new[] { l, r, f, b }.Count(b => !!b) == 1)
		{
			if (l)
			{
				rotatePoint = GetLeft();
				rotateAxis = GetLeftAxis();
				state = RotationState.Left;
			}
			else if (r)
			{
				rotatePoint = GetRight();
				rotateAxis = GetRightAxis();
				state = RotationState.Right;
			}
			else if (f)
			{
				rotatePoint = GetForward();
				rotateAxis = GetForwardAxis();
				state = RotationState.Forward;
			}
			else if (b)
			{
				rotatePoint = GetBackward();
				rotateAxis = GetBackwardAxis();
				state = RotationState.Backward;
			}
		}
	}

	int round(float a) {
		if (a < 0) {
			return -Mathf.RoundToInt(0 - a);
		} else {
			return Mathf.RoundToInt(a);
		}
	}

	Vector3 roundVec(Vector3 v) {
		var x = round(v.x);
		var y = round(v.y);
		var z = round(v.z);
		return new Vector3(x, y, z);
	}

	void FixedUpdate()
	{
		if (state != RotationState.None)
		{
			var s = Time.fixedDeltaTime * rotateSpeed;
			transform.RotateAround(rotatePoint, rotateAxis, -s);
			rotated += s;
			if (rotated >= 90)
			{
				rotated = 0;
				state = RotationState.None;
				fixTransform();
			}
		}
	}

	private void fixTransform()
	{
		var pos = roundVec(transform.position * 2) / 2;
		var rot = roundVec(transform.rotation.eulerAngles / 90) * 90;
		transform.position = pos;
		transform.rotation = Quaternion.Euler(rot);
	}

	private Vector3 GetLeftAxis()
	{
		switch (cameraController.lookDirection)
		{
			case LookDir.North: return Vector3.back;
			case LookDir.East: return Vector3.left;
			case LookDir.South: return Vector3.forward;
			case LookDir.West: return Vector3.right;
		}
		throw new Exception("Unknown..");
	}

	private Vector3 GetRightAxis()
	{
		switch (cameraController.lookDirection)
		{
			case LookDir.North: return Vector3.forward;
			case LookDir.East: return Vector3.right;
			case LookDir.South: return Vector3.back;
			case LookDir.West: return Vector3.left;
		}
		throw new Exception("Unknown..");
	}

	private Vector3 GetForwardAxis()
	{
		switch (cameraController.lookDirection)
		{
			case LookDir.North: return Vector3.left;
			case LookDir.East: return Vector3.forward;
			case LookDir.South: return Vector3.right;
			case LookDir.West: return Vector3.back;
		}
		throw new Exception("Unknown..");
	}

	private Vector3 GetBackwardAxis()
	{
		switch (cameraController.lookDirection)
		{
			case LookDir.North: return Vector3.right;
			case LookDir.East: return Vector3.back;
			case LookDir.South: return Vector3.left;
			case LookDir.West: return Vector3.forward;
		}
		throw new Exception("Unknown..");
	}

	private Vector3 GetLeft()
	{
		var grounded = GetGrounded();
		if (grounded.Count != 2)
		{
			throw new Exception("Cannot get Left Vector!");
		}

		var a = grounded[0];
		var b = grounded[1];

		switch (cameraController.lookDirection)
		{
			case LookDir.North: return smallerX(a, b);
			case LookDir.East: return biggerZ(a, b);
			case LookDir.South: return biggerX(a, b);
			case LookDir.West: return smallerZ(a, b);
		}
		throw new Exception("Unknown..");
	}

	private Vector3 GetRight()
	{
		var grounded = GetGrounded();
		if (grounded.Count != 2)
		{
			throw new Exception("Cannot get Right Vector!");
		}

		var a = grounded[0];
		var b = grounded[1];

		switch (cameraController.lookDirection)
		{
			case LookDir.North: return biggerX(a, b);
			case LookDir.East: return smallerZ(a, b);
			case LookDir.South: return smallerX(a, b);
			case LookDir.West: return biggerZ(a, b);
		}
		throw new Exception("Unknown..");
	}

	private Vector3 GetForward()
	{
		var grounded = GetGrounded();
		if (grounded.Count != 2)
		{
			throw new Exception("Cannot get Forward Vector!");
		}

		var a = grounded[0];
		var b = grounded[1];

		switch (cameraController.lookDirection)
		{
			case LookDir.North: return biggerZ(a, b);
			case LookDir.East: return biggerX(a, b);
			case LookDir.South: return smallerZ(a, b);
			case LookDir.West: return smallerX(a, b);
		}
		throw new Exception("Unknown..");
	}

	private Vector3 GetBackward()
	{
		var grounded = GetGrounded();
		if (grounded.Count != 2)
		{
			throw new Exception("Cannot get Backward Vector!");
		}

		var a = grounded[0];
		var b = grounded[1];

		switch (cameraController.lookDirection)
		{
			case LookDir.North: return smallerZ(a, b);
			case LookDir.East: return smallerX(a, b);
			case LookDir.South: return biggerZ(a, b);
			case LookDir.West: return biggerX(a, b);
		}
		throw new Exception("Unknown..");
	}

	private Vector3 biggerX(Transform a, Transform b)
	{
		if (a.position.x > b.position.x)
		{
			return a.position;
		}
		else
		{
			return b.position;
		}
	}

	private Vector3 biggerZ(Transform a, Transform b)
	{
		if (a.position.z > b.position.z)
		{
			return a.position;
		}
		else
		{
			return b.position;
		}
	}

	private Vector3 smallerX(Transform a, Transform b)
	{
		if (a.position.x < b.position.x)
		{
			return a.position;
		}
		else
		{
			return b.position;
		}
	}

	private Vector3 smallerZ(Transform a, Transform b)
	{
		if (a.position.z < b.position.z)
		{
			return a.position;
		}
		else
		{
			return b.position;
		}
	}
}
