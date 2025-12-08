using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
   [SerializeField] WiringPool wiringPool;
   [SerializeField] List<FlashingLight> allLights;
   List<List<GameObject>> connections;
   List<List<GameObject>> internal_connections;
   List<List<GameObject>> omni_connections;
   List<GameObject> visit;
   List<GameObject> seen;
   // Fill requiredComponents with strings that match needed items
   [SerializeField] List<string> requiredComponents;
   bool winFlag = false;
   // Start is called before the first frame update
   void Start()
   {
       connections = wiringPool.getWireConnections();
       internal_connections = wiringPool.getInternalWires();
       omni_connections = wiringPool.getOmniWires();
       visit = new List<GameObject>();
       seen = new List<GameObject>();
   }


   // Update is called once per frame
   void Update()
   {
       // Get current connections list
       connections = wiringPool.getWireConnections();
       internal_connections = wiringPool.getInternalWires();
       omni_connections = wiringPool.getOmniWires();
       if (connections.Count == 0){
           StopAllLights();
           return;
       }

       if(connections.Count > 0)
       {
           visit.Clear();
           seen.Clear();
           List<List<object>> loops = search(connections[0][0]);
           if(loops.Count > 0)
           {
               //Debug.Log("Loop found");
               if (loops.Count == 0)
               {
                   StopAllLights();
                   return;
               }


               // If more than 1 loop exists, it's considered a fail
               if (loops.Count > 1)
               {
                   TriggerFail();
                   return;
               }


               // Now exactly 1 loop -> validate it
               bool loopIsValid = validate(loops[0]);


               if (loopIsValid)
                   TriggerSuccess();
               else
                   TriggerFail();
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


       for(int i = 0; i < omni_connections.Count; ++i)
       {
           if (omni_connections[i][0] == node){
               children.Add(omni_connections[i][1]);
           }
           else if (omni_connections[i][1] == node){
               children.Add(omni_connections[i][0]);
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
       child.AddRange(getChildrenInternal(node));
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


   bool validate(List<object> loop)
   {
       /*if (winFlag)
       {
           return true;
       }*/


       List<object> valid = new List<object>(loop);
       valid.RemoveAt(valid.Count - 1);


       if (valid.Count != requiredComponents.Count){
           Debug.Log("Incorrect number of parts in loop");
           return false;
       }


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
           Debug.Log("Wrong components");
           return false;
       }


       // Check if components satisfy polarity rules
       string direction_flag = "NULL";
       bool direction_set = false;


       for (int i = 0; i < loop.Count - 2; ++i){
           if(direction_set){
               break;
           }
           for(int j = 0; j < internal_connections.Count; ++j){
               if(internal_connections[j][0] == ((GameObject)loop[i])){
                   if(internal_connections[j][1] == ((GameObject)loop[i+1])){
                       direction_flag = "RIGHT";
                       direction_set = true;
                       break;
                   }
               }
               else if(internal_connections[j][1] == ((GameObject)loop[i])){
                   if(internal_connections[j][0] == ((GameObject)loop[i+1])){
                       direction_flag = "LEFT";
                       direction_set = true;
                       break;
                   }
               }
           }
       }


       if(!direction_set){
           for(int j = 0; j < internal_connections.Count; ++j){
               if(internal_connections[j][0] == ((GameObject)loop[0])){
                   if(internal_connections[j][1] == ((GameObject)loop[loop.Count - 2])){
                       direction_flag = "LEFT";
                       direction_set = true;
                       break;
                   }
               }
               else if(internal_connections[j][1] == ((GameObject)loop[0])){
                   if(internal_connections[j][0] == ((GameObject)loop[loop.Count - 2])){
                       direction_flag = "RIGHT";
                       direction_set = true;
                       break;
                   }
               }
           }
       }


       Debug.Log("Direction flag: " + direction_flag);


       if(!direction_set){
           // No polar components used, loop valid
           //Debug.Log("Level Complete");
           //winFlag = true;
           //return true;
           return false;
       }
       else if(direction_flag == "RIGHT"){
           for (int i = 0; i < loop.Count - 2; ++i){
               for(int j = 0; j < internal_connections.Count; ++j){
                   if(internal_connections[j][0] == ((GameObject)loop[i+1])){
                       if(internal_connections[j][1] == ((GameObject)loop[i])){
                           Debug.Log("Invalid polarity");
                           return false;
                       }
                   }
               }
           }
           for(int i = 0; i < internal_connections.Count; ++i){
               if(internal_connections[i][0] == ((GameObject)loop[0])){
                   if(internal_connections[i][1] == ((GameObject)loop[loop.Count - 2])){
                       Debug.Log("Invalid polarity");
                       return false;
                   }
               }
           }
       }
       else if(direction_flag == "LEFT"){
           for (int i = 0; i < loop.Count - 2; ++i){
               for(int j = 0; j < internal_connections.Count; ++j){
                   if(internal_connections[j][0] == ((GameObject)loop[i])){
                       if(internal_connections[j][1] == ((GameObject)loop[i+1])){
                           Debug.Log("Invalid polarity");
                           return false;
                       }
                   }
               }
           }
           for(int i = 0; i < internal_connections.Count; ++i){
               if(internal_connections[i][0] == ((GameObject)loop[loop.Count - 2])){
                   if(internal_connections[i][1] == ((GameObject)loop[0])){
                       Debug.Log("Invalid polarity");
                       return false;
                   }
               }
           }
       }


       // Passed all checks, circuit valid
       Debug.Log("Level Complete");
       winFlag = true;
       return true;
   }


   //Lights for level complete
   void TriggerSuccess(){
       foreach (var light in allLights){
           light.flashColor = Color.green;
           light.StartFlashing();
       }
   }
   // Lights for level incomplete
   void TriggerFail(){
       foreach (var light in allLights){
           light.flashColor = Color.red;
           light.StartFlashing();
       }
   }


   void StopAllLights(){
       foreach (var light in allLights){
           light.StopFlashing();
       }
   }
}
}

