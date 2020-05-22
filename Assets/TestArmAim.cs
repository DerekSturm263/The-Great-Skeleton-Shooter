using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;
public class TestArmAim : MonoBehaviour
{
    public Transform armb;
    public Transform armb2;
    public Animator anim;
    public SpriteSkin sp;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
           armb.transform.localEulerAngles += new Vector3(0, 0, 5);
            armb2.transform.localEulerAngles += new Vector3(0, 0, 5);
        }        
    }
}
