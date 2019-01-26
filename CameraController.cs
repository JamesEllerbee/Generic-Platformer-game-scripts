using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField] private Camera cam;
    [SerializeField] private GameObject target;
    public GameObject Target
    {
        get
        {
            return target;
        }

        set
        {
            target = value;
        }
    }
    //used for the Z-axises
    private const int depth = -10;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //this line updates the camera's position to the target's position every frame
        transform.position = Vector3.Lerp(this.transform.position, Target.transform.position, 0.1f) + new Vector3(0, 0, depth);//new Vector3(target.transform.position.x, target.transform.position.y, -10);
    }
}
