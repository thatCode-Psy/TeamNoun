using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateLevel : MonoBehaviour {
	public TextAsset level;
	public GameObject wallPrefab;

	public GameObject tablePrefab;

	public GameObject floorPrefab;

	public GameObject shirtBinPrefab;

	public GameObject boomboxPrefab;

	public GameObject doorPrefab;

	public GameObject playerPrefab;

	public Vector3 levelCenter;

	string[] levelRows;

	LevelManagerScript levelManager;
	// Use this for initialization
	void Awake () {
		
		levelManager = GetComponent<LevelManagerScript> ();
		levelRows = level.text.Split(new char[]{'\n'});
		Bounds wallBounds = wallPrefab.transform.GetChild(0).GetComponent<MeshFilter> ().sharedMesh.bounds;

		Bounds tableBounds = tablePrefab.transform.GetChild(0).GetComponent<MeshFilter> ().sharedMesh.bounds;
		float wallLengthInMeters = tableBounds.extents.x * 2f * tablePrefab.transform.localScale.x;
		float wallHalfHeightInMeters = wallBounds.extents.y * wallPrefab.transform.localScale.y;
		float wallWidthInMeters = tableBounds.extents.z * 2f * tablePrefab.transform.localScale.z;
		Bounds floorBounds = floorPrefab.GetComponent<MeshFilter> ().sharedMesh.bounds;
		float floorLengthInMeters = wallLengthInMeters * levelRows[0].Trim().Length;
		float floorWidthInMeters = wallWidthInMeters * (levelRows.Length - 1);

		GameObject floor = Instantiate (floorPrefab);

		floor.transform.localScale = new Vector3 (floorLengthInMeters / (floorBounds.extents.x * 2f), 1f, floorWidthInMeters / (floorBounds.extents.z * 2f));

		floor.transform.position = levelCenter;

		float zOffset = floorWidthInMeters / (float)(levelRows.Length - 1);

		levelManager.levelGrid = new Tile[levelRows.Length - 1][];

		Vector3 topLeftCorner = new Vector3 (levelCenter.x - (floorLengthInMeters / 2f), levelCenter.y + wallHalfHeightInMeters, levelCenter.z - (floorWidthInMeters / 2));
	
		for (int i = 0; i < levelRows.Length - 1; ++i) {
			levelRows [i] = levelRows [i].Trim ();
			float xOffset = floorLengthInMeters / (float)(levelRows [i].Length);

			levelManager.levelGrid [i] = new Tile[levelRows [i].Length];

			for(int j = 0; j < levelRows[i].Length; ++j) {

				Vector3 placePosition = new Vector3 (topLeftCorner.x + j * xOffset + wallLengthInMeters / 2f, 
					                        topLeftCorner.y, topLeftCorner.z + i * zOffset + wallWidthInMeters / 2f);

				Tile tile = new Tile ();
				tile.position = placePosition;
				tile.position.y = 0;
				tile.isWall = false;
				GameObject instance = null;
				switch (levelRows [i] [j]) {
				case 'W':
					instance = Instantiate (wallPrefab);
					tile.isWall = true;
					break;
				case 'E':
					instance = Instantiate (wallPrefab);
					instance.transform.Rotate (0, 90f, 0f);
					tile.isWall = true;
					break;
				case 'T':
					instance = Instantiate (tablePrefab);
					tile.isWall = true;
					break;
				case 'S':
					instance = Instantiate (shirtBinPrefab);
					break;
				case 'B':
					instance = Instantiate (boomboxPrefab);
					break;
				case 'U':
					instance = Instantiate (doorPrefab);
					instance.transform.Rotate (0, 180f, 0f);
					break;
				case 'n':
					instance = Instantiate (doorPrefab);
					break;
				case 'D':
					instance = Instantiate (doorPrefab);
					instance.transform.Rotate (0, 90f, 0f);
					break;
				case 'P':
					instance = Instantiate (playerPrefab);
					placePosition.y -= 0.5f;
					break;
				case 'd':
					instance = Instantiate (playerPrefab);
					instance.transform.Rotate (0f, 180f, 0f);
					placePosition.y -= 0.5f;
					break;
				
				}
				if (instance != null) {
					instance.transform.position = placePosition;
				}



				levelManager.levelGrid [i] [j] = tile;
			}
		}
        

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
