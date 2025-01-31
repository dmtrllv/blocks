using UnityEngine;

public class WallSlideController : TriggerController
{

	public enum SlideState
	{
		Up,
		Down,
	}

	[SerializeField]
	SlideState state = SlideState.Down;

	[SerializeField]
	float slideSpeed = 3.0f;

	[SerializeField]
	float slideOffset = 2.0f;

	[SerializeField]
	Vector3 target;

	void Start()
	{
		target = transform.position;
	}

	void Update()
	{
		transform.position = Vector3.Slerp(transform.position, target, slideSpeed * Time.deltaTime);
	}

	public override void Trigger()
	{
		if (state == SlideState.Down)
		{
			state = SlideState.Up;
			target.y += slideOffset;
		}
		else
		{
			state = SlideState.Down;
			target.y -= slideOffset;
		}
	}
}
