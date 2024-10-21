using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotor : MonoBehaviour
{
    public Transform lookAt;
    public Vector3 offset =  new Vector3(0, 2.5f, -2);
    public bool IsMoving { set; get;}
    public Vector3 rotation = new Vector3(35, 0, 0);
    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        if(!IsMoving)
        return;

        Vector3 desiredPosition = lookAt.position + offset;
        desiredPosition.x = 0;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation,Quaternion.Euler(rotation),0.1f);
    }
}
