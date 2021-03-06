using UnityEngine;

namespace _4._Scripts
{
    public class GameManager : MonoBehaviour
    {
        int spawnerAmount_now;
        float _x, _z;

        [SerializeField]
        GameObject SpawnerPrefab;
        [SerializeField]
        private int spawnerAmount;
        // 이후 수정 예정
        [SerializeField] private Inventory inventory;
        [SerializeField] public GameObject TempleSpawners,MapSpawners;

        public void PlayerEnteredTemple()
        {
            TempleSpawners.SetActive(true);
            for (int a = 0; a < TempleSpawners.transform.childCount; a++)
            {
                TempleSpawners.transform.GetChild(a).gameObject.SetActive(true);
                var spawnscript=TempleSpawners.transform.GetChild(a).GetComponent<SpawnerScript>();
                spawnscript.DelayedDestroy(15f);
            }
        }
        
        void Start()
        {
            for (int a = 0; a < MapSpawners.transform.childCount; a++)
            {
                var spawnscript=MapSpawners.transform.GetChild(a).GetComponent<SpawnerScript>();
                spawnscript.SetAmount(2);
                spawnscript.isCrystal = true;
                spawnscript.infSpawn = false;
            }
            TempleSpawners.SetActive(false);
            for (int a = 0; a < TempleSpawners.transform.childCount; a++)
            {
                var spawnscript=TempleSpawners.transform.GetChild(a).GetComponent<SpawnerScript>();
                spawnscript.SetAmount(5);
                spawnscript._Enemyrange = 200f;
                spawnscript.SpawnTime = 3;
                spawnscript.isCrystal = false;
                TempleSpawners.transform.GetChild(a).gameObject.SetActive(false);
            }
            
            // 스포너를 맵에 뿌리는 단계 , 미궁 혹은 다른 모드에서 사용
            {
                
                bool NearSpawner = false;
                // 정해준 수량만큼만 맵 랜덤좌표에 생성.
                // 좌표를 랜덤지정후 근처 가까운곳에 스포너가 없어야 생성
                while (spawnerAmount_now < spawnerAmount) 
                {
                    _x = Random.Range(-45, 45);
                    _z = Random.Range(-45, 45);
                    Vector3 Pos = new Vector3(_x, 0, _z);
                    Collider[] cols = Physics.OverlapSphere(Pos, 10f);
                    for (int i = 0; i < cols.Length; i++)
                    {
                        if (cols[i].tag == "Spawner")
                        {
                            NearSpawner = true;
                        }
                    }
                    if (!NearSpawner)
                    {
                        Instantiate(SpawnerPrefab, Pos, Quaternion.identity);
                        spawnerAmount_now++;
                    }
                    NearSpawner = false;
                }
            }
            
        

        }
        private void Update()
        {
            // ESC로 닫는 기능은 아직 추가하지 않음 (윈도우 스택과 함께 추가할 예정)
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                inventory.ManipulateWindow();
            }

            
        }
    }
}
