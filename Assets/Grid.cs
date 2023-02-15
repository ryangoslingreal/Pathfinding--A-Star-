using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Grid : MonoBehaviour
{
	public LayerMask untraversableMask;
	public Vector2 gridWorldSize;
	public float nodeRadius;
	Node[,] grid;

	float nodeDiameter;
	int gridSizeX, gridSizeY;

	void Start()
	{
		nodeDiameter = nodeRadius * 2;
		// calculate how many nodes are required in each axis. 
		// x and y are separate since map may not be a square.
		gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
		gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);

		CreateGrid();
	}

	void CreateGrid()
	{
		grid = new Node[gridSizeX, gridSizeY];
		Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2; // find world position of bottom left corner of grid. corresponds to grid[0, 0].

		for (int x = 0; x < gridSizeX; x++)
		{
			for (int y = 0; y < gridSizeY; y++)
			{
				Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius); // find node's world position.
				bool traversable = !(Physics.CheckSphere(worldPoint, nodeRadius, untraversableMask)); // collision sphere to check if the node is traversable.
				grid[x, y] = new Node(traversable, worldPoint); // add node to grid.
			}
		}
	}

	void OnDrawGizmos()
	{
		Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

		if (grid != null)
		{
			foreach (Node n in grid)
			{
				Gizmos.color = (n.traversable) ? Color.white : Color.red;
				Gizmos.DrawCube(n.worldPos, Vector3.one * (nodeDiameter - 0.1f));
			}
		}
	}
}