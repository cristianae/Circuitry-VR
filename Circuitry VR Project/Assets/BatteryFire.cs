using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryFire : MonoBehaviour
{
    [SerializeField] WiringPool wiringPool;
    [SerializeField] GameObject fireEffect;
    [SerializeField] GameObject positiveEnd;
    [SerializeField] GameObject negativeEnd;
    bool stopFire = false;
  
    // Update is called once per frame
    void Update()
    {   
        bool circuitComplete = IsConnected(positiveEnd, negativeEnd);
        if(!stopFire){
            fireEffect.SetActive(circuitComplete);
        }
        else
        {
            fireEffect.SetActive(false);
        }
    }
    public void DisableFire()
    {
        stopFire = true;
        fireEffect.SetActive(false);
    }

    bool IsConnected(GameObject start, GameObject end)
    {
        var connections = wiringPool.getWireConnections();
        var visited =new HashSet<GameObject>();
        var stack = new Stack<GameObject>();
        stack.Push(start);
        while (stack.Count> 0)
        {
           GameObject current = stack.Pop();
           if (!visited.Add(current)) 
               continue;
           if (current == end)
               return true;
            foreach (var pair in connections)
            {
                if (pair[0] == current && !visited.Contains(pair[1]))
                {
                    stack.Push(pair[1]);
                }
                else if (pair[1] == current && !visited.Contains(pair[0]))
                {
                    stack.Push(pair[0]);
                }
            }
        }

        return false;
    }
}
