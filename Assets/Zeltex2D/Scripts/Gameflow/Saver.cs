using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zeltex2D
{
    public class Saver : MonoBehaviour
    {
        private float lastSaved;
        public Transform MainCharacter;
        public SaveData Data = new SaveData();
        // SaveData
        void Start()
        {
            Data.Position = new Vector2(MainCharacter.position.x, MainCharacter.position.y);
            Data.Load();
            MainCharacter.transform.position = new Vector3(Data.Position.x, Data.Position.y, MainCharacter.transform.position.z);
            MainCharacter.GetComponent<UserControl2D>().TeleportCamera();
        }

        // Update is called once per frame
        void Update()
        {
            if (Time.time - lastSaved >= 1f)
            {
                lastSaved = Time.time;
                Vector2 NewPosition = new Vector2(MainCharacter.position.x, MainCharacter.position.y);
                if (Data.Position != NewPosition)
                {
                    Data.Position = NewPosition;
                    Data.Save();
                }
            }
        }

        public class SaveData
        {
            public Vector2 Position = Vector2.zero;

            public void Load()
            {
                Position.x = PlayerPrefs.GetFloat("PositionX", Position.x);
                Position.y = PlayerPrefs.GetFloat("PositionY", Position.y);
            }

            public void Save()
            {
                PlayerPrefs.SetFloat("PositionX", Position.x);
                PlayerPrefs.SetFloat("PositionY", Position.y);
            }
        }
    }
}
