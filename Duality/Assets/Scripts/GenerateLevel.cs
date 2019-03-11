using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateLevel : MonoBehaviour {
	public TextAsset level;
	public GameObject wallPrefab;

	public GameObject platformPrefab;

	public GameObject glassPrefab;

	public GameObject backgroundAsset1;

	public GameObject backgroundAsset2;

	public GameObject backgroundAsset3;

	public Vector3 levelCenter;

	string[] levelRows;

	
	// Use this for initialization
	void Awake () {
		
		
		levelRows = level.text.Split(new char[]{'\n'});
		Bounds wallBounds = wallPrefab.transform.GetChild(0).GetComponent<MeshFilter> ().sharedMesh.bounds;
		float wallHeightInMeters = wallBounds.extents.y * 2f * wallPrefab.transform.localScale.y;
		float wallLengthInMeters = wallBounds.extents.x * 2f * wallPrefab.transform.localScale.x;

		Bounds platformBounds = platformPrefab.transform.GetChild(0).GetComponent<MeshFilter> ().sharedMesh.bounds;
		float platformWidthInMeters = platformBounds.extents.z * 2f * platformPrefab.transform.localScale.z;
		
		// Bounds floorBounds = floorPrefab.GetComponent<MeshFilter> ().sharedMesh.bounds;
		// float floorLengthInMeters = wallLengthInMeters * levelRows[0].Trim().Length;
		// float floorWidthInMeters = wallWidthInMeters * (levelRows.Length - 1);

		// GameObject floor = Instantiate (floorPrefab);

		// floor.transform.localScale = new Vector3 (floorLengthInMeters / (floorBounds.extents.x * 2f), 1f, floorWidthInMeters / (floorBounds.extents.z * 2f));

		// floor.transform.position = levelCenter;
		GameObject backgroundParent = new GameObject("Background Parent");
		float backgroundOffset = platformWidthInMeters / 2;

		Vector3 topLeftCorner = new Vector3 (levelCenter.x - (levelRows[0].Length * wallLengthInMeters / 2f), levelCenter.y + wallHeightInMeters * levelRows.Length / 2, levelCenter.z);
	
		for (int i = 0; i < levelRows.Length - 1; ++i) {
			levelRows [i] = levelRows [i].Trim ();
			


			for(int j = 0; j < levelRows[i].Length; ++j) {

				Vector3 placePosition = new Vector3 (topLeftCorner.x + j * wallLengthInMeters, 
					                        topLeftCorner.y - i * wallHeightInMeters, topLeftCorner.z);

				
				GameObject instance = null;
				switch (levelRows [i] [j]) {
				case 'P':
					instance = Instantiate (platformPrefab);
					PlaceBackgroundWall(placePosition, platformWidthInMeters, backgroundParent.transform);
					break;
				case 'G':
					instance = Instantiate (glassPrefab);
					
					PlaceBackgroundWall(placePosition, platformWidthInMeters, backgroundParent.transform);
					break;
				case '.':
					PlaceBackgroundWall(placePosition, platformWidthInMeters, backgroundParent.transform);
					
					break;
				case 'J':
					instance = Instantiate (backgroundAsset1);
					placePosition.z -= platformWidthInMeters / 2f;
					break;
				case 'S':
					instance = Instantiate (backgroundAsset2);
					placePosition.z -= platformWidthInMeters / 2f;
					break;
				case 'L':
					instance = Instantiate (backgroundAsset3);
					placePosition.z -= platformWidthInMeters / 2f;
					
					break;
				
				
				}
				if (instance != null) {
					instance.transform.position = placePosition;
				}


			}
		}
        

	}
	
	// Update is called once per frame
	void Update () {
		
	}


	void PlaceBackgroundWall(Vector3 position, float platformWidth, Transform parentTransform){
		GameObject backgroundWall = Instantiate(wallPrefab);
		backgroundWall.transform.parent = parentTransform;
		Vector3 wallPosition = position;
		position.z += platformWidth / 2f;
		backgroundWall.transform.position = wallPosition;
	}
}
