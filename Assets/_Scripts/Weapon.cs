namespace Assets._Scripts
{
    using System.Collections;
    using System.Collections.Generic;

    using JetBrains.Annotations;

    using UnityEngine;

    public enum WeaponType
    {
        None,
        Blaster,
        Spread,
        Phaser,
        Missile,
        Laser,
        Shield,
    }

    [System.Serializable]
    public class WeaponDefinition
    {
        public WeaponType Type;

        public string Letter;

        public Color Color;

        public GameObject ProjectilePrefab;

        public Color ProjectileColor;

        public float DamageOnHit = 0;

        public float ContinuousDamage = 0;

        public float DelayBetweenShots = 0;

        public float Velocity = 20;
    }

    public class Weapon : MonoBehaviour
    {
        public static Transform ProjectileAnchor;

        public bool ______________________________;

        [SerializeField]
        private WeaponType type = WeaponType.Blaster;

        public WeaponDefinition Def;

        private GameObject Collar => this.collar ?? (this.collar = this.transform.Find("Collar").gameObject);

        private GameObject collar;


        //最后射击时间
        public float LastShot;


        // Start is called before the first frame update

        private void Start()
        {
            
            SetType(this.type);
            if (ProjectileAnchor == null)
            {
                var go = new GameObject("_ProjectileAnchor");
                ProjectileAnchor = go.transform;
            }

            var parentGo = transform.parent.gameObject;
            if (parentGo != null && parentGo.tag == "Hero")
            {
                Hero.S.FireDelegate += this.Fire;
            }
        }

        public WeaponType Type
        {
            get => this.type;
            set => this.SetType(value);

        }

        public void SetType(WeaponType wt)
        {
            this.type = wt;
            if (Type == WeaponType.None)
            {
                this.gameObject.SetActive(false);
                return;
            }
            else
            {
                this.gameObject.SetActive(true);
            }

            this.Def = Main.GetWeaponDefinition(this.type);
            this.Collar.GetComponent<Renderer>().material.color = this.Def.Color;
            this.LastShot = 0;

        }

        public void Fire()
        {
            //检查武器是否可用
            if(!this.gameObject.activeInHierarchy) return;

            //CD间隔
            if(Time.time - this.LastShot < this.Def.DelayBetweenShots) return;

            Projectile p;
            switch (this.Type)
            {
                case WeaponType.Blaster:
                    p = this.MakeProjectile();
                    p.GetComponent<Rigidbody>().velocity = Vector3.up * this.Def.Velocity;
                    break;

                case WeaponType.Spread:
                    p = this.MakeProjectile();
                    p.GetComponent<Rigidbody>().velocity = Vector3.up * this.Def.Velocity;
                    p = this.MakeProjectile();
                    p.GetComponent<Rigidbody>().velocity = new Vector3(-0.2f,0.9f,0) * this.Def.Velocity;
                    p = this.MakeProjectile();
                    p.GetComponent<Rigidbody>().velocity = new Vector3(0.2f, 0.9f, 0) * this.Def.Velocity;
                    break;
                    
            }
        }

        public Projectile MakeProjectile()
        {
            //生成子弹实体
            var go = Instantiate(this.Def.ProjectilePrefab) as GameObject;
            if (this.transform.parent.gameObject.tag == "Hero")
            {
                go.tag = "ProjectileHero";
                go.layer = LayerMask.NameToLayer("ProjectileHero");
            }
            else
            {
                go.tag = "ProjectileEnemy";
                go.layer = LayerMask.NameToLayer("ProjectileEnemy");
            }

            go.transform.position = this.Collar.transform.position;
            go.transform.parent = ProjectileAnchor;
            var p = go.GetComponent<Projectile>();
            p.Type = this.Type;
            this.LastShot = Time.time;
            return p;
        }
    }
}