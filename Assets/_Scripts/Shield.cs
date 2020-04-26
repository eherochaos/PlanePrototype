using UnityEngine;

namespace Assets._Scripts
{
    public class Shield : MonoBehaviour
    {
        public float RotationsPerSecond = 0.1f;

        public bool ______________;

        public int LevelShown = 0;

        // Update is called once per frame
        private void Update()
        {
            var currentLevel = Mathf.FloorToInt(Hero.S.ShieldLevel);

            if (this.LevelShown != currentLevel)
            {
                this.LevelShown = currentLevel;
                var mat = this.GetComponent<Renderer>().material;
                mat.mainTextureOffset = new Vector2(0.2f*this.LevelShown,0);
            }

            var rZ = (this.RotationsPerSecond * Time.time * 360) % 360f;
            this.transform.rotation = Quaternion.Euler(0,0,rZ);

        }
    }
}
