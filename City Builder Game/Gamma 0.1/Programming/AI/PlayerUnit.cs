using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using TreeEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayerUnit : UnitBehavoir
{
    Animator animator;
    Gamemanager GM;
    public Canvas canvas;
    public bool select;
    

    public List<Vector3> checkpoints = new List<Vector3>();
   

    void Start()
    {
        checkpoints.Add(transform.position);
        animator = GetComponent<Animator>();
        GM = FindObjectOfType<Gamemanager>();
        
    }

    // Update is called once per frame
    void Update()
    {
       
        canvas.enabled = isSelected();
        select = isSelected();
        
    }

    #region Selection Gate
    public bool isSelected()
    {
        if (GM.Units.Contains(this.gameObject))
            return true;

     return false;
    }

    #endregion

    public void Attacking()
    {
        Debug.Log("Attacking");
    }

    public void Defending()
    {
        Debug.Log("Defending");
    }

    public void FollowingOrders()
    {
        Checkpoints(checkpoints, 10);
       
    }

    public void Task()
    {
        Debug.Log("Task");
    }

    public void OnGuard()
    {
        Checkpoints(checkpoints, 5);
       
    }

    public void Idle()
    {
       
    }



   void Checkpoints( List<Vector3> points, float _speed)
    { 
        

            float dist = Vector3.Distance(transform.position, points[1]);
           
        if (points.Count > 1)
        {
            speed = _speed;
            animator.SetFloat("Moving", _speed);
        }
        else if (points.Count == 1)
            speed = 0;



        if (dist < 5f)
        { points.Remove(points[1]); }
      
    }

  
}
