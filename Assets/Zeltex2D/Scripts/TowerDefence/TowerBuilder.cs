using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zeltex2D.TowerDefence
{
    /// <summary>
    /// 
    /// </summary>
    [RequireComponent(typeof(Character2D))]
    public class TowerBuilder : UserControl2D
    {
        [Header("Tower Builder")]
        public GameObject TowerPrefab;
        public MapData Map;
        private Character2D SelectedTower;
        private Rigidbody2D MyRigid;

        private void Start()
        {
            MyRigid = GetComponent<Rigidbody2D>();
            StartCoroutine(SpawnInTime());
        }

        private IEnumerator SpawnInTime()
        {
            yield return new WaitForSeconds(0.2f);
            GameObject Tower = Instantiate(TowerPrefab, transform.position, Quaternion.identity);
            SelectedTower = Tower.GetComponent<Character2D>();
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

        public GameObject TestingMonsterPrefab;
        protected override void Update()
        {
            base.Update();  // for now !
            if (Input.GetMouseButtonDown(0))
            {
                // get tile selected
                // is tower on this tile? if so, select new tower

                // if it is a wall, show its health

                // if it is ground, show the build options

                Vector3 SpawnPosition = Camera.main.ScreenPointToRay(Input.mousePosition).origin;
                SpawnPosition.z = 0;
                Instantiate(TestingMonsterPrefab, SpawnPosition, Quaternion.identity);
            }
            if (Input.GetMouseButtonDown(1))
            {
                // get tile selected
                // is tower on this tile? if so, select new tower

                // if it is a wall, show its health

                // if it is ground, show the build options

                Vector3 SpawnPosition = Camera.main.ScreenPointToRay(Input.mousePosition).origin;
                SpawnPosition.z = 0;
                Instantiate(TowerPrefab, SpawnPosition, Quaternion.identity);
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