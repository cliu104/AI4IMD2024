using System.Collections;
using System.Collections.Generic;
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
    public int g;

    // H is a heuristic that estimates the cost of the closest path.
    public int h;

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
    }

    // Update is called once per frame
    void Update()
    {

    }
}
