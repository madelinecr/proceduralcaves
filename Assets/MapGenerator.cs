using UnityEngine;
using System.Collections;

using System;

public class MapGenerator : MonoBehaviour {

	[Range (0,100)] public int randomFillPercent;
	public int width, height;

	public string seed;
	public bool useRandomSeed;

	private int[,] map;

	void Start() {
		GenerateMap ();
	}

	void Update() {
		if (Input.GetMouseButtonDown(0)) {
			GenerateMap ();
		}
	}

	void GenerateMap() {
		map = new int[width, height];
		RandomFillMap ();

		for (int i = 0; i < 5; i++) {
			SmoothMap ();
		}

		int borderSize = 5;
		int[,] borderedMap = new int[width + borderSize * 2, height + borderSize * 2];

		for (int x = 0; x < borderedMap.GetLength (0); x++) {
			for (int y = 0; y < borderedMap.GetLength (1); y++) {
				if (x >= borderSize && x < width + borderSize && y >= borderSize && y < height + borderSize) {
					borderedMap [x, y] = map [x - borderSize, y - borderSize];
				} else {
					borderedMap [x, y] = 1;
				}
			}
		}

		MeshGenerator meshGen = GetComponent<MeshGenerator> ();
		meshGen.GenerateMesh (borderedMap, 1);
	}

	void RandomFillMap() {
		if (useRandomSeed) {
			seed = Time.time.ToString ();
		}
		System.Random prng = new System.Random (seed.GetHashCode ());

		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				if (x == 0 || x == width - 1 || y == 0 || y == height - 1) {
					map [x, y] = 1;
				} else {
					map [x, y] = (prng.Next (0, 100) < randomFillPercent) ? 1 : 0;
				}
			}
		}
	}

	void SmoothMap() {

		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				int neighborWallTiles = GetSurroundingTileCount (x, y);

				if (neighborWallTiles > 4) {
					map [x, y] = 1;
				} else if (neighborWallTiles < 4) {
					map [x, y] = 0;
				}
			}
		}

	}

	int GetSurroundingTileCount(int x, int y) {
		int wallCount = 0;

		for (int neighborX = x - 1; neighborX <= x + 1; neighborX++) {
			for (int neighborY = y - 1; neighborY <= y + 1; neighborY++) {
				if (neighborX >= 0 && neighborX < width && neighborY >= 0 && neighborY < height) {
					if (neighborX != x || neighborY != y) {
						wallCount += map [neighborX, neighborY];
					}
				} else {
					wallCount++;
				}

			}
		}
		return wallCount;
	}

	void OnDrawGizmos() {
		/*
		if (map == null)
			return;

		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				Gizmos.color = (map [x, y] == 1) ? Color.black : Color.white;
				Vector3 position = new Vector3 (-width / 2 + x + .5f, 0, -height / 2 + y + .5f);
				Gizmos.DrawCube (position, Vector3.one);
			}
		}
		*/
	}
}
