using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zeltex2D
{
    public class Teleporter : MonoBehaviour
    {
        public Teleporter OtherTeleporter;
        public float TeleportTime = 1.2f;
        public List<Character2D> MyCharacters = new List<Character2D>();// UserControl2D MyUser;
        public List<float> TimeEntered = new List<float>();// UserControl2D MyUser;
        public GameObject TeleporterParticles;
        private bool IsParticlesAnimating;

        void Awake()
        {
            if (OtherTeleporter && OtherTeleporter.OtherTeleporter == null)
            {
                OtherTeleporter.OtherTeleporter = this;
            }
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            Character2D PossibleUser = other.gameObject.GetComponent<Character2D>();
            if (PossibleUser && MyCharacters.Contains(PossibleUser) == false)
            {
                Debug.Log("Adding " + PossibleUser.name);
                MyCharacters.Add(PossibleUser);
                TimeEntered.Add(Time.time);
            }
        }

        void OnTriggerStay2D(Collider2D other)
        {
            Character2D PossibleUser = other.gameObject.GetComponent<Character2D>();
            if (PossibleUser && MyCharacters.Contains(PossibleUser))
            {
                int Index = MyCharacters.IndexOf(PossibleUser);
                if (Time.time - TimeEntered[Index] >= TeleportTime)
                {
                    Debug.Log("Teleporting and removing " + PossibleUser.name);
                    PossibleUser.transform.position = OtherTeleporter.transform.position;
                    UserControl2D MyUser = PossibleUser.gameObject.GetComponent<UserControl2D>();
                    if (MyUser)
                    {
                        MyUser.TeleportCamera();
                    }
                    /*if (IsParticlesAnimating)
                    {
                        GameObject MyParticleSpawn = Instantiate(TeleporterParticles, transform.position);
                    }*/
                    GameObject MyParticleSpawn = Instantiate(TeleporterParticles, transform.position, Quaternion.identity);
                    Destroy(MyParticleSpawn, 6f);
                    GameObject MyParticleSpawn2 = Instantiate(TeleporterParticles, OtherTeleporter.transform.position, Quaternion.identity);
                    Destroy(MyParticleSpawn2, 6f);
                    MyParticleSpawn.transform.eulerAngles = new Vector3(90, 0, 0);
                    MyParticleSpawn2.transform.eulerAngles = new Vector3(90, 0, 0);

                    MyCharacters.RemoveAt(Index);
                    TimeEntered.RemoveAt(Index);
                    OtherTeleporter.IgnoreForCharacter(PossibleUser);
                    StartCoroutine(DisableCharacterForTime(PossibleUser));
                }
            }
        }

        private IEnumerator DisableCharacterForTime(Character2D PossibleUser)
        {
            //PossibleUser.gameObject.SetActive(false);
            UserControl2D MyUser = PossibleUser.gameObject.GetComponent<UserControl2D>();
            MinionControl2D MyMinion = PossibleUser.gameObject.GetComponent<MinionControl2D>();
            PossibleUser.enabled = false;
            PossibleUser.transform.GetChild(0).gameObject.SetActive(false);
            if (MyUser)
            {
                MyUser.enabled = false;
            }
            else if (MyMinion)
            {
                MyMinion.enabled = false;
            }
            yield return new WaitForSeconds(1f);
            PossibleUser.transform.GetChild(0).gameObject.SetActive(true);
            float TimeBegun = Time.time;
            Vector3 OriginalVector = PossibleUser.transform.localScale;
            while (Time.time - TimeBegun <= 1.5f)
            {
                PossibleUser.transform.localScale = Vector3.Lerp(new Vector3(0.1f, 0.1f, 0.1f), OriginalVector, (Time.time - TimeBegun) / 1.5f);
                yield return null;
            }
            PossibleUser.transform.localScale = OriginalVector;
            //PossibleUser.gameObject.SetActive(true);
            PossibleUser.enabled = true;
            if (MyUser)
            {
                MyUser.enabled = true;
            }
            else if (MyMinion)
            {
                MyMinion.enabled = true;
            }
        }

        public void IgnoreForCharacter(Character2D PossibleUser)
        {
            if (MyCharacters.Contains(PossibleUser))
            {
                int Index = MyCharacters.IndexOf(PossibleUser);
                MyCharacters.RemoveAt(Index);
                TimeEntered.RemoveAt(Index);
                Debug.Log("Removing " + PossibleUser.name);
            }
            MyCharacters.Add(PossibleUser);
            TimeEntered.Add(1000000000); // never activate
        }

        void OnTriggerExit2D(Collider2D other)
        {
            Character2D PossibleUser = other.gameObject.GetComponent<Character2D>();
            if (PossibleUser && MyCharacters.Contains(PossibleUser))
            {
                Debug.Log("Removing " + PossibleUser.name);
                int Index = MyCharacters.IndexOf(PossibleUser);
                MyCharacters.RemoveAt(Index);
                TimeEntered.RemoveAt(Index);
            }
        }
    }
}