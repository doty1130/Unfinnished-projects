using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    Gamemanager GM;
    public enum type
    { 
        CashCrate,
        PowerCrate,
        PersonalTent,
        PowerGen
    }

    public type _Type;  
    

    // Start is called before the first frame update
    void Start()
    {
        GM = FindObjectOfType<Gamemanager>();
        type_check();
    }

    IEnumerator Resource(string resource) 
    {
        if(resource == "CashCrate")
        {
           
            while (true)
            {
                if (GM.Cash < GM.cash_Limit)
                {
                    GM.Cash += 1;
                    yield return new WaitForSeconds(1);
                }
                else
                {
                    yield return new WaitForSeconds(1);
                }
            }
        }


        if (resource == "PowerGen")
        {

            while (true)
            {
                if (GM.Power < GM.Power_Limit)
                {
                    GM.Power += 1;
                    yield return new WaitForSeconds(1);
                }
                else
                {
                    yield return new WaitForSeconds(1);
                }
            };

        }



        if (resource == "PowerCrate")
        {
          
                    GM.Power_Limit += 50;
                    yield return new WaitForSeconds(1);
             
        }

        if (resource == "PersonalTent")
        {
            GM.Unit_Points += 5;
        }
    }
        

    void type_check()
    {
            switch (_Type)
            {
                case type.CashCrate:
                StartCoroutine("Resource", "CashCrate");
                    break;

                case type.PowerCrate:
                StartCoroutine("Resource", "PowerCrate");
                    break;

            case type.PowerGen:
                StartCoroutine("Resource", "PowerGen");
                break;

            case type.PersonalTent:
                StartCoroutine("Resource", "PersonalTent");
                    break;
            }
       
    }
}
