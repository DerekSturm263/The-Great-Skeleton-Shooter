using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoronoiSplitScreen : MonoBehaviour
{
   
    
    [Header("Player Locations")]
     public GameObject player1;
     public GameObject player2;

    [Header("Camera Locations")]
    public Camera Cam1;
    public Camera Cam2;
    [Header("Temporary,remove later")]
    //public Camera Cam3;
    [Header("Camera Options/Controls")]

    [Header("Split Camera Offset")]

    [Range(3, 9)]
    public float distX = 5;
    public float distY =3;

    [Header("Camera Splitting Controls")]
    [Range(10,20 )]
    public float DistanceRange = 13;

    [Range(0.1f, 6)]
    public float CameraSplitConvergeSpeed = 4;
    float Rotation;

    [Header("Material Setttings")]
    public Material mat;
    public RenderTexture camRT1;
    public RenderTexture camRT2;

    void Awake()
    {
        mat.SetFloat("Rotation", 0);
        player1 = GameObject.Find("1");
        if(GameController.playerCount == GameController.PlayerCount.Multiplayer)
        {
            player2 = GameObject.Find("2");
        }
        else
        {
            player2 = player1;
        }
    }
    
    void Update()
    {
            // gets distance
            float distance = Vector2.Distance(player1.transform.position, player2.transform.position);
            //checks distance
           
            //subtracts distance for lerping for when falloff should start
            float distRange = (distance - DistanceRange) * (CameraSplitConvergeSpeed * 0.1f);

            // sets material property Values
            Rotation = Mathf.Atan2(player1.transform.position.y - player2.transform.position.y, player1.transform.position.x - player2.transform.position.x) * Mathf.Rad2Deg * -1;

            // Material property settings
            mat.SetFloat("Rotation", Rotation);    

            //gets/sets rotational offset of Player1

            float fx1 = distX * Mathf.Cos(Rotation * Mathf.Deg2Rad);
            float fy1 = distY * Mathf.Sin(Rotation * Mathf.Deg2Rad);
            //Cam1.transform.position = new Vector3(player1.position.x - fx1, player1.position.y + fy1, -10);

            //gets/sets rotational offset of Player2

            float fx2 = distX * Mathf.Cos(Rotation * Mathf.Deg2Rad);
            float fy2 = distY * Mathf.Sin(Rotation * Mathf.Deg2Rad);
            // Cam2.transform.position = new Vector3(player2.position.x + fx2, player2.position.y - fy2, -10);

            //gets the different positions the cameras can be in, 
            //converged pos is the very center one basically when the players are both in range
            Vector3 P1SplitPos = new Vector3(player1.transform.position.x - fx1, player1.transform.position.y + fy1, -10);
            Vector3 P2SplitPos = new Vector3(player2.transform.position.x + fx2, player2.transform.position.y - fy2, -10);
            Vector3 ConvergedPos = new Vector3((player1.transform.position.x + player2.transform.position.x) / 2, (player1.transform.position.y + player2.transform.position.y) / 2, -10);

        // Blends the two transforms so it doesnt feel trash    
        Cam1.transform.position = Vector3.Lerp(ConvergedPos, P1SplitPos, distRange);
        Cam2.transform.position = Vector3.Lerp(ConvergedPos, P2SplitPos, distRange);
        // sets opacity in the shader 
        mat.SetFloat("Split", Mathf.Clamp(distRange, 0, 1));
        
        
        //Changes Resolution when screen is resized basically
        if (camRT1.width != Screen.width || camRT1.height != Screen.height)
        {
            ChangeRes();
        }    
    }
  
    //resizes render to textures, this can be hopefully passed throughw henever resolution or aspect ratio changes occur
    public void ChangeRes()
    {
        //camRT1.width = 520;
        camRT1 = new RenderTexture(Screen.width, Screen.height, 16);
        camRT2 = new RenderTexture(Screen.width, Screen.height, 16);
        mat.SetTexture("Cam1", camRT1);
        mat.SetTexture("Cam2", camRT2);
        Cam1.targetTexture = camRT1;
        Cam2.targetTexture = camRT2;
    }
}
