using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public bool displayGridGizmos;

    public Transform player;
    Node[,] grid;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    public LayerMask unWalkableMsk;

    float nodeDiameter;
    int gridsizex, gridsizey;


    private void Awake()
    {
        nodeDiameter = nodeRadius * 2;
        gridsizex = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridsizey = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();

    }
    public int MaxSize
    {
        get
        {
            return gridsizex * gridsizey;
        }
    }
    void CreateGrid()
    {
        bool bWalkable;
        grid = new Node[gridsizex,gridsizey];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x/2 -Vector3.up * gridWorldSize.y/2;
        for (int x = 0; x<gridsizex;x++)
        {
            for(int y = 0; y<gridsizey;y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.up * (y * nodeDiameter + nodeRadius);
                Collider2D walkable = Physics2D.OverlapCircle(worldPoint, nodeRadius, unWalkableMsk);
                if (walkable == null)
                {
                    bWalkable = true;
                }
                else
                {
                    bWalkable = false;
                }
                grid[x, y] = new Node(bWalkable, worldPoint, x,y);
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
                if (x == 0 && y == 0)
                    continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < gridsizex && checkY >= 0 && checkY < gridsizey)
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
		float percentY = (worldPosition.y + gridWorldSize.y / 2) / gridWorldSize.y;

        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);



		int x = Mathf.RoundToInt((gridsizex-1)*percentX);
		int y = Mathf.RoundToInt((gridsizey) * percentY -0.5f);

		return grid[x, y];

	}

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x,0, gridWorldSize.y));
            if (grid != null && displayGridGizmos)
            {
                Node playerNode = NodeFromWorldPoint(player.position);

                foreach (Node N in grid)
                {
                    Gizmos.color = (N.walkable) ? Color.white : Color.red;
                    if (playerNode.worldPosition == N.worldPosition)
                    {
                        Gizmos.color = Color.cyan;
                    }
                    Gizmos.DrawCube(N.worldPosition, Vector3.one * (nodeDiameter - 0.1f));
                }
            }
        

        
    }
    static void drawString(string text, Vector3 worldPos, Color? colour = null)
    {
        UnityEditor.Handles.BeginGUI();
        if (colour.HasValue) GUI.color = colour.Value;
        var view = UnityEditor.SceneView.currentDrawingSceneView;
        Vector3 screenPos = view.camera.WorldToScreenPoint(worldPos);
        Vector2 size = GUI.skin.label.CalcSize(new GUIContent(text));
        GUI.Label(new Rect(screenPos.x - (size.x / 2), -screenPos.y + view.position.height + 4, size.x, size.y), text);
        UnityEditor.Handles.EndGUI();
    }
}
