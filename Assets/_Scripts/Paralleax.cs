using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paralleax : MonoBehaviour
{
    public GameObject Poi;

    public GameObject[] Panels;

    public float ScrollSpeed = -30f;

    public float MotionMult = 0.25f;

    private float panelHeight;

    private float depth;

    // Start is called before the first frame update
    void Start()
    {
        this.panelHeight = this.Panels[0].transform.localScale.y;
        this.depth = this.Panels[0].transform.position.z;

        this.Panels[0].transform.position = new Vector3(0,0,this.depth);
        this.Panels[1].transform.position =new Vector3(0,this.panelHeight,this.depth);

    }

    // Update is called once per frame
    void Update()
    {
        float tY, tX = 0;
        tY = Time.time * this.ScrollSpeed % this.panelHeight + (this.panelHeight * 0.5f);
        if (this.Poi != null)
        {
            //左右位置一定程度受玩家控制
            tX = -this.Poi.transform.position.x * this.MotionMult;
        }
        this.Panels[0].transform.position = new Vector3(tX,tY,this.depth);
        if (tY >= 0)
        {
            this.Panels[1].transform.position = new Vector3(tX,tY - this.panelHeight,this.depth);
        }
        else
        {
            this.Panels[1].transform.position = new Vector3(tX, tY + this.panelHeight, this.depth);
        }
    }
}
