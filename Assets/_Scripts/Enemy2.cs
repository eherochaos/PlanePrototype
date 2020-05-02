// ----------------------------------------------------------------------------
// <author>MaZiJun</author>
// <date>26/04/2020</date>
// ----------------------------------------------------------------------------

namespace Assets._Scripts
{
    using Assets.Utils;

    using UnityEngine;

    public class Enemy2:Enemy
    {
        public Vector3[] Points;

        public float BirthTime;

        [SerializeField]
        private float lifeTime = 10;
        
        [SerializeField]
        private float sinEccentricity = 0.2f;

        private void Start()
        {
            this.Points = new Vector3[2];

            var cameraBoundMin = Utils.CamBounds.min;
            var cameraBoundMax = Utils.CamBounds.max;
            var v = Vector3.zero;

            v.x = cameraBoundMin.x - this.EnemySpawnPaddingX;
            v.y = Random.Range(cameraBoundMin.y, cameraBoundMax.y);
            this.Points[0] = v;

            v = Vector3.zero;
            v.x = cameraBoundMax.x + this.EnemySpawnPaddingX;
            v.y = Random.Range(cameraBoundMin.y, cameraBoundMax.y);
            this.Points[1] = v;

            if (Random.value < 0.5f)
            {
                this.Points[0].x *= -1;
                this.Points[1].x *= -1;
            }

            this.BirthTime = Time.time;
        }

        public override void Move()
        {
            var u = (Time.time - this.BirthTime) / this.lifeTime;

            if (u > 1)
            {
                Destroy(this.gameObject);
                return;
            }

            u += this.sinEccentricity * (Mathf.Sin(u * Mathf.PI * 2));

            this.Pos = (1 - u) * this.Points[0] + u * this.Points[1];
        }
    }
}