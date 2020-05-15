using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.U2D.IK;

public class PlayerFootIKTest : MonoBehaviour
{
    public GameObject Foot;
    public GameObject Foot2;
    public LayerMask CastMask;
    public float RayDistance = 1f;
    public float LerpTest = 0f;
    public IKManager2D IK;
    public Animation anim;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up,RayDistance,CastMask);

        if (hit.collider != null)
        {
            Foot.transform.position = new Vector3(Foot.transform.position.x, hit.point.y, Foot.transform.position.z);
            Foot2.transform.position = new Vector3(Foot2.transform.position.x, hit.point.y, Foot2.transform.position.z);
            //IK.weight = 1;
            
        }
        else
        {
            //IK.weight = 0;
        }
        
    }
}
