using UnityEngine;
using UnityEngine.UI;

namespace FoW
{
    [AddComponentMenu("FogOfWar/HideInFog")]
    public class HideInFog : MonoBehaviour
    {
        [Range(0.0f, 1.0f)]
        public float minFogStrength = 0.2f;
        public Vector3 OffsetPosition;

        Transform _transform;
        Renderer _renderer;
        Graphic _graphic;
        Canvas _canvas;
        protected bool visible;

        void Start()
        {
            _transform = transform;
            _renderer = GetComponent<Renderer>();
            _graphic = GetComponent<Graphic>();
            _canvas = GetComponent<Canvas>();
        }

        void Update()
        {
            visible = !FogOfWar.current.IsInFog(_transform.position + OffsetPosition, minFogStrength);
            OnVisibleChanged();
        }

        public virtual void OnVisibleChanged()
        {
            if (_renderer != null)
                _renderer.enabled = visible;
            if (_graphic != null)
                _graphic.enabled = visible;
            if (_canvas != null)
                _canvas.enabled = visible;
        }
    }
}
