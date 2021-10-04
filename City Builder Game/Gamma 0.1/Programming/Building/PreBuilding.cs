using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreBuilding : MonoBehaviour
{

    public float SpawnHeight;
    public GameObject building;
    PlayerController pc;
    bool placement = false;
    // Start is called before the first frame update
    void Start()
    {
        pc = FindObjectOfType<PlayerController>();
       
    }

    // Update is called once per frame
    void Update()
    {
        if (placement == false)
        {
            transform.position = new Vector3(pc.MosPos.x, SpawnHeight, pc.MosPos.z);
            if (Input.GetKey(KeyCode.Q))
                gameObject.transform.Rotate( 0, 0, 1);
            if (Input.GetKey(KeyCode.E))
                gameObject.transform.Rotate( 0, 0, -1);
            if (Input.GetMouseButtonDown(0))
                placement = true;
     
        }
       else
            {
                GameObject Building = Instantiate(building);
                Building.transform.position = gameObject.transform.position;
            
                Building.transform.rotation = gameObject.transform.rotation;

                Destroy(gameObject);
                pc.p_Building = false;
            }
    }
}
