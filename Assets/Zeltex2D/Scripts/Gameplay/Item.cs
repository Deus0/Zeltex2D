using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zeltex2D
{
    /// <summary>
    /// User2D Interacts with this in world
    /// </summary>
    public class Item : MonoBehaviour
    {
        public ItemData Data;
        public GameObject GlowObject;
        private Animator MyAnimator;
        [HideInInspector]
        public bool CanPickup = true;
        private bool IsGlowing;

        void Awake()
        {
            MyAnimator = GetComponent<Animator>();
        }

        void Start()
        {
            GetComponent<SpriteRenderer>().sprite = Data.MySprite;
        }

        void LateUpdate()
        {
            GlowObject.SetActive(IsGlowing);
        }

        public void PickedUp()
        {
            CanPickup = false;
            MyAnimator.SetTrigger("Die");
            StartCoroutine(DestroyAfterDeath());
        }

        private IEnumerator DestroyAfterDeath()
        {
            yield return new WaitForSeconds(1f);
            Destroy(gameObject);
        }

        public void Glow()
        {
            Debug.Log(name + " is being Selected.");
            IsGlowing = true;
        }

        public void RemoveGlow()
        {
            Debug.Log(name + " is being DeSelected.");
            IsGlowing = false;
        }
    }
}
