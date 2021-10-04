using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gui : MonoBehaviour
{
    private Gamemanager GM;
    private PlayerController pc;
    public Text Resource_Cash;
    public Text Resource_Power;
    public Text Resource_Unit;
    

    public void Start()
    {
        GM = FindObjectOfType<Gamemanager>();
        pc = FindObjectOfType<PlayerController>();
    }


    private void Update()
    {
        ResourceCounter(GM.Cash,GM.Units_Count, GM.Power, GM.cash_Limit, GM.Unit_Points, GM.Power_Limit);
    }

    public void ResourceCounter(int cash, int unit, int power, int cash_limit, int unit_limit, int power_limit)
    {
        Resource_Cash.text = $"Cash Limit: ({cash_limit}/{cash})";
        Resource_Unit.text = $"Unit Limit: ({unit_limit}/{unit})";
        Resource_Power.text = $"Power Limit: ({power_limit}/{power})";
    }

    public void placeBuilding(GameObject building)
    {
        pc.p_Building = true;
        GameObject Building_placement = Instantiate(building);

    }
}
