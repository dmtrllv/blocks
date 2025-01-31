using Unity.VisualScripting;
using UnityEngine;

public class WallButtonController : MonoBehaviour
{
	[SerializeField]
	Transform target;

	TriggerController tc;

    public void Start() {
		if(target)
			tc = target.GetComponent<TriggerController>();
	}

	private void OnTriggerEnter(Collider c)
    {
        Debug.Log("colided with " + c.gameObject.name);
		if(c.gameObject.name == "Player") {
			tc?.Trigger();
		}
    }
}
