using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zeltex2D
{
    [ExecuteInEditMode]
    public class HealthBar : MonoBehaviour
    {
        public GameObject BarTemplate;
        [SerializeField, HideInInspector]
        private int BarCount;
        [SerializeField, HideInInspector]
        private List<GameObject> MyBars = new List<GameObject>();
        [Header("Testing")]
        public bool DoClearBars;
        public bool DoSpawnBars;

        private void Update()
        {
            if (DoClearBars)
            {
                DoClearBars = false;
                ClearBars();
            }
            if (DoSpawnBars)
            {
                DoSpawnBars = false;
                SetHealthBars(3);
            }
        }

        public void ClearBars()
        {
            for (int i = 0; i <  MyBars.Count; i++)
            {
                if (MyBars[i])
                {
                    if (Application.isPlaying)
                    {
                        Destroy(MyBars[i]);
                    }
                    else
                    {
                        DestroyImmediate(MyBars[i]);
                    }
                }
            }
            MyBars.Clear();
        }

        public void SetHealthBars(int HealthBarsCount)
        {
            ClearBars();
            for (int i = 0; i < HealthBarsCount; i++)
            {
                GameObject Newbar = Instantiate(BarTemplate, BarTemplate.transform.parent);
                Newbar.name = "HealthBar" + i;
                Newbar.SetActive(true);
                MyBars.Add(Newbar);
            }
        }

        public void SetBars(int NewBarCount)
        {
            for (int i = 0; i < MyBars.Count; i++)
            {
                if (i >= NewBarCount)
                {
                    MyBars[i].SetActive(false);
                }
                else
                {
                    MyBars[i].SetActive(true);
                }
            }
            if (NewBarCount == 0)
            {
                gameObject.SetActive(false);
            }
        }

        public void SetVisibility(bool NewState)
        {
            gameObject.SetActive(NewState);
        }
    }
}

