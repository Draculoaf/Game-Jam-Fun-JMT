using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System;

public class PathFinding : MonoBehaviour
{
	PathRequestManager requestManager;
	//public Transform seeker, target;//no longer needed since we pass in targe& end
	Grid grid;

	void Awake()
	{
		requestManager = GetComponent<PathRequestManager>();
		grid = GetComponent<Grid>();
	}

	
	public void StartFindPath(Vector3 startPos, Vector3 targetPos)
    {
		StartCoroutine(FindPath(startPos, targetPos));
    }
	IEnumerator FindPath(Vector3 startPos, Vector3 targetPos)
	{
		Stopwatch sw = new Stopwatch();
		sw.Start();

		Vector3[] waypoints = new Vector3[0];
		bool pathSuccess = false;


		Node startNode = grid.NodeFromWorldPoint(startPos);
		Node targetNode = grid.NodeFromWorldPoint(targetPos);

		if (startNode.walkable && targetNode.walkable) // dont bother if either are in invalid space
		{


			Heap<Node> openSet = new Heap<Node>(grid.MaxSize);//implimenting the heap
			HashSet<Node> closedSet = new HashSet<Node>();
			openSet.Add(startNode);

			while (openSet.Count > 0)
			{

				Node node = openSet.RemoveFirst();

				closedSet.Add(node);

				if (node == targetNode)
				{
					sw.Stop();
					print("PATH FOUND :" + sw.ElapsedMilliseconds);
					pathSuccess = true;
					

					break;
				}

				foreach (Node neighbour in grid.GetNeighbours(node))
				{

					if (!neighbour.walkable || closedSet.Contains(neighbour))
					{
						continue;
					}

					int newCostToNeighbour = node.gCost + GetDistance(node, neighbour);

					if (newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
					{
						neighbour.gCost = newCostToNeighbour;
						neighbour.hCost = GetDistance(neighbour, targetNode);
						neighbour.parent = node;
						if (!openSet.Contains(neighbour))
							openSet.Add(neighbour);
						else
							openSet.updateItem(neighbour);
					}
				}
			}
		}
		yield return null; // wait for 1 frame before returning

		if (pathSuccess)//we only generate the path if target was found, this puts some of my concerns to rest
        {
			waypoints = RetracePath(startNode, targetNode);
		}
		requestManager.FinishedProcessingPath(waypoints, pathSuccess);
	}

	Vector3[] RetracePath(Node startNode, Node endNode)
	{
		List<Node> path = new List<Node>();
		Node currentNode = endNode;

		while (currentNode != startNode)
		{
			path.Add(currentNode);
			currentNode = currentNode.parent;
		}
		Vector3[] waypoints =  SimplifyPath(path);
		Array.Reverse(waypoints);
		return waypoints;



	}
	//dont need to store everyway point just need the path direction changes
	Vector3[] SimplifyPath(List<Node> path)
    {
		List<Vector3> waypoints = new List<Vector3>();
		Vector2 directionOld = Vector2.zero;

		for (int i = 1; i<path.Count; i++)
        {
			Vector2 directionNew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
            if (directionNew != directionOld)
            {
				waypoints.Add(path[i].worldPosition);
            }
			directionOld = directionNew;
        }

		return waypoints.ToArray();
    }
	int GetDistance(Node nodeA, Node nodeB)
	{
		
		int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
		int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);
		
		if (dstX > dstY)
			return 14 * dstY + 10 * (dstX - dstY);
		return 14 * dstX + 10 * (dstY - dstX);
	}
}