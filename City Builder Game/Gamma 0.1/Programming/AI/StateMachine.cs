using UnityEngine;
using System.Collections;

public class StateMachine : MonoBehaviour
{
    PlayerUnit player;
    Gamemanager GM;

   public enum overState
    {
        peace,
        fighting
    }
    public enum Acting
    {
        Attacking,
        Defending,
        FollowingOrders,
        Task,
        OnGuard,
        Idle
    }
   
    public overState state0;
    public Acting state1;

    public float CurrentAggro;
    public float Currenthealth;
    

    public void Start()
    {
        GM = FindObjectOfType<Gamemanager>();
        player = GetComponent<PlayerUnit>();
        
    }

    private void Update()
    {
        CurrentAggro = player.aggro;
        Currenthealth = player.health;

        

        State0();
        State1();
        
    }
    // Update is called once per frame


    public void State0()
    {
        
        switch (state0)
        {
            case overState.peace:
                if (CurrentAggro > 0)
                    state0 = overState.fighting;
                    

                break;
            case overState.fighting:
                if (CurrentAggro <= 0)
                    state0 = overState.peace;
                    


                break;
            default:
                state0 = overState.peace;
                break;
               

        }
       
    }

    public void State1()
    {


        if (state0 == overState.fighting)
        {
            if ( player.select == true )
            {
                state1 = Acting.FollowingOrders;
            }
            else if (CurrentAggro >= Currenthealth)
            {
                state1 = Acting.Defending;
            }
            else if (CurrentAggro < Currenthealth)
            {
                state1 = Acting.Attacking;
            }
        }

         if (state0 == overState.peace)
        {
            if (player.select == false)
                state1 = Acting.Idle;

            else if (player.select == true)
            {
                state1 = Acting.OnGuard;
            }

        }

        switch (state1)
        {
            case Acting.Attacking:
                player.Attacking();
                break;
            case Acting.Defending:
                player.Defending();
                break;
            case Acting.FollowingOrders:
                player.FollowingOrders();
                break;
            case Acting.Task:
                player.Task();
                break;
            case Acting.OnGuard:
                player.OnGuard();
                break;
            case Acting.Idle:
                player.Idle();
                break;
            default:
                player.Idle();
                break;
        }

    }

}
