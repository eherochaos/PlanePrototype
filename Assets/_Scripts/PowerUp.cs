// ----------------------------------------------------------------------------
// <author>MaZiJun</author>
// <date>24/04/2020</date>
// ----------------------------------------------------------------------------

namespace Assets._Scripts
{
    using Assets.Utils;

    using UnityEngine;

    public class PowerUp:MonoBehaviour
    {
        public Vector2 RotMinMax = new Vector2(15,90);
        public Vector2 DriftMinMax = new Vector2(.25f,2);

        public float LifeTime = 6f;

        public float FadeTime = 4f;

        public bool ________________________________;

        public WeaponType Type;

        public GameObject Cube;

        public TextMesh Letter;

        public Vector3 RotPerSecond;

        public float BirthTime;


        private void Awake()
        { 
            this.Cube = this.transform.Find("Cube").gameObject;

            this.Letter = this.GetComponent<TextMesh>();

            var vel = Random.onUnitSphere;

            vel.z = 0;

            vel.Normalize();//使向量长度变为1

            vel *= Random.Range(this.DriftMinMax.x, this.DriftMinMax.y);

            this.GetComponent<Rigidbody>().velocity = vel;

            this.transform.rotation = Quaternion.identity;;

            var x = Random.Range(this.RotMinMax.x, this.RotMinMax.y);
            var y = Random.Range(this.RotMinMax.x, this.RotMinMax.y);
            var z = Random.Range(this.RotMinMax.x, this.RotMinMax.y);
            this.RotPerSecond = new Vector3(x,y,z);

            this.InvokeRepeating("CheckOffscreen",2f,2f);
            this.BirthTime = Time.time;
        }

        private void Update()
        {
            this.Cube.transform.rotation = Quaternion.Euler(this.RotPerSecond * Time.time);

            var u = (Time.time - this.BirthTime) / this.LifeTime;
            if (u >= 1)
            {
                Destroy(this.gameObject);
                return;
            }

            u = (Time.time - this.BirthTime) / this.FadeTime;
            if (u > 0)
            {
                var rendererComponent = this.Cube.GetComponent<Renderer>();
                var c = rendererComponent.material.color;
                c.a = 1f - u;
                rendererComponent.material.color = c;

                c = this.Letter.color;
                c.a = 1f - (u * 0.5f);
                this.Letter.color = c;
            }
        }

        public void SetType(WeaponType wt)
        {
            WeaponDefinition def = Main.GetWeaponDefinition(wt);

            var rendererComponent = this.Cube.GetComponent<Renderer>();

            rendererComponent.material.color = def.Color;

            this.Letter.text = def.Letter;

            this.Type = wt;
        }

        public void AbsorbedBy(GameObject target)
        {
            //此处可以制作缩小演出
            Destroy(this.gameObject);
        }

        public void CheckOffscreen()
        {

            if (Utils.ScreenBoundsCheck(this.Cube.GetComponent<Collider>().bounds, BoundsTest.OffScreen)
                != Vector3.zero)
            {
                Destroy(this.gameObject);
            }
        }
    }
}