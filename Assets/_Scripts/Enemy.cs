using UnityEngine;

namespace Assets._Scripts
{
    using Assets.Utils;

    public class Enemy : MonoBehaviour
    {
        public float Speed = 10f;

        public float FireRate = 0.3f;

        public float Health = 10;

        public int Score = 100;

        public int ShowDamageForFrames = 4;

        public float PowerUpDropChance = 1f;

        public bool ________________________________;

        public Color[] OriginalColors;

        public Material[] Materials;

        public int RemainingDamageFrames = 0;

        public Bounds Bounds;

        public Vector3 BoundsCenterOffset;

        private void Update()
        {
            this.Move();
            if (this.RemainingDamageFrames > 0)
            {
                this.RemainingDamageFrames--;
                if (this.RemainingDamageFrames == 0) this.UnShowDamage();

            }
        }

        public virtual void Move()
        {
            var temPos = this.Pos;
            temPos.y -= this.Speed * Time.deltaTime;
            this.Pos = temPos;
        }

        public Vector3 Pos
        {
            get => this.transform.position;

            set => this.transform.position = value;
        }

        void Awake()
        {
            this.Materials = Utils.GetAllMaterials(this.gameObject);
            this.OriginalColors = new Color[this.Materials.Length];
            for (var i = 0; i < this.Materials.Length; i++)
            {
                this.OriginalColors[i] = this.Materials[i].color;
            }
            this.InvokeRepeating("CheckOffscreen",0f,2f);
        }

        private void CheckOffscreen()
        {
            if (this.Bounds.size == Vector3.zero)
            {
                this.Bounds = Utils.CombineBoundsOfChildren(this.gameObject);
                this.BoundsCenterOffset = this.Bounds.center - this.transform.position;
            }

            this.Bounds.center = this.transform.position + this.BoundsCenterOffset;

            var off = Utils.ScreenBoundsCheck(this.Bounds, BoundsTest.OffScreen);

            if (off == Vector3.zero) return;

            if (off.y < 0)
            {
                Destroy(this.gameObject);
            }
        }

        private void OnCollisionEnter(Collision coll)
        {
            var other = coll.gameObject;
            switch (other.tag)
            {
                case "ProjectileHero":
                    var p = other.GetComponent<Projectile>();
                    this.Bounds.center = transform.position + this.BoundsCenterOffset;
                    if (this.Bounds.extents == Vector3.zero
                        || Utils.ScreenBoundsCheck(this.Bounds, BoundsTest.OffScreen) != Vector3.zero)
                    {
                        Destroy(other);
                        break;
                    }

                    if(this.RemainingDamageFrames ==0) this.ShowDamage();
                    this.Health -= Main.WeaponDic[p.Type].DamageOnHit;
                    if (this.Health <= 0)
                    {
                        Main.S.ShipDestroyed(this);
                        Destroy((this.gameObject));
                    }
                    Destroy(other);
                    break;
                    
            }
        }

        private void ShowDamage()
        {
            foreach (var m in this.Materials)
            {
                m.color = Color.red;
            }

            this.RemainingDamageFrames = this.ShowDamageForFrames;
        }

        private void UnShowDamage()
        {
            for (var i = 0; i < this.Materials.Length; i++)
            {
                this.Materials[i].color = this.OriginalColors[i];
            }
        }





    }
}
