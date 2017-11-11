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
        private Character2D SelectedTower;

        private void Start()
        {
            StartCoroutine(SpawnInTime());
        }

        private IEnumerator SpawnInTime()
        {
            yield return new WaitForSeconds(0.2f);
            Instantiate(TowerPrefab, transform.position, Quaternion.identity);
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
            if (Input.GetMouseButtonDown(0))
            {
                // get tile selected
                // is tower on this tile? if so, select new tower
                
                // if it is a wall, show its health

                // if it is ground, show the build options
                
            }
        }

        private void DeSelectTower()
        {

        }

        private void SelectTower(GameObject Tower)
        {

        }
    }

}