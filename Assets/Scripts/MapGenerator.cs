using UnityEngine;

public class MapGenerator : MonoBehaviour
{
	[SerializeField]
	private GameObject platform;

	[SerializeField]
	private uint width = 20;

	[SerializeField]
	private uint height = 20;

	void Start()
	{
		int x = Mathf.FloorToInt(width / 2) - (int)width;
		int y = Mathf.FloorToInt(height / 2) - (int)height;
		for (int i = 0; i < width; i++) {
			for (int j = 0; j < height; j++) {
				Instantiate(platform, new Vector3(x + i, 0, y + j), new Quaternion(), transform);
			}
		}
	}

	void Update()
	{
		
	}
}
