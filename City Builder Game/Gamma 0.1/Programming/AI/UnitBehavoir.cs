using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class UnitBehavoir : MonoBehaviour
{

    [Header("Unit Stats")]
    public float health;
    public float aggro;
    public float speed;
    public int ammo;
    public Unit unit;

     void Start()
    {
        unit.Move();
    }
   

    public void lookatlocation(Vector3 newloc)
    {
        Vector3 self = transform.position;
        Vector3 lookDir = newloc - self;

        float angle = Mathf.Atan2(lookDir.x, lookDir.z) * Mathf.Rad2Deg;

        if (angle != 0)
        {
            Quaternion rotation = Quaternion.AngleAxis(angle, new Vector3(0, 1, 0));
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, speed * Time.deltaTime);

        }
    }

   



}
