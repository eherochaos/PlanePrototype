using UnityEngine;

namespace Assets._Scripts
{
    using Assets.Utils;

    public class Projectile : MonoBehaviour
    {
        [SerializeField]
        private WeaponType _type;

        private Renderer Renderer => this.GetComponent<Renderer>();

        private Collider Collider => this.GetComponent<Collider>();

        public WeaponType Type
        {
            get => this._type;
            set => this.SetType(value);
        }
        // Start is called before the first frame update
        private void Awake()
        {
            this.InvokeRepeating("CheckOffscreen",2f,2f);
        }

        public void SetType(WeaponType eType)
        {
            this._type = eType;
            var def = Main.GetWeaponDefinition(this._type);
            this.Renderer.material.color = def.ProjectileColor;
        }

        private void Checkoffscreen()
        {
            if (Utils.ScreenBoundsCheck(this.Collider.bounds, BoundsTest.OffScreen) != Vector3.zero)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
