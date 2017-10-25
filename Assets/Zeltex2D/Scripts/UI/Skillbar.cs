using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Zeltex2D
{
    public class Skillbar : MonoBehaviour
    {
        public GameObject SelectionIcon;
        public List<GameObject> Skills = new List<GameObject>();
        public int CurrentTargetedSkill = 0;

        public void IncreaseSkillSelection()
        {
            int NewSkill = CurrentTargetedSkill + 1;
            if (NewSkill >= Skills.Count)
            {
                NewSkill = 0;
            }
            SetSkill(NewSkill);
        }

        public void DecreaseSkillSelection()
        {
            int NewSkill = CurrentTargetedSkill - 1;
            if (NewSkill < 0)
            {
                NewSkill = Skills.Count - 1;
            }
            SetSkill(NewSkill);
        }

        public void SetSkill(int NewSkill)
        {
            if (CurrentTargetedSkill != NewSkill)
            {
                CurrentTargetedSkill = NewSkill;
                RectTransform MyRect = SelectionIcon.GetComponent<RectTransform>();
                RectTransform TargetRect = Skills[CurrentTargetedSkill].GetComponent<RectTransform>();
                MyRect.anchoredPosition = TargetRect.anchoredPosition;
            }
        }
        /*public void SetBars()
        {

        }*/
    }
}