using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Astar_grid: MonoBehaviour
{
	Gamemanager GM;
	public LayerMask unwalkableMask;
	public Vector2 gridWorldSize;
	public float nodeRadius;
	public bool displayGridGizmos;
	public TerrainType[] walkableRegions;
	public int obstacleProximityPenalty = 10;
	LayerMask walkableMask;
	Dictionary<int, int> walkableRegionsDictionary = new Dictionary<int, int>();
	
	Node[,] grid;
	
	float nodeDiameter;
	int gridSizeX, gridSizeY;

	int penaltyMin = int.MaxValue;
	int penaltyMax = int.MinValue;

	void Awake()
	{
		GM = FindObjectOfType<Gamemanager>();
	
		nodeDiameter = nodeRadius * 2;
		gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
		gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);

		foreach (TerrainType region in walkableRegions)
		{
			walkableMask.value += region.terrainMask.value;
			walkableRegionsDictionary.Add((int)Mathf.Log(region.terrainMask.value, 2), region.terrainPenalty);
		}
		CreateGrid ();
	}

	public int MaxSize 
	{
		get { return gridSizeX * gridSizeY; }
	}
    
	public void CreateGrid()
	{
		grid = new Node[gridSizeX, gridSizeY];
		Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;

		for(int x = 0; x < gridSizeX; x++)
		{
			for(int y = 0; y < gridSizeY; y++)
			{
				Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
				bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));
				int movementPenalty = 0;

				
					Ray ray = new Ray(worldPoint + Vector3.up * 50, Vector3.down);
					RaycastHit hit;
					if (Physics.Raycast(ray, out hit, 100, walkableMask)) 
					{
						walkableRegionsDictionary.TryGetValue(hit.collider.gameObject.layer, out movementPenalty);
					}

				if (!walkable)
				{ movementPenalty += obstacleProximityPenalty; }

				grid[x, y] = new Node(walkable, worldPoint, x,y, movementPenalty);
			}
		}

		BlurPenaltyMap(2);
	}

	void BlurPenaltyMap(int blurSize)
	{
		int kernelSize = blurSize * 2 + 1;
		int kernalExtents = (kernelSize - 1) / 2;

		int[,] penaltiesHorizontolPass = new int[gridSizeX, gridSizeY];
		int[,] penaltiesVerticalPass = new int[gridSizeX, gridSizeY];

		for (int y = 0; y < gridSizeY; y++)
		{
			for (int x = -kernalExtents; x < kernalExtents; x++) {
				int sampleX = Mathf.Clamp(x, 0, kernalExtents);
				penaltiesHorizontolPass[0, y] += grid[sampleX, y].movementPenalty;
			}
			for (int x =1; x < gridSizeX; x++) 
			{
				int removeIndex = Mathf.Clamp( x - kernalExtents - 1, 0 , gridSizeX);
				int addIndex = Mathf.Clamp(x + kernalExtents, 0, gridSizeX -1);

				penaltiesHorizontolPass[x, y] = penaltiesHorizontolPass[x - 1, y] - grid[removeIndex, y].movementPenalty + grid[addIndex, y].movementPenalty;
			}
		
		}

		for (int x = 0; x < gridSizeX; x++)
		{
			for (int y = -kernalExtents; y <= kernalExtents; y++)
			{
				int sampleY = Mathf.Clamp (y, 0, kernalExtents);
				penaltiesVerticalPass[x, 0] += penaltiesHorizontolPass[x,sampleY];
			}
			
			int blurredPenalty = Mathf.RoundToInt((float)penaltiesVerticalPass[x, 0] / (kernelSize * kernelSize));
			grid[x, 0].movementPenalty = blurredPenalty;

			for (int y = 1; y < gridSizeY; y++)
			{
				int removeIndex = Mathf.Clamp(y - kernalExtents - 1, 0, gridSizeY);
				int addIndex = Mathf.Clamp(y + kernalExtents, 0, gridSizeY - 1);

				penaltiesVerticalPass[x, y] = penaltiesVerticalPass[x , y- 1] - penaltiesHorizontolPass[x, removeIndex] + penaltiesHorizontolPass[x, addIndex];
				blurredPenalty = Mathf.RoundToInt((float)penaltiesVerticalPass[x, y] / (kernelSize * kernelSize));
				grid[x, y].movementPenalty = blurredPenalty;

				if (blurredPenalty > penaltyMax) 
				{ penaltyMax = blurredPenalty; }
				if (blurredPenalty > penaltyMin)
				{ penaltyMin = blurredPenalty; }
			}

		}

	}

	public List<Node> GetNeighbours(Node node)
	{
		List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
			for (int y = -1; y <= 1; y++) 
			{
				if(x == 0 && y == 0)
					continue;
				int checkX = node.gridX + x;
				int checkY = node.gridY + y;

				if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
				{
					neighbours.Add(grid[checkX, checkY]);
				}
			}
		}
		return neighbours;
	}

	public Node NodeFromWorldPoint(Vector3 worldPosition)
	{
		float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
		float percentY = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y;
		percentX = Mathf.Clamp01(percentX );
		percentY = Mathf.Clamp01(percentY );
		int x = Mathf.FloorToInt(gridSizeX * percentX);
		int y = Mathf.FloorToInt(gridSizeY * percentY);

		
		return grid[x, y];
	}


	

	void OnDrawGizmos()
	{
		Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));	
		if (grid != null && displayGridGizmos)
		{
			foreach (Node n in grid)
				{

				Gizmos.color = Color.Lerp(Color.white, Color.black, Mathf.InverseLerp (penaltyMin, penaltyMax, n.movementPenalty));

				Gizmos.color = (n.walkable) ? Gizmos.color : Color.red;
				Gizmos.DrawCube(n.worldPos, Vector3.one * (nodeDiameter-.1f));

				}
			
		}
	}

	[System.Serializable]
	public class TerrainType 
	{
		public LayerMask terrainMask;
		public int terrainPenalty;
	}

}


