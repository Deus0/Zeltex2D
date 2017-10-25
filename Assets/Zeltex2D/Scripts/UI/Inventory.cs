using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Zeltex2D
{
    /// <summary>
    /// This is connected to the gui, which attached on the players body
    /// </summary>
    public class Inventory : MonoBehaviour
    {
        public GameObject TemplateItem;
        private Dictionary<string, int> ItemData = new Dictionary<string, int>();
        public Dictionary<string, GameObject> SpawnedItemGuis = new Dictionary<string, GameObject>();
        private bool IsEmpty = true;

        void Start()
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Add in animation soon
        /// </summary>
        public void Toggle()
        {
            gameObject.SetActive(!gameObject.activeInHierarchy);
            Debug.Log("Toggled Inventory to " + gameObject.activeInHierarchy);
            if (gameObject.activeInHierarchy && IsEmpty)
            {
                transform.GetChild(0).gameObject.SetActive(false);
            }
        }

        public void AddItem(ItemData MyItem)
        {
            if (ItemData.ContainsKey(MyItem.name))
            {
                ItemData[MyItem.name]++;
                // Update text too
                GameObject ItemGui = SpawnedItemGuis[MyItem.name];
                ItemGui.transform.Find("QuantityText").GetComponent<Text>().text = "x" + ItemData[MyItem.name];
            }
            else
            {
                ItemData.Add(MyItem.name, 1);
                GameObject NewItem = Instantiate(TemplateItem, TemplateItem.transform.parent);
                NewItem.SetActive(true);
                NewItem.transform.Find("QuantityText").GetComponent<Text>().text = "x1";
                // 
                NewItem.transform.Find("ItemImage").GetComponent<Image>().sprite = MyItem.MySprite;
                SpawnedItemGuis.Add(MyItem.name, NewItem);
                if (IsEmpty)
                {
                    IsEmpty = false;
                    transform.GetChild(0).gameObject.SetActive(true);
                }
            }
        }
    }
}