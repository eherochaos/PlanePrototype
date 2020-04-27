// ----------------------------------------------------------------------------
// <author>MaZiJun</author>
// <date>26/04/2020</date>
// ----------------------------------------------------------------------------

namespace Assets._Scripts
{
    using System;

    using Assets.Utils;

    using UnityEngine;

    using Random = UnityEngine.Random;

    public class Enemy4:Enemy
    {
        public Vector3[] Points;

        public float TimeStart;

        public float Duration = 4;

        public Part[] Parts;

        private void Start()
        {
            this.Points = new Vector3[2];

            this.Points[0] = this.Pos;
            this.Points[1] = this.Pos;
            this.InitMovement();
            Transform t;
            foreach (var part in this.Parts)
            {
                t = this.transform.Find(part.Name);
                if (t == null) continue;
                part.Go = t.gameObject;
                part.Mat = part.Go.GetComponent<Renderer>().material;
            }

        }

        private void InitMovement()
        {
            var p1 = Vector3.zero;
            var enemySpawnPadding = Main.S.EnemySpawnPadding;
            var camBounds = Utils.CamBounds;

            p1.x = Random.Range(camBounds.min.x + enemySpawnPadding, camBounds.max.x - enemySpawnPadding);
            p1.y = Random.Range(camBounds.min.y + enemySpawnPadding, camBounds.max.y - enemySpawnPadding);

            this.Points[0] = this.Points[1];
            this.Points[1] = p1;

            this.TimeStart = Time.time;
        }

        public override void Move()
        {
            var u = (Time.time - this.TimeStart) / this.Duration;
            if (u >= 1)
            {
                this.InitMovement();
                u = 0;
            }

            u = 1 - Mathf.Pow(1 - u, 2);

            this.Pos = (1 - u) * this.Points[0] + u * this.Points[1];
        }

        private void OnCollisionEnter(Collision coll)
        {
            var other = coll.gameObject;
            switch (other.tag)
            {
                case "ProjectileHero":
                    var p = other.GetComponent<Projectile>();
                    this.Bounds.center = this.transform.position + this.BoundsCenterOffset;
                    if (this.Bounds.extents == Vector3.zero
                        || Utils.ScreenBoundsCheck(this.Bounds, BoundsTest.OffScreen) != Vector3.zero)
                    {
                        Destroy(other);
                        break;
                    }

                    //查找碰撞体连接物
                    var goHit = coll.contacts[0].thisCollider.gameObject;
                    var partHit = this.FindPart(goHit);
                    // if (partHit == null)
                    // {
                    //     //有可能第一碰撞物不是敌人（不过这段代码在碰撞机制中不会发生
                    //     goHit = coll.contacts[0].otherCollider.gameObject;
                    //     partHit = this.FindPart(goHit);
                    // }

                    if (partHit.ProtectedBy != null)
                    {
                        foreach (var s in partHit.ProtectedBy)
                        {
                            if (this.Destroyed(s)) continue;
                            //炮弹消失
                            Destroy(other);
                            return;
                        }

                        partHit.Health -= Main.WeaponDic[p.Type].DamageOnHit;
                        this.ShowLocalizedDamage(partHit.Mat);
                        //部件生命值归零时
                        if (partHit.Health <= 0)
                        {
                            partHit.Go.SetActive(false);
                        }

                        var allDestroyed = true;
                        foreach (var part in this.Parts)
                        {
                            if (this.Destroyed(part)) continue;
                            allDestroyed = false;
                            break;
                        }

                        if (allDestroyed)
                        {
                            Main.S.ShipDestroyed(this.Pos,this.PowerUpDropChance);
                            Destroy(this.gameObject);
                        }

                        Destroy(other);
                        break;
                    }
                    
                    break;
                
            }
        }

        private Part FindPart(string n)
        {
            foreach (var part in this.Parts)
            {
                if (part.Name == n) return part;
            }

            return null;
        }

        private Part FindPart(GameObject go)
        {
            foreach (var part in this.Parts)
            {
                if (part.Go == go) return part;
            }

            return null;
        }

        private bool Destroyed(GameObject go) => this.Destroyed(this.FindPart(go));

        private bool Destroyed(string n) => this.Destroyed(this.FindPart(n));

        private bool Destroyed(Part part)
        {
            if (part == null) return true;
            return part.Health <= 0;
        }

        private void ShowLocalizedDamage(Material m)
        {
            m.color = Color.red;
            this.RemainingDamageFrames = this.ShowDamageForFrames;
        }

    }

    [Serializable]
    public class Part
    {
        public string Name;

        public float Health;

        public string[] ProtectedBy;

        [HideInInspector]
        public GameObject Go;

        [HideInInspector]
        public Material Mat;
    }
    
}