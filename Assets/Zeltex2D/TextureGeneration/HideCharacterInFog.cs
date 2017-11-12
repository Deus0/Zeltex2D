using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zeltex2D
{
    public class HideCharacterInFog : FoW.HideInFog
    {
        public MinimapIconSpawner MapIcon;
        public HealthBar MyHealthBar;

        public override void OnVisibleChanged()
        {
            base.OnVisibleChanged();
            if (MapIcon && MapIcon.MyIcon)
            {
                MapIcon.MyIcon.SetVisibility(visible);
            }
            if (MyHealthBar)
            {
                MyHealthBar.SetVisibility(visible);
            }
        }
    }

}