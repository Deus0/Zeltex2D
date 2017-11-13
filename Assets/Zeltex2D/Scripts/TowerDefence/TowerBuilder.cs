using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Zeltex2D.TowerDefence
{
    /// <summary>
    /// 
    /// </summary>
    [RequireComponent(typeof(Character2D))]
    public class TowerBuilder : UserControl2D
    {
        [Header("Tower Builder")]
        public List<GameObject> TowerPrefabs;
        public List<int> GoldCosts;
        public MapData Map;
        private Character2D SelectedTower;
        private List<Character2D> SpawnedTowers = new List<Character2D>();
        private Rigidbody2D MyRigid;

        public Text GoldText;
        public Text TowersText;
        //public GameObject TestingMonsterPrefab;

        [Header("Stats")]
        public int GoldCoins = 5;
        private FoW.FogOfWar MyFog;
        private bool CanBuild;
        public Character2DGui SelectedGui;
        public SpawnZone MySpawner;

        public ActionType MyActionType = ActionType.Select;
        [HideInInspector]
        public int TowerPrefabIndex = -1;
        public SpriteRenderer PreviewSprite;

        public enum ActionType
        {
            Select,
            Build,
            None
        }

        private void Start()
        {
            TowerPrefabIndex = -1;
            MyFog = Camera.main.GetComponent<FoW.FogOfWar>();
            MyRigid = GetComponent<Rigidbody2D>();
            OnGoldCoinsChanged();
        }

        public void OnBeginGame()
        {
            CanBuild = true;
            TowerPrefabIndex = 0;
        }

        public void AddGold(int GoldAddition)
        {
            GoldCoins += GoldAddition;
            OnGoldCoinsChanged();
        }

        public override void SelectCharacter(Character2D NewSelectedCharacter)
        {
            base.SelectCharacter(NewSelectedCharacter);
            if (MyCharacter && MyCharacter.CanMove && NewSelectedCharacter)
            {
                Debug.Log(name + " is now battling with " + NewSelectedCharacter.name);
                // Start a combat area
                /*if (MyCombatArea)
                {
                    List<Character2D> InvolvedCharacters = new List<Character2D>();
                    InvolvedCharacters.Add(MyCharacter);
                    InvolvedCharacters.Add(NewSelectedCharacter);
                    MyCombatArea.EnterCombat(InvolvedCharacters);
                }*/
            }
        }

        protected override void Update()
        {
            base.Update();  // for now !
            PreviewSprite.enabled = false;
            if (Time.timeScale != 0)
            {
                if (Input.GetMouseButtonDown(0) && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
                {
                    if (MyActionType == ActionType.Select)
                    {
                        ActionSelect();
                    }
                    else if (MyActionType == ActionType.Build)
                    {
                        ActionBuild();
                    }
                }
                else if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
                {
                    if (MyActionType == ActionType.Build)
                    {
                        PreviewBuild();
                    }
                    else if (MyActionType == ActionType.Select)
                    {
                        PreviewSelect();
                    }
                }
                CheckForNullTowers();
                if (CanBuild && SpawnedTowers.Count == 0)
                {
                    GameManager.Instance.GameOver();
                }
            }
        }

        private void PreviewSelect()
        {
            if (CanBuild)
            {
                Vector3 SpawnPosition = GetMousePositionInMap();
                Character2D HighlightedCharacter = GetTowerAtPosition(SpawnPosition);
                if (HighlightedCharacter != null && HighlightedCharacter != SelectedTower)//new Vector3(Mathf.CeilToInt(SpawnPosition.x), Mathf.CeilToInt(SpawnPosition.y), 0)) == (byte)255))
                {
                    PreviewSprite.transform.position = SpawnPosition;
                    PreviewSprite.enabled = true;
                }
            }
        }

        private void PreviewBuild()
        {
            if (CanBuild)
            {
                Vector3 SpawnPosition = GetMousePositionInMap();
                if (GoldCoins >= GoldCosts[TowerPrefabIndex])
                {
                    if (!(MyFog.IsInCompleteFog(new Vector3(SpawnPosition.x - 0.5f, SpawnPosition.y - 0.5f, 0)))
                        && GetTowerAtPosition(SpawnPosition) == null)//new Vector3(Mathf.CeilToInt(SpawnPosition.x), Mathf.CeilToInt(SpawnPosition.y), 0)) == (byte)255))
                    {
                        PreviewSprite.transform.position = SpawnPosition;
                        PreviewSprite.enabled = true;
                    }
                }
            }
        }

        private void ActionBuild()
        {
            if (CanBuild)
            {
                // get tile selected
                // is tower on this tile? if so, select new tower

                // if it is a wall, show its health

                // if it is ground, show the build options
                Vector3 SpawnPosition = GetMousePositionInMap();
                if (GoldCoins >= GoldCosts[TowerPrefabIndex])
                {
                    if (!(MyFog.IsInCompleteFog(new Vector3(SpawnPosition.x - 0.5f, SpawnPosition.y - 0.5f, 0)))
                        && GetTowerAtPosition(SpawnPosition) == null)//new Vector3(Mathf.CeilToInt(SpawnPosition.x), Mathf.CeilToInt(SpawnPosition.y), 0)) == (byte)255))
                    {
                        SpawnTower(SpawnPosition);
                        GoldCoins -= GoldCosts[TowerPrefabIndex];    // minus cost of it
                        OnGoldCoinsChanged();
                    }
                }
            }
        }

        private void ActionSelect()
        {
            // get tile selected
            // is tower on this tile? if so, select new tower

            // if it is a wall, show its health

            // if it is ground, show the build options

            // If building mode, build selected tower
            /*if (GoldCoins >= GoldCost)
            {
                Vector3 SpawnPosition = Camera.main.ScreenPointToRay(Input.mousePosition).origin;
                SpawnPosition.z = 0;
                GameObject MySpawn = Instantiate(TestingMonsterPrefab, SpawnPosition, Quaternion.identity);
                MapData.Instance.SpawnedCharacters.Add(MySpawn);
                GoldCoins -= GoldCost;    // minus cost of it
                OnGoldCoinsChanged();
            }*/
            Vector3 SpawnPosition = GetMousePositionInMap();
            Character2D TowerAtPosition = GetTowerAtPosition(SpawnPosition);
            //if (TowerAtPosition)
            {
                SelectTower(TowerAtPosition);
            }
        }
        private void OnUpdatedTowersCount()
        {
            TowersText.text = "Towers [" + SpawnedTowers.Count + "]";
        }

        private void CheckForNullTowers()
        {
            for (int i = SpawnedTowers.Count - 1; i >= 0; i--)
            {
                if (SpawnedTowers[i] == null)
                {
                    SpawnedTowers.RemoveAt(i);
                    OnUpdatedTowersCount();
                }
            }
        }

        public void SelectTower(Character2D TowerAtPosition, bool IsForce = false)
        {
            if (SelectedTower)
            {
                SelectedTower.RemoveGlow();
            }
            SelectedTower = TowerAtPosition;
            if (SelectedTower)
            {
                SelectedTower.Glow();
            }
            SelectedGui.SelectCharacter(SelectedTower, IsForce);
        }

        public GameObject SpawnTower(Vector3 SpawnPosition)
        {
            GameObject MySpawn = Instantiate(TowerPrefabs[TowerPrefabIndex + 1], SpawnPosition, Quaternion.identity);
            SpawnedTowers.Add(MySpawn.GetComponent<Character2D>());
            OnUpdatedTowersCount();
            MapData.Instance.SpawnedCharacters.Add(MySpawn);
            MySpawn.transform.SetParent(GameObject.Find("CaveLevel").transform);
            SpawnedTowers[SpawnedTowers.Count - 1].OnKilledCharacterEvent.AddListener(OnKilledCharacter);
            return MySpawn;
        }

        private void OnKilledCharacter(Character2D DeadCharacter)
        {
            int GoldAdd = DeadCharacter.GetGoldCoinsDrop();
            GoldCoins += GoldAdd;// Mathf.FloorToInt(Random.Range(0, 1.99f));
            OnGoldCoinsChanged();
            if (GoldAdd > 0)
            {
                MapData.Instance.CreateHealthPopup(DeadCharacter.transform.position, GoldAdd, 1);
            }
        }

        private Vector3 GetMousePositionInMap()
        {
            Vector3 SpawnPosition = Camera.main.ScreenPointToRay(Input.mousePosition).origin;
            SpawnPosition.y = Mathf.RoundToInt(SpawnPosition.y - 0.5f) + 0.5f;
            SpawnPosition.x = Mathf.RoundToInt(SpawnPosition.x - 0.5f) + 0.5f;
            SpawnPosition.z = 0;
            return SpawnPosition;
        }

        private Character2D GetTowerAtPosition(Vector3 SpawnPosition)
        {
            for (int i = SpawnedTowers.Count - 1; i >= 0; i--)
            {
                if (SpawnedTowers[i] == null)
                {
                    SpawnedTowers.RemoveAt(i);
                    OnUpdatedTowersCount();
                }
                else
                {
                    if (SpawnedTowers[i].transform.position.x == SpawnPosition.x &&
                        SpawnedTowers[i].transform.position.y == SpawnPosition.y)
                    {
                        return SpawnedTowers[i];
                    }
                }
            }
            return null;
        }

        private void OnGoldCoinsChanged()
        {
            // update ui for gold
            if (GoldText)
            {
                GoldText.text = GoldCoins + "g";
            }
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            ClampPosition();
        }
        
        private void ClampPosition()
        {
            if (CanInput)
            {
                GetComponent<Character2D>().SetMovement(true);
                if (transform.position.x > Map.MapWidth / 2f)
                {
                    MyRigid.velocity.Set(-1, MyRigid.velocity.y);
                    GetComponent<Character2D>().SetMovement(false);
                }
                if (transform.position.x < -Map.MapWidth / 2f)
                {
                    MyRigid.velocity.Set(1, MyRigid.velocity.y);
                    GetComponent<Character2D>().SetMovement(false);
                }
                if (transform.position.y > Map.MapHeight / 2f)
                {
                    MyRigid.velocity.Set(MyRigid.velocity.x, -1);
                    GetComponent<Character2D>().SetMovement(false);
                }
                if (transform.position.y < -Map.MapHeight / 2f)
                {
                    MyRigid.velocity.Set(MyRigid.velocity.x, 1);
                    GetComponent<Character2D>().SetMovement(false);
                }
                transform.position = new Vector3(
                    Mathf.Clamp(transform.position.x, -Map.MapWidth / 2f, Map.MapWidth / 2f),
                    Mathf.Clamp(transform.position.y, -Map.MapHeight / 2f, Map.MapHeight / 2f), 
                    transform.position.z);
            }
        }

        private void DeSelectTower()
        {

        }

        private void SelectTower(GameObject Tower)
        {

        }

        public void CenterTower()
        {
            if (SelectedTower)
            {
                transform.position = SelectedTower.transform.position;
            }
        }

        public void BuildTower()
        {

        }
    }

}