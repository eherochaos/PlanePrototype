namespace Assets._Scripts
{
    using System.Linq;

    using Assets.Utils;

    using UnityEngine;

    public class Hero : MonoBehaviour
    {
        public static Hero S;

        public float GameRestartDelay = 2f;

        public float Speed = 30;

        public float RollMult = -45;

        public float PitchMult = 30;

        //飞船状态信息
        public float ShieldLevel
        {
            get => this.shieldLevel;
            set
            {
                this.shieldLevel = Mathf.Min(value, 4);
                if (value < 0)
                {
                    Destroy(this.gameObject);

                    //游戏重开
                }
            }

        }
        [SerializeField]
        private float shieldLevel = 1;

        public Weapon[] Weapons;

        public Bounds Bounds;

        public delegate void WeaponFireDelegate();

        public WeaponFireDelegate FireDelegate;

        public bool ______________________________;
        [SerializeField]
        private Vector3 off;

        [SerializeField]
        private GameObject lastTriggerGo = null;
        private void Awake()
        {
            S = this;
            this.Bounds = Utils.CombineBoundsOfChildren(this.gameObject);

            
        }

        private void Start()
        {
            this.ClearWeapons();
            this.Weapons[0].SetType(WeaponType.Blaster);
        }

        // Update is called once per frame
        private void Update()
        {
            var xAxis = Input.GetAxis("Horizontal");
            var yAxis = Input.GetAxis("Vertical");
            var pos = this.transform.position;
            pos.x += xAxis * this.Speed * Time.deltaTime;
            pos.y += yAxis * this.Speed * Time.deltaTime;
            this.transform.position = pos;
            this.Bounds.center = this.transform.position;
            this.off = Utils.ScreenBoundsCheck(this.Bounds, BoundsTest.OnScreen);
            if (this.off != Vector3.zero)
            {
                pos -= this.off;
                this.transform.position = pos;
            }

            this.transform.rotation = Quaternion.Euler(yAxis*this.PitchMult,xAxis*this.RollMult,0);

            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (Input.GetAxis("Jump") == 1)
            {
                this.FireDelegate?.Invoke();
            }
        }

        //碰撞
        private void OnTriggerEnter(Collider other)
        {
            var go = Utils.FindTaggedParent(other.gameObject);
            if (go != null)
            {
                if(go == this.lastTriggerGo) return;
                this.lastTriggerGo = go;
                if (go.tag == "Enemy")
                {
                    this.ShieldLevel--;
                    Destroy(go);
                }
                else if(go.tag == "PowerUp")
                {
                    this.AbsorbPowerUp(go);
                }
                else
                {
                    print("触发碰撞事件：" + go.name);
                }
            
            }
            else
            {
                print("触发碰撞事件：" + other.gameObject.name);
            }

        }

        public void AbsorbPowerUp(GameObject go)
        {
            var pu = go.GetComponent<PowerUp>();
            switch (pu.Type)
            {
                case WeaponType.Shield:
                    this.ShieldLevel++;
                    break;

                default:
                    if (pu.Type == this.Weapons[0].Type)
                    {
                        var w = this.GetEmptyWeaponSlot;
                        if (w != null) w.SetType(pu.Type);
                    }
                    else
                    {
                            this.ClearWeapons();
                            this.Weapons[0].SetType(pu.Type);
                    }
                    
                    break;
            }
            pu.AbsorbedBy(this.gameObject);
        }

        private Weapon GetEmptyWeaponSlot => this.Weapons.FirstOrDefault(t => t.Type == WeaponType.None);

        private void ClearWeapons()
        {
            foreach (var w in this.Weapons)
            {
                w.SetType(WeaponType.None);
            }
        }
    

    }
}
