using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zeltex2D.TowerDefence;

namespace Zeltex2D
{
    public class Character2DGui : MonoBehaviour
    {
        public Text NameText;
        public Text HealthText;
        public Text HealthRegenText;
        public Text DefenceText;
        public Text AttackDamageText;
        public Text AttackRangeText;
        public Text AttackSpeedText;
        public Text AttackTypeText;
        public Text VisionText;
        public Text UpgradeAttackDamageText;
        public Text UpgradeHealthText;
        private Character2D SelectedCharacter;
        public TowerBuilder MyBuilder;
        private bool IsSelected;
        public HelpButton MyTabs;

        public void SelectCharacter(Character2D NewCharacter, bool IsForce = false)
        {
            if (SelectedCharacter != NewCharacter || IsForce)
            {
                if (SelectedCharacter != null)
                {
                    SelectedCharacter.OnHealthUpdatedEvent.RemoveListener(OnHealthUpdated);
                    SelectedCharacter.OnHealthRegenUpdatedEvent.RemoveListener(OnHealthRegenUpdated);
                }
                SelectedCharacter = NewCharacter;
                if (SelectedCharacter)
                {
                    MyTabs.SelectSelect();
                    gameObject.SetActive(true);
                    IsSelected = true;
                    SelectedCharacter.OnHealthUpdatedEvent.AddListener(OnHealthUpdated);
                    SelectedCharacter.OnHealthRegenUpdatedEvent.AddListener(OnHealthRegenUpdated);
                    OnNameUpdated();
                    OnHealthUpdated();
                    OnHealthRegenUpdated();
                    OnDamageUpdated();
                    OnAttackSpeedUpdated();
                    OnRangeUpdated();
                    OnVisionUpdated();
                }
                else
                {
                    NameText.text = "None Selected";
                    gameObject.SetActive(false);
                }
            }
        }

        #region OnUpdated

        public void OnNameUpdated()
        {
            if (SelectedCharacter)
            {
                if (SelectedCharacter.name.Contains("Clone"))
                {
                    SelectedCharacter.name = SelectedCharacter.name.Remove(SelectedCharacter.name.IndexOf("(Clone)"));
                }
                NameText.text = SelectedCharacter.name + " [" +
                    Mathf.FloorToInt(SelectedCharacter.transform.position.x) + "," +
                    Mathf.FloorToInt(SelectedCharacter.transform.position.y) + "]";
            }
            else
            {
                NameText.text = "None Selected";
            }
        }

        public void OnHealthUpdated()
        {
            if (SelectedCharacter)
            {
                HealthText.text = "Health [" + SelectedCharacter.Health + "/" + SelectedCharacter.MaxHealth + "]";
            }
        }

        public void OnHealthRegenUpdated()
        {
            HealthRegenText.text = "Regen [" + SelectedCharacter.HealthRegenValue + "]";
        }

        public void OnDamageUpdated()
        {
            AttackDamageText.text = "Damage [" + SelectedCharacter.AttackDamage + "]";
        }

        public void OnAttackSpeedUpdated()
        {
            AttackSpeedText.text = "Attack Speed [" + SelectedCharacter.AttackSpeed + "]";
        }
        public void OnRangeUpdated()
        {
            AttackRangeText.text = "Range [" + SelectedCharacter.Range + "]";
        }
        public void OnVisionUpdated()
        {
            VisionText.text = "Vision [" + SelectedCharacter.Vision + "]";
        }

        #endregion

        #region Upgrades
        public void UpgradeHealth()
        {
            if (SelectedCharacter && MyBuilder.GoldCoins >= 3)
            {
                MyBuilder.AddGold(-3);
                SelectedCharacter.AddMaxHealth(0.5f);
            }
        }
        public void UpgradeHealthRegen()
        {
            if (SelectedCharacter && MyBuilder.GoldCoins >= 3)
            {
                MyBuilder.AddGold(-3);
                SelectedCharacter.AddHealthRegen(0.5f);
            }
        }

        public void UpgradeDamage()
        {
            if (SelectedCharacter && MyBuilder.GoldCoins >= 4)
            {
                MyBuilder.AddGold(-4);
                SelectedCharacter.AddDamage(0.5f);
                OnDamageUpdated();
            }
        }

        public void UpgradeAttackSpeed()
        {
            if (SelectedCharacter && MyBuilder.GoldCoins >= 3)
            {
                MyBuilder.AddGold(-3);
                SelectedCharacter.AddAttackSpeed(0.1f);
                OnAttackSpeedUpdated();
            }
        }

        public void UpgradeRange()
        {
            if (SelectedCharacter && MyBuilder.GoldCoins >= 3)
            {
                MyBuilder.AddGold(-3);
                SelectedCharacter.AddRange(0.5f);
                OnRangeUpdated();
            }
        }

        public void UpgradeVision()
        {
            if (SelectedCharacter && MyBuilder.GoldCoins >= 3)
            {
                MyBuilder.AddGold(-3);
                SelectedCharacter.AddVision(0.5f);
                OnVisionUpdated();
            }
        }

        #endregion


        private void Awake()
        {
            OnHealthUpdated();
        }
        private void Update()
        {
            if (IsSelected && SelectedCharacter == null)
            {
                IsSelected = false;
                SelectCharacter(null, true);
            }
        }
    }

}