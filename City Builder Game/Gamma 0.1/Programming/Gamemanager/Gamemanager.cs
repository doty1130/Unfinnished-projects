using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Gamemanager : MonoBehaviour
{

    
    public List<GameObject> Buildings;
    public List<GameObject> Units;

    public int Cash;
    public int cash_Limit;
    public int Power;
    public int Power_Limit;
    public int Units_Count;
    public int Unit_Points;
   

    public void Start()
    {
        
        cash_Limit = 10;
        Power_Limit = 10;
        Unit_Points = 5;
    }

    public void update()
    {

     

    }

   

    

}
