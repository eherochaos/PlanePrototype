using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    public static Hero S;
    public float speed = 30;

    public float rollMult = -45;

    public float pitchMult = 30;

    //飞船状态信息
    public float shieldLevel = 1;

    public Bounds Bounds;

    [SerializeField]
    private Vector3 off;
    private void Awake()
    {
        S = this;
        this.Bounds = Utils.CombineBoundsOfChildren(this.gameObject);
    }

    // Update is called once per frame
    private void Update()
    {
        var xAxis = Input.GetAxis("Horizontal");
        var yAxis = Input.GetAxis("Vertical");
        var pos = transform.position;
        pos.x += xAxis * this.speed * Time.deltaTime;
        pos.y += yAxis * this.speed * Time.deltaTime;
        this.transform.position = pos;
        Bounds.center = transform.position;
        off = Utils.ScreenBoundsCheck(this.Bounds, BoundsTest.OffScreen);
        if (off != Vector3.zero)
        {
            pos -= off;
            transform.position = pos;
        }

        transform.rotation = Quaternion.Euler(yAxis*this.pitchMult,xAxis*this.rollMult,0);
    }

    

}
