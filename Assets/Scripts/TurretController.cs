using UnityEngine;

public class TurretController : MonoBehaviour
{
	LineRenderer lr;

	[SerializeField]
	Transform emptyTarget;

	void Start()
	{
		lr = GetComponent<LineRenderer>();
	}

	void Update()
	{
		if (Physics.Raycast(transform.position, transform.right, out RaycastHit hitInfo))
		{
			lr.SetPosition(0, transform.position);
			lr.SetPosition(1, hitInfo.point);
		}
		else if (emptyTarget != null)
		{
			lr.SetPosition(0, transform.position);
			lr.SetPosition(1, emptyTarget.position);
		}
	}
}
