using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowMotor : MonoBehaviour
{
    public Transform lookAt;


    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        

        Vector3 desiredPosition = new Vector3(-0.86f, 1.84f, lookAt.position.z);
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime);

    }

}
