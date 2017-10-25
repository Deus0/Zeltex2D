using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using SlimeWitch;

namespace Zeltex2D.TurnBased
{
    /// <summary>
    /// Main class to handle turn based combat
    /// </summary>
    public class CombatArea : MonoBehaviour
    {
        public List<CombatSpawner> Spawners = new List<CombatSpawner>();
        [Header("UI")]
        public Text CharacterLabel;
        public List<Button> ActionButtons = new List<Button>();
        public Text AttackingCharacterLabel;

        [Header("CaveLevel")]
        public GameObject CaveLevel;
        public Character2D SelectedCharacter;
        public Character2D PlayerCharacter;
        //public List<Character2D> MyCharacters = new List<Character2D>();
        public List<Character2D> CombatCharacters = new List<Character2D>();
        public int SelectedCharacterIndex = 0;
        // Audio
        [Header("Audio")]
        public AudioClip CombatHitSound;
        private AudioSource MySource;

        [Header("Links")]
        public CanvasFader MyFader;
        public MapData MyMap;

        [Header("Events")]
        public UnityEngine.Events.UnityEvent OnEnterCombat;
        public UnityEngine.Events.UnityEvent OnExitCombat;
        private Coroutine EnterCombatRountine;

        private void Awake()
        {
            MySource = GetComponent<AudioSource>();
            if (MySource == null)
            {
                MySource = gameObject.AddComponent<AudioSource>();
            }
            for (int i = 0; i < ActionButtons.Count; i++)
            {
                int ActionIndex = i;
                ActionButtons[i].onClick.AddListener(() => { DoAction(ActionIndex); });
            }
        }

        public void EnterCombat(List<Character2D> LinkedCharacters)
        {
            if (EnterCombatRountine != null)
            {
                return;
            }
            if (LinkedCharacters.Count > 0)
            {
                for (int i = 0; i < LinkedCharacters.Count; i++)
                {
                    if (LinkedCharacters[i] == null)
                    {
                        Debug.LogError("Character " + i + " is null");
                        return;
                    }
                    if (LinkedCharacters[i].Health <= 0)
                    {
                        Debug.LogError("Character " + LinkedCharacters[i].name + " has no hp");
                        return;
                    }
                }
                CombatCharacters = LinkedCharacters;
                SelectedCharacter = CombatCharacters[0];
                PlayerCharacter = SelectedCharacter;
                AttackingCharacterLabel.text = SelectedCharacter.name;
                EnterCombatRountine = MyFader.StartCoroutine(EnterCombatRoutine());
            }
            else
            {
                Debug.LogError("No characters, so cannot enter combat.");
            }
        }

        private IEnumerator EnterCombatRoutine()
        {
            MyMap.SetCharactersMovement(false);
            
            MyFader.SetText(PlayerCharacter.name + "\n-= VS =-\n" + GetNextCharacter().name);
            OnEnterCombat.Invoke(); // play music
            MyFader.ReverseFade();
            yield return new WaitForSeconds(1f);

            // Turn off all characters
            // turn off cavearea
            if (CaveLevel)
            {
                CaveLevel.SetActive(false);
            }

            // turn on combat area
            gameObject.SetActive(true);
            // reposition things!
            for (int i = 0; i < Spawners.Count; i++)
            {
                Spawners[i].Teleport(CombatCharacters[i]);
            }

            yield return new WaitForSeconds(0.1f);
            MyFader.Fade();
            yield return new WaitForSeconds(1f);

            SetButtonsInteractable(true);
            AttackingCharacterLabel.text = PlayerCharacter.name;

            EnterCombatRountine = null;
        }

        public void ExitCombat()
        {
            MyFader.StartCoroutine(ExitCombatRoutine());
        }

        private IEnumerator ExitCombatRoutine()
        {
            if (PlayerCharacter != null)
            {
                MyFader.SetText("Victory");
            }
            else
            {
                MyFader.SetText("Game Over");
            }
            OnEnterCombat.Invoke(); // play music
            MyFader.ReverseFade();
            if (PlayerCharacter != null)
            {
                yield return new WaitForSeconds(1f);
                for (int i = 0; i < Spawners.Count; i++)
                {
                    Spawners[i].RevertTeleport();
                }
                gameObject.SetActive(false);
                if (CaveLevel)
                {
                    CaveLevel.SetActive(true);
                }
                OnExitCombat.Invoke();  // play normal music
                MyFader.Fade();
                yield return new WaitForSeconds(1f);
                MyMap.SetCharactersMovement(true);
            }
        }

        /// <summary>
        /// When a monster is selected, apply an action
        /// </summary>
        public void DoAction(int ActionIndex)
        {
            float AttackDamage = ActionIndex + Random.Range(0, 2);// ActionIndex + Random.Range(ActionIndex, ActionIndex + 2);
            string ActionName = ActionButtons[ActionIndex].transform.GetChild(0).GetComponent<Text>().text;
            Debug.Log(SelectedCharacter.name + " is attacking character: " + GetNextCharacter().name + " for " + AttackDamage + " Damage with " + ActionName + ".");
            StartCoroutine(AttackCharacterRoutine(AttackDamage));
        }

        private IEnumerator AttackCharacterRoutine(float AttackDamage)
        {
            Camera.main.gameObject.GetComponent<FollowCamera>().enabled = false;
            Character2D AttackingCharacter = SelectedCharacter;
            Character2D HitCharacter = GetNextCharacter();
            float AttackingCharacterSpeed = AttackingCharacter.MaxSpeed;
            AttackingCharacter.MaxSpeed = 4;
            SetButtonsInteractable(false);
            MinionControl2D Minion = SelectedCharacter.GetComponent<MinionControl2D>();
            bool IsMinion = Minion != null;
            if (Minion == null)
            {
                Minion = SelectedCharacter.gameObject.AddComponent<MinionControl2D>();
            }
            // Distance should depend on move
            yield return StartCoroutine(Minion.WalkToTarget(HitCharacter.transform.transform, 0.05f));
            // Attack animation if melee
            if (CombatHitSound)
            {
                MySource.pitch = Random.Range(0.85f, 1.15f);
                MySource.PlayOneShot(CombatHitSound);
            }
            yield return new WaitForSeconds(0.5f);
            // walk back to spawner
            HitCharacter.GetHit(SelectedCharacter, (int)AttackDamage);
            CreateHealthPopup(HitCharacter.transform.position, AttackDamage);

            Vector3 HitCharacterPosition = HitCharacter.transform.position;
            if (HitCharacter.Health <= 0)
            {
                Debug.Log(SelectedCharacter.name + " has defeated " + HitCharacter.name);
                // Death animation here
                yield return new WaitForSeconds(0.5f);
                Destroy(HitCharacter.gameObject);
            }

            yield return StartCoroutine(Minion.WalkToTarget(Spawners[SelectedCharacterIndex].transform, 0f));
            AttackingCharacter.FaceDirection((HitCharacterPosition - AttackingCharacter.transform.position).normalized);
            if (!IsMinion)
            {
                Destroy(Minion);
            }
            AttackingCharacter.MaxSpeed = AttackingCharacterSpeed;

            if (HitCharacter.Health <= 0)
            {
                // Victory dance here
                yield return new WaitForSeconds(2.5f);
                // Game Over
                ExitCombat();
            }
            else
            {
                SelectNextMonster();
                AttackingCharacterLabel.text = SelectedCharacter.name;
                if (AttackingCharacterLabel.text.Contains("(Clone)"))
                {
                    AttackingCharacterLabel.text = AttackingCharacterLabel.text.Substring(0, AttackingCharacterLabel.text.IndexOf("(Clone)"));
                }
                if (SelectedCharacter == PlayerCharacter)
                {
                    SetButtonsInteractable(true);
                }
                else
                {
                    yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));
                    // now damage the player
                    DoAction(Random.Range(0, 4));
                }
            }
            Camera.main.gameObject.GetComponent<FollowCamera>().enabled = true;
        }

        private void SetButtonsInteractable(bool NewButtonsState)
        {
            for (int i = 0; i < ActionButtons.Count; i++)
            {
                ActionButtons[i].interactable = NewButtonsState;
            }
        }

        private Character2D GetNextCharacter()
        {
            int CharacterIndex = SelectedCharacterIndex + 1;
            if (CharacterIndex == CombatCharacters.Count)
            {
                CharacterIndex = 0;
            }
            return CombatCharacters[CharacterIndex];
        }

        private void SelectNextMonster()
        {
            if (SelectedCharacter == CombatCharacters[CombatCharacters.Count-1])
            {
                SelectCharacter(0);
            }
            else
            {
                SelectCharacter(SelectedCharacterIndex + 1);
            }
        }

        public void SelectCharacter(int CharacterIndex)
        {
            SelectedCharacterIndex = CharacterIndex;
            SelectedCharacter = CombatCharacters[SelectedCharacterIndex];
        }

        public GameObject PopupPrefab;
        private void CreateHealthPopup(Vector3 Position, float Health)
        {
            GameObject Popup = Instantiate(PopupPrefab);
            Popup.transform.position = Position + new Vector3(0, 1, 0);
            Popup PopupComponent = Popup.GetComponent<Popup>();
            PopupComponent.Initialise(Health, 2);
            Destroy(Popup, 2f);
        }
    }

}