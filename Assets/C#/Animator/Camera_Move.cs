using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Move : MonoBehaviour {
    public Transform Role;
    private Transform camera_transform;
    private float distanceAway = 3.0f;
    private float distanceUp = 2.0f;
    private float distanceRight = 0.5f;
    private Vector3 distanceVector;
    // Use this for initialization
    private void Awake()
    {
        camera_transform = this.GetComponent<Transform>();
        Vector3 p = Role.TransformPoint(Vector3.up * distanceUp - Vector3.forward * distanceAway + Vector3.right * distanceRight);
        distanceVector = p - Role.position;
        camera_transform.position = p;
        camera_transform.LookAt(Role.position + Vector3.up * 1.5f + Role.right * 0.5f);
    }
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
    }
    private void LateUpdate()
    {
        
        camera_transform.position = Role.position + distanceVector;
        float cam_h = Input.GetAxis("Mouse X");
        float cam_v = Input.GetAxis("Mouse Y");
        camera_transform.RotateAround(Role.position + Vector3.up * 2, Vector3.up, cam_h*50f*Time.deltaTime);
        float angleX = camera_transform.rotation.eulerAngles.x;
        //Debug.Log(angleX);
        float nextAngleX = -cam_v * 0.5f + angleX;
        if (nextAngleX >= 360f)
        {
            nextAngleX -= 360f;
        }
        if ((nextAngleX < 30f) || (nextAngleX <= 360f && nextAngleX > 330f))
            camera_transform.RotateAround(Role.position + Vector3.up * 2, camera_transform.right, -cam_v*50f * Time.deltaTime);
        
        distanceVector = camera_transform.position - Role.position;

    }
}
