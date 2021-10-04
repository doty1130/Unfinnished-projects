using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	private Gamemanager GM;
	
	private Transform m_Transform; 

	public float panSpeed;
	public float rotSpeed;

	string horizontalAxis = "Horizontal";
	string verticalAxis = "Vertical";
	public string ObjectType_LookingAt;

	bool useKeyboardInput = true;
	public bool p_Building = false;

	[Header("Camera Limits")]
	public float limitX = 50f; //x limit of map
	public float limitZ = 50f; //z limit of map
	public float T_limitY = 10; //y limit of map


	public Transform MosTran; 
	public Vector3 MosPos;
	


	Quaternion camOrientation;
	public GameObject MosClick;

	private Vector2 KeyboardInput
	{
		get { return useKeyboardInput ? new Vector2(Input.GetAxis(horizontalAxis), Input.GetAxis(verticalAxis)) : Vector2.zero; }
	}

	void Start()
	{
		
		GM = FindObjectOfType<Gamemanager>();
		m_Transform = transform;
		camOrientation = transform.rotation;
	}

	void Update()
	{
		camMethods();
		MouseFunction();

	}

    #region this is the movement controls
    void camMethods()
	{
		CamOrientation();
		Move();
		MoveLimit();
		CameraClime();
	}


    void CamOrientation()
    {
       
            Vector3 origin = Camera.main.transform.eulerAngles;
            Vector3 destination = origin;


            //if (Input.GetKeyDown(KeyCode.Space))
            //{

            //    Camera.main.transform.rotation = camOrientation;
            //}
            if (Input.GetMouseButton(2))
            {
                destination.x -= Input.GetAxis("Mouse Y") * rotSpeed * Time.deltaTime;
                destination.y += Input.GetAxis("Mouse X") * rotSpeed * Time.deltaTime;
            }
            if (destination != origin)
            {

                Camera.main.transform.eulerAngles = Vector3.MoveTowards(origin, destination, Time.deltaTime * rotSpeed);
            }

            camOrientation = new Quaternion(destination.x, destination.y, destination.z, camOrientation.w);
        
    }

    void Move()
	{

		if (useKeyboardInput)
		{
			Vector3 desiredMove = new Vector3(KeyboardInput.x, 0, KeyboardInput.y);

			desiredMove *= panSpeed;
			desiredMove *= Time.deltaTime;
			desiredMove = Quaternion.Euler(new Vector3(0f, transform.eulerAngles.y, 0f)) * desiredMove;
			desiredMove = m_Transform.InverseTransformDirection(desiredMove);

			m_Transform.Translate(desiredMove, Space.Self);
		}
			
	}

	void CameraClime ()
	{
		//if (limitY < 10 && limitY > 2) 
			Vector3 upMove = new Vector3 (0, Input.GetAxis ("Mouse ScrollWheel"), 0);
			
			upMove *= panSpeed * 50f;
			upMove *= Time.deltaTime;

			m_Transform.Translate (upMove, Space.World);

	}

	void MoveLimit ()
	{
		m_Transform.position = new Vector3(Mathf.Clamp(m_Transform.position.x, -limitX, limitX),
			Mathf.Clamp(m_Transform.position.y, 5, T_limitY),
					Mathf.Clamp(m_Transform.position.z, 15 , limitZ));
	}

	#endregion




	public void MouseFunction()
	{
		if (p_Building == false)
		{
			SendCursorLocation();
			SelectObject();
		}
		else if (p_Building == true)
		{
			
			SendCursorLocation();
			
		}
	}
	public void SendCursorLocation()
		{
			RaycastHit MosPosHit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out MosPosHit))
			{
				Debug.DrawLine(ray.origin, MosPosHit.point, Color.green);
				MosPos = MosPosHit.point;
				ObjectType_LookingAt = MosPosHit.collider.tag;
				MosClick = MosPosHit.collider.gameObject;
				
			}
		}


    private void SelectObject()
		{
			if (Input.GetMouseButtonDown(0))
			{
				if (ObjectType_LookingAt == "Building")
				{
				
					if (GM.Buildings.Contains(MosClick))
						return;

					GM.Buildings.Add(MosClick);
				}
				if (ObjectType_LookingAt == "Unit")
				{
				
					if (GM.Units.Contains(MosClick))
					{ return; }

					GM.Units.Add(MosClick);
				}
				if (ObjectType_LookingAt == "Terrain")
				{
					Vector3 MovePoint = new Vector3(MosPos.x,0,MosPos.z);
					foreach (GameObject item in GM.Units)
					{
					item.GetComponent<PlayerUnit>().checkpoints.Add (MosPos);
					}
					
				}
			}
			if (Input.GetMouseButtonDown(1))
			{
				if (ObjectType_LookingAt == "Building")
				{
					if (GM.Buildings.Contains(MosClick))
						GM.Buildings.Remove(MosClick); 
				}

				if (ObjectType_LookingAt == "Unit")
				{
					
					if (GM.Units.Contains(MosClick))
					{ GM.Units.Remove(MosClick); }
				}

			}
		}
	
	

}






