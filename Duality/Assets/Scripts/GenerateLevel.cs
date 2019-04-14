using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateLevel : MonoBehaviour {
	public TextAsset level;
	public GameObject wallPrefab;

	public GameObject platformPrefab;

	public GameObject glassPrefab;

	public GameObject foregroundWallPrefab;

	public GameObject backgroundAsset1;

	public GameObject backgroundAsset2;

	public GameObject backgroundAsset3;

	public GameObject rightStairPrefab;

	public GameObject leftStairPrefab;

	public Vector3 levelCenter;

	string[] levelRows;

	
	// Use this for initialization
	public void Generate () {
		
		
		levelRows = level.text.Split(new char[]{'\n'});
		Bounds wallBounds = wallPrefab.transform.GetChild(0).GetComponent<MeshFilter> ().sharedMesh.bounds;
		float wallHeightInMeters = wallBounds.extents.y * 2f * wallPrefab.transform.localScale.y;
		float wallLengthInMeters = wallBounds.extents.x * 2f * wallPrefab.transform.localScale.x;
		
		
		Bounds platformBounds = platformPrefab.transform.GetChild(0).GetComponent<MeshFilter> ().sharedMesh.bounds;
		float platformWidthInMeters = platformBounds.extents.z * 2f * platformPrefab.transform.localScale.z;
		
		GameObject levelParent = new GameObject("Level Parent", typeof(CompositeCollider2D));
		levelParent.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
		levelParent.layer = 11;
		levelParent.transform.position = Vector3.zero;
		levelParent.tag = "Level";
		
		GameObject backgroundParent = new GameObject("Background Parent");
		backgroundParent.transform.parent = levelParent.transform;
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
					instance = Instantiate (platformPrefab, levelParent.transform);
					PlaceBackgroundObject(placePosition, platformWidthInMeters, backgroundParent.transform, wallPrefab);
					break;
				case 'R':
					instance = Instantiate(rightStairPrefab, levelParent.transform);
					PlaceBackgroundObject(placePosition, platformWidthInMeters, backgroundParent.transform, wallPrefab);
					break;
				case 'L':
					instance = Instantiate(leftStairPrefab, levelParent.transform);
					PlaceBackgroundObject(placePosition, platformWidthInMeters, backgroundParent.transform, wallPrefab);
					break;
				case 'G':
					instance = Instantiate (glassPrefab, levelParent.transform);
					
					PlaceBackgroundObject(placePosition, platformWidthInMeters, backgroundParent.transform, wallPrefab);
					break;
				case 'W':
					instance = Instantiate(foregroundWallPrefab, levelParent.transform);
					PlaceBackgroundObject(placePosition, platformWidthInMeters, backgroundParent.transform, wallPrefab);
					break;
				case '.':
					PlaceBackgroundObject(placePosition, platformWidthInMeters, backgroundParent.transform, wallPrefab);
					
					break;
				case 'J':
					instance = Instantiate (backgroundAsset1, levelParent.transform);
					//placePosition.z += platformWidthInMeters / 2f;
					break;
				case 'S':
					instance = Instantiate (backgroundAsset2, levelParent.transform);
					//placePosition.z += platformWidthInMeters / 2f;
					break;
				case 'K':
					instance = Instantiate (backgroundAsset3, levelParent.transform);
					//placePosition.z += platformWidthInMeters / 2f;
					
					break;
				
				case 'B':
					instance = Instantiate (platformPrefab, levelParent.transform);
					PlaceBackgroundObject(placePosition, platformWidthInMeters, backgroundParent.transform, backgroundAsset1);
					break;
				case 'D':
					PlaceBackgroundObject(placePosition, platformWidthInMeters, backgroundParent.transform, wallPrefab);
					GameObject forwardWall = PlaceBackgroundObject(placePosition, platformWidthInMeters, backgroundParent.transform, wallPrefab);
					Vector3 frontWall = forwardWall.transform.localPosition;
					frontWall.z -= platformWidthInMeters;
					forwardWall.transform.localPosition = frontWall;
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


	GameObject PlaceBackgroundObject(Vector3 position, float platformWidth, Transform parentTransform, GameObject backgroundAssetPrefab){
		GameObject backgroundWall = Instantiate(backgroundAssetPrefab);
		backgroundWall.transform.parent = parentTransform;
		Vector3 wallPosition = position;
		//position.z += platformWidth / 2f;
		backgroundWall.transform.position = wallPosition;
		return backgroundWall;
	}
}
