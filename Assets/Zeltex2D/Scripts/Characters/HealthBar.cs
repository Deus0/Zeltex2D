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
        public bool IsUseBars;
        public RectTransform PercentageBar;

        private void Start()
        {
            if (IsUseBars == false && BarTemplate.transform.parent)
            {
                BarTemplate.transform.parent.gameObject.SetActive(false);
            }
        }

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
            if (IsUseBars)
            {
                for (int i = 0; i < MyBars.Count; i++)
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
        }

        private float MaxHealth;

        public void SetHealthBars(float HealthBarsCount)
        {
            if (IsUseBars)
            {
                SetHealthBars((int)HealthBarsCount);
            }
            else if (PercentageBar)
            {
                MaxHealth = HealthBarsCount;
                PercentageBar.offsetMax = new Vector2(0, PercentageBar.offsetMax.y);
            }
        }

        public void SetBars(float NewBarCount)
        {
            if (IsUseBars)
            {
                SetBars((int)NewBarCount);
            }
            else if (PercentageBar)
            {
                float BarLength = PercentageBar.parent.gameObject.GetComponent<RectTransform>().rect.size.x - 3.6f - 3.6f;
                PercentageBar.offsetMax = new Vector2(-BarLength + ((NewBarCount / MaxHealth) * BarLength), PercentageBar.offsetMax.y);
                if (NewBarCount == 0)
                {
                    PercentageBar.parent.gameObject.SetActive(false);
                }
            }
        }

        public void SetHealthBars(int HealthBarsCount)
        {
            ClearBars();
            if (IsUseBars)
            {
                for (int i = 0; i < HealthBarsCount; i++)
                {
                    GameObject Newbar = Instantiate(BarTemplate, BarTemplate.transform.parent);
                    Newbar.name = "HealthBar" + i;
                    Newbar.SetActive(true);
                    MyBars.Add(Newbar);
                }
            }
        }

        public void SetBars(int NewBarCount)
        {
            if (IsUseBars)
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
        }

        public void SetVisibility(bool NewState)
        {
            gameObject.SetActive(NewState);
        }
    }
}

