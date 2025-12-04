using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] WiringPool wiringPool;
    List<List<GameObject>> connections;
    List<List<GameObject>> internal_connections;
    List<GameObject> visit;
    List<GameObject> seen;
    // Fill requiredComponents with strings that match needed items
    [SerializeField] List<string> requiredComponents;
    bool winFlag = false;
    // Start is called before the first frame update
    void Start()
    {
        connections = wiringPool.getWireConnections();
        internal_connections = new List<List<GameObject>>();
        visit = new List<GameObject>();
        seen = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        // Get current connections list
        connections = wiringPool.getWireConnections();
        // internal_connections = wiringPool.getInternalConnections();
        if(connections.Count > 0)
        {
            visit.Clear();
            seen.Clear();
            List<List<object>> loops = search(connections[0][0]);
            if(loops.Count > 0)
            {
                //Debug.Log("Loop found");
                for(int i = 0; i < loops.Count; ++i)
                {
                    validate(loops[i]);
                }
            }
        }

    }

    // Get list of connected children based on connections list
    List<GameObject> getChildren(GameObject node)
    {
        List<GameObject> children = new List<GameObject>();
        for(int i = 0; i < connections.Count; ++i)
        {
            if (connections[i][0] == node){
                children.Add(connections[i][1]);
            }
            else if (connections[i][1] == node){
                children.Add(connections[i][0]);
            }
        }
        return children;
    }

    // Get list of connected children based on internal connections list
    List<GameObject> getChildrenInternal(GameObject node)
    {
        List<GameObject> children = new List<GameObject>();
        for(int i = 0; i < internal_connections.Count; ++i)
        {
            if (internal_connections[i][0] == node){
                children.Add(internal_connections[i][1]);
            }
            else if (internal_connections[i][1] == node){
                children.Add(internal_connections[i][0]);
            }
        }
        return children;
    }

    // Search for loops from a starting node
    // Returns list of loops, stores as lists of game objects
    List<List<object>> search(GameObject node)
    {
        visit.Add(node);
        if (!seen.Contains(node))
        {
            seen.Add(node);
        }

        List<GameObject> child = getChildren(node);
        // child.AddRange(getChildrenInternal(node));
        List<List<object>> loops = new List<List<object>>();
        List<GameObject> updateChild = new List<GameObject>(child);

        for(int i = 0; i < child.Count; ++i)
        {
            if (visit.Contains(child[i]))
            {
                updateChild.Remove(child[i]);
            }
        }

        for(int i = 0; i < updateChild.Count; ++i)
        {
            if (seen.Contains(updateChild[i]))
            {
                loops.Add(new List<object> {updateChild[i], false});

            }
            else
            {
                seen.Add(updateChild[i]);
            }
        }

        for(int i = 0; i < child.Count; ++i)
        {
            List<List<object>> retVals;
            if (!visit.Contains(child[i]))
            {
                retVals = search(child[i]);
            }
            else
            {
                retVals = new List<List<object>>();
            }
            if(retVals.Count > 0)
            {
                loops.AddRange(retVals);
            }
        }

        for(int i = 0; i < loops.Count; ++i)
        {
            if((bool)loops[i][loops[i].Count - 1] == false)
            {
                if(updateChild.Contains((GameObject)loops[i][0]) && loops[i].Count > 2)
                {
                    loops[i][loops[i].Count - 1] = true;
                    loops[i].Insert(loops[i].Count - 1, node);
                }
                else if((GameObject)loops[i][0] == node)
                {
                    loops[i][loops[i].Count - 1] = true;
                }
                else
                {
                    loops[i].Insert(loops[i].Count - 1, node);
                }
            }
        }
        return loops;
    }

    void validate(List<object> loop)
    {
        if (winFlag)
        {
            return;
        }

        List<object> valid = new List<object>(loop);
        valid.RemoveAt(valid.Count - 1);

        for(int i = 0; i < requiredComponents.Count; ++i)
        {
            for(int j = 0; j < valid.Count; ++j)
            {
                if(((GameObject)valid[j]).GetComponent<ElectricComponent>().componentName == requiredComponents[i])
                {
                    valid.Remove(valid[j]);
                    Debug.Log("Found required object " + requiredComponents[i]);
                    break;
                }
            }
        }
        if(valid.Count > 0)
        {
            Debug.Log("Fail");
        }
        else
        {
            Debug.Log("Level Complete");
            winFlag = true;
        }
    }

    /*
    def validate(loop):
	length = len(loop) - 1
	direction_flag = "NULL"
	for i in range(0 , length -1):
		if [loop[i], loop[i+1]] in edge_internal:
			direction_flag = "RIGHT"
			break
		elif [loop[i+1],loop[i]] in edge_internal:
			direction_flag = "LEFT"
			break
	if direction_flag == "NULL":
		if [loop[0], loop[length - 1]] in edge_internal:
			direction_flag = "LEFT"
		elif [loop[length-1], loop[0]] in edge_internal:
			direction_flag = "RIGHT"
	if direction_flag == "NULL":
		return "Circuit valid"
	elif direction_flag == "RIGHT":
		for i in range(0, length -1):
			if [loop[i+1], loop[i]] in edge_internal:
				return "Invalid circuit"
		if [loop[0],loop[length -1]] in edge_internal:
			return "Invalid circuit"
	elif direction_flag == "LEFT":
		for i in range(0, length -1):
			if [loop[i], loop[i+1]] in edge_internal:
				return "Invalid circuit"
		if [loop[length-1],loop[0]] in edge_internal:
			return "Invalid circuit"
	return "Valid circuit"
    */
}