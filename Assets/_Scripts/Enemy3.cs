// ----------------------------------------------------------------------------
// <author>MaZiJun</author>
// <date>26/04/2020</date>
// ----------------------------------------------------------------------------

namespace Assets._Scripts
{
    using Assets.Utils;

    using UnityEngine;

    public class Enemy3:Enemy
    {
        public Vector3[] Points;

        public float BirthTime;

        public float LifeTime = 10;

        private void Start()
        {
            this.Points =new Vector3[3];

            this.Points[0] = this.Pos;

            var xMin = Utils.CamBounds.min.x + this.EnemySpawnPaddingX;

            var xMax = Utils.CamBounds.max.x + this.EnemySpawnPaddingX;

            Vector3 v;

            v = Vector3.zero;
            v.x = Random.Range(xMin, xMax);
            v.y = Random.Range(Utils.CamBounds.min.y, 0);
            this.Points[1] = v;

            v = Vector3.zero;
            v.y = this.Pos.y;
            v.x = Random.Range(xMin, xMax);
            this.Points[2] = v;

            this.BirthTime = Time.time;
        }

        public override void Move()
        {
            float u = (Time.time - this.BirthTime) / this.LifeTime;

            if (u > 1)
            {
                Destroy(this.gameObject);
                return;
            }

            Vector3 p01, p12;
            u -= 0.2f * Mathf.Sin(u * Mathf.PI * 2);
            p01 = (1 - u) * this.Points[0] + u * this.Points[1];
            p12 = (1 - u) * this.Points[1] + u * this.Points[2];
            this.Pos = (1 - u) * p01 + u * p12;
        }

    }
}