using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Zeltex2D.TowerDefence
{
    public class HelpButton : MonoBehaviour
    {
        public GameObject HelpTab;
        public GameObject BuildTab;
        public GameObject CharacterTab;
        public GameObject CharacterTabLabel;
        public Text HelpText;
        public Text BuildText;
        public Text BuildLabelText;
        public TowerBuilder MyTowerBuilder;
        public Character2DGui MyGui;

        private void Awake()
        {

        }

        private void DeactivateAll()
        {
            HelpTab.SetActive(false);
            CharacterTab.SetActive(false);
            BuildTab.SetActive(false);
        }

        public void SelectHelp()
        {
            DeactivateAll();
            HelpTab.SetActive(true);
            BuildLabelText.text = "Help";
            //MyTowerBuilder.SelectTower(null, true);
        }

        public void SelectSelect()
        {
            DeactivateAll();
            CharacterTab.SetActive(true);
            MyTowerBuilder.MyActionType = TowerBuilder.ActionType.Select;
            MyGui.OnNameUpdated();
        }

        public void SelectBuild()
        {
            DeactivateAll();
            BuildTab.SetActive(true);
            //MyTowerBuilder.SelectTower(null, true);
            MyTowerBuilder.MyActionType = TowerBuilder.ActionType.Build;
            SelectTowerType(MyTowerBuilder.TowerPrefabIndex);
        }

        public void SelectTowerType(int TowerType)
        {
            MyTowerBuilder.TowerPrefabIndex = TowerType;
            List<string> TowerNames = new List<string>();
            TowerNames.Add("Piercer");
            TowerNames.Add("Cannon");
            TowerNames.Add("Aura");
            BuildLabelText.text = "Build [" + TowerNames[TowerType] + "]";
        }
    }

}