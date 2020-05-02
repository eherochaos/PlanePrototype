using UnityEngine;

namespace Assets._Scripts
{
    using System;
    using System.ComponentModel;

    using Assets.Utils;

    public class Enemy1 : Enemy
    {
        public bool _________________________________;

        [SerializeField]
        private float waveFrequency = 2;

        [SerializeField]
        private float waveWidth = 4;

        [SerializeField]
        private float waveRotY = 45;

        private float x0 = -12345;

        private float birthTime;

        void Start()
        {
            this.x0 = Pos.x;
            this.birthTime = Time.time;
        }

        public override void Move()
        {
            Vector3 tempPos = this.Pos;

            var age = Time.time - this.birthTime;

            //左右偏移
            var theta = Mathf.PI * 2 * age / this.waveFrequency;

            var sin = Mathf.Sin(theta);

            //sin是一个 0->1->-1的区间
            tempPos.x = this.x0 + this.waveWidth * sin;

            this.Pos = tempPos;

            //旋转
            var rot = new Vector3(0, sin * this.waveRotY, 0);
            this.transform.rotation = Quaternion.Euler(rot);

            base.Move();
        }

    }
}
