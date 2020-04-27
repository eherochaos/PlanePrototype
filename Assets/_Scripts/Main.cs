using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets._Scripts
{
    using Assets.Utils;

    public class Main : MonoBehaviour
    {
        public static Main S;

        public static Dictionary<WeaponType, WeaponDefinition> WeaponDic;

        public GameObject[] PrefabEnemies;

        public float EnemySpawnPerSecond = 0.5f;

        public float EnemySpawnPadding = 1.5f;

        public WeaponDefinition[] WeaponDefinitions;

        public GameObject PrefabPowerUp;

        public WeaponType[] PowerUpFrequency = new WeaponType[]
           {
               WeaponType.Blaster,
               WeaponType.Blaster,
               WeaponType.Spread,
               WeaponType.Shield
           };

        public bool ________________________________;

        public WeaponType[] ActiveWepTypes;

        [SerializeField]
        private float enemySpawnRate;

        private void Awake()
        {
            S = this;
            Utils.SetCameraBounds(this.GetComponent<Camera>());
            this.enemySpawnRate = 1f / this.EnemySpawnPerSecond;
            this.Invoke("SpawnEnemy",this.enemySpawnRate);

            WeaponDic = new Dictionary<WeaponType, WeaponDefinition>();
            foreach (var def in this.WeaponDefinitions)
            {
                WeaponDic[def.Type] = def;
            }
        }


        public static WeaponDefinition GetWeaponDefinition(WeaponType wt)
        {
            if (WeaponDic.ContainsKey(wt)) return WeaponDic[wt];

            return new WeaponDefinition();
        }

        private void Start()
        {
            this.ActiveWepTypes = new WeaponType[this.WeaponDefinitions.Length];
            for (var i = 0; i < this.WeaponDefinitions.Length; i++)
            {
                this.ActiveWepTypes[i] = this.WeaponDefinitions[i].Type;
            }
        }
        public void SpawnEnemy()
        {
            int ndx = Random.Range(0, this.PrefabEnemies.Length);
            var go = Instantiate(this.PrefabEnemies[ndx]) as GameObject;

            var pos = Vector3.zero;
            var xMin = Utils.CamBounds.min.x + this.EnemySpawnPadding;
            var xMax = Utils.CamBounds.max.x - this.EnemySpawnPadding;
            pos.x = Random.Range(xMin, xMax);
            pos.y = Utils.CamBounds.max.y + this.EnemySpawnPadding;
            go.transform.position = pos;
            this.Invoke("SpawnEnemy",this.enemySpawnRate);
        }


        public void ShipDestroyed(Vector3 pos, float dropChance)
        {
            if (!(Random.value <= dropChance)) return;

            var selectRandom = Random.Range(0, this.PowerUpFrequency.Length);
            var powerUpType = this.PowerUpFrequency[selectRandom];
            var go = Instantiate(this.PrefabPowerUp) as GameObject;
            var powerUp = go.GetComponent<PowerUp>();
            powerUp.SetType(powerUpType);
            powerUp.transform.position = pos;
        }

        public void DelayedRestart(float delay)
        {
            this.Invoke("Restart",delay);
        }

        public void Restart()
        {
            SceneManager.LoadScene("_Scene");
        }


    }
}
