using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

// Class that holds information about a certain posiiton, so it's used in a 
// pathfinding algortithm.
class Node
{
    // Nodes may have different values according to your game.
    public enum Value
    {
        FREE,
        BLOCKED
    }

    // Nodes have x and y positions.
    public int posX;
    public int posY;

    // G is a basic cost vallue to go from one node to another.
    public int g = int.MaxValue;

    // H is a heuristic that estimates the cost of the closest path.
    // F = G + H.
    public int f = int.MaxValue;

    // Reference to other nodes so it is possbile to build a path.
    public Node parent;

    // The value of the node.
    public Value value;

    // Constructor.
    public Node(int posX, int posY)
    {
        this.posX = posX;
        this.posY = posY;

        value = Value.FREE;
    }
}

public class AStar : MonoBehaviour
{
    // Constants.
    private const int MAP_SIZE = 6;

    // Variables.
    private List<string> map;
    private Node[,] nodeMap;

    // Start is called before the first frame update
    void Start()
    {
        map = new List<string>();
        map.Add("G-----");
        map.Add("XXXXX-");
        map.Add("S-X-X-");
        map.Add("--X-X-");
        map.Add("--X-X-");
        map.Add("------");

        // Parse the map.
        nodeMap = new Node[MAP_SIZE, MAP_SIZE];
        Node start = null;
        Node goal = null;

        for (int y = 0; y < MAP_SIZE; y++)
        {
            for (int x = 0; x < MAP_SIZE; x++)
            {
                Node node = new Node(x, y);

                char currentChar = map[y][x];
                if (currentChar == 'X')
                {
                    node.value = Node.Value.BLOCKED;
                }
                else if (currentChar == 'G')
                {
                    goal = node;
                }
                else if (currentChar == 'S')
                {
                    start = node;
                }

                nodeMap[x, y] = node;
            }
        }

        List<Node> nodePath = ExecuteAstar(start, goal);

        // TODO: burn the path in the map.
        foreach (Node node in nodePath)
        {
            char[] charArray = map[node.posY].ToCharArray();
            charArray[node.posX] = '+';
            map[node.posY] = new string(charArray);
        }

        // Print the map.
        string mapString = "";
        foreach (string mapRow in map)
        {
            mapString += mapRow + '\n';
        }
        Debug.Log(mapString);
    }

    private List<Node> ExecuteAstar(Node start, Node goal)
    {
        // Holds potential best path nodes that starts with the origin.
        List<Node> openList = new List<Node>() { start };

        // This list keeps track of all nodes that have been visited.
        List<Node> closedList = new List<Node>();

        // Initialize the start node.
        start.g = 0;
        start.f = CalculateHeuristicValue(start, goal);

        // Main algorithm.
        while (openList.Count > 0)
        {
            // Find the node with the lowest setimated cost to reach the target.
            Node current = openList[0];
            foreach (Node node in openList)
            {
                if (node.f < current.f)
                {
                    current = node;
                }
            }

            // Check if the target has been reached.
            if (current == goal)
            {
                return BuildPath(goal);
            }

            // Make sure the current node will not be visited again.
            openList.Remove(current);
            closedList.Add(current);

            // Execute the algorithm in the current node's neighbours.
            List<Node> neighbours = GetNeighbourNodes(current);
            foreach (Node neighbour in neighbours)
            {
                // The neighbour has already been visited.
                if (closedList.Contains(neighbour))
                {
                    continue;
                }

                // The neightbour has not been scheduled for visiting.
                if (!openList.Contains(neighbour))
                {
                    openList.Add(neighbour);
                }

                // Calculate new G value and test if it is better.
                int candidateG = current.g + 1;

                // Does not belong to a good path.
                if (candidateG >= neighbour.g)
                {
                    continue;
                }
                // Found a better way. Initialize its vallues.
                else
                {
                    neighbour.parent = current;
                    neighbour.g = candidateG;
                    neighbour.f = neighbour.g + CalculateHeuristicValue(neighbour, goal);
                }
            }
        }

        // If this point reached, it means no path found.
        return new List<Node>();
    }

    private List<Node> GetNeighbourNodes(Node node)
    {
        List<Node> neighbours = new List<Node>();

        // Verify all possible neightbours. Note that if a node is blocked,
        // it cannot be visited.
        if (node.posX - 1 >= 0)
        {
            Node candidate = nodeMap[node.posX - 1, node.posY];

            if (candidate.value != Node.Value.BLOCKED)
            {
                neighbours.Add(candidate);
            }
        }

        if (node.posX + 1 <= MAP_SIZE - 1)
        {
            Node candidate = nodeMap[node.posX + 1, node.posY];

            if (candidate.value != Node.Value.BLOCKED)
            {
                neighbours.Add(candidate);
            }
        }
        if (node.posY - 1 >= 0)
        {
            Node candidate = nodeMap[node.posX, node.posY - 1];

            if (candidate.value != Node.Value.BLOCKED)
            {
                neighbours.Add(candidate);
            }
        }

        if (node.posY + 1 <= MAP_SIZE - 1)
        {
            Node candidate = nodeMap[node.posX, node.posY + 1];

            if (candidate.value != Node.Value.BLOCKED)
            {
                neighbours.Add(candidate);
            }
        }

        return neighbours;
    }

    private int CalculateHeuristicValue(Node node1, Node node2)
    {
        return Mathf.Abs(node1.posX - node2.posX) + Mathf.Abs(node1.posY - node2.posY);
    }

    private List<Node> BuildPath(Node node)
    {
        List<Node> path = new List<Node>() { node };

        while (node.parent != null)
        {
            node = node.parent;
            path.Add(node);
        }

        return path;
    }

}
