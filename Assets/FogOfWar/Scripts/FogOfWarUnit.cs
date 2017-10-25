using UnityEngine;

namespace FoW
{
    public enum FogOfWarShapeType
    {
        Circle,
        Box,
        Texture
    }

    [AddComponentMenu("FogOfWar/FogOfWarUnit")]
    public class FogOfWarUnit : MonoBehaviour
    {
        public int team = 0;

        [Header("Shape")]
        public FogOfWarShapeType shapeType = FogOfWarShapeType.Circle;
        public Vector2 offset = Vector2.zero;

        // circle
        public float radius = 5.0f;
        [Range(0.0f, 1.0f)]
        public float innerRadius = 1;
        [Range(0.0f, 180.0f)]
        public float angle = 180;

        // texture
        public Texture2D texture;
        public bool rotateToForward = false;

        [Header("Line of Sight")]
        public LayerMask lineOfSightMask = 0;
        public float lineOfSightPenetration = 0;
        public bool cellBased = false;
        public bool antiFlicker = false;

        float[] _distances = null;
        bool[] _visibleCells = null;

        Transform _transform;

        void Start()
        {
            _transform = transform;
            FogOfWar.RegisterUnit(this);
        }

        static bool CalculateLineOfSight2D(Vector2 eye, float radius, float penetration, LayerMask layermask, float[] distances)
        {
            bool hashit = false;
            float angle = 360.0f / distances.Length;
            RaycastHit2D hit;

            for (int i = 0; i < distances.Length; ++i)
            {
                Vector2 dir = Quaternion.AngleAxis(angle * i, Vector3.back) * Vector2.up;
                hit = Physics2D.Raycast(eye, dir, radius, layermask);
                if (hit.collider != null)
                {
                    distances[i] = (hit.distance + penetration) / radius;
                    if (distances[i] < 1)
                        hashit = true;
                    else
                        distances[i] = 1;
                }
                else
                    distances[i] = 1;
            }

            return hashit;
        }

        static bool CalculateLineOfSight3D(Vector3 eye, float radius, float penetration, LayerMask layermask, float[] distances)
        {
            bool hashit = false;
            float angle = 360.0f / distances.Length;
            RaycastHit hit;

            for (int i = 0; i < distances.Length; ++i)
            {
                Vector3 dir = Quaternion.AngleAxis(angle * i, Vector3.up) * Vector3.forward;
                if (Physics.Raycast(eye, dir, out hit, radius, layermask))
                {
                    distances[i] = (hit.distance + penetration) / radius;
                    if (distances[i] < 1)
                        hashit = true;
                    else
                        distances[i] = 1;
                }
                else
                    distances[i] = 1;
            }

            return hashit;
        }

        public float[] CalculateLineOfSight(FogOfWarPhysics physicsmode, Vector3 eyepos)
        {
            if (lineOfSightMask == 0)
                return null;

            if (_distances == null)
                _distances = new float[256];

            if (physicsmode == FogOfWarPhysics.Physics2D)
            {
                if (CalculateLineOfSight2D(eyepos, radius, lineOfSightPenetration, lineOfSightMask, _distances))
                    return _distances;
            }
            else // 3D
            {
                if (CalculateLineOfSight3D(eyepos, radius, lineOfSightPenetration, lineOfSightMask, _distances))
                    return _distances;
            }
            return null;
        }

        static float Sign(float v)
        {
            if (Mathf.Approximately(v, 0))
                return 0;
            return v > 0 ? 1 : -1;
        }
        
        public bool[] CalculateLineOfSightCells(FogOfWarPhysics physicsmode, Vector3 eyepos)
        {
            if (physicsmode == FogOfWarPhysics.Physics3D)
            {
                Debug.LogWarning("Physics3D is not supported with cells!", this);
                return null;
            }

            int rad = Mathf.RoundToInt(radius);
            int width = rad + rad + 1;
            if (_visibleCells == null || _visibleCells.Length != width * width)
                _visibleCells = new bool[width * width];

            Vector2 cellsize = (FogOfWar.current.mapResolution.vector2 * 1.1f) / FogOfWar.current.mapSize; // do 1.1 to bring it away from the collider a bit so the raycast won't hit it
            Vector2 playerpos = FogOfWarConversion.SnapWorldPositionToNearestFogPixel(_transform.position, FogOfWar.current.mapOffset, FogOfWar.current.mapResolution, FogOfWar.current.mapSize);
            for (int y = -rad; y <= rad; ++y)
            {
                for (int x = -rad; x <= rad; ++x)
                {
                    Vector2i offset = new Vector2i(x, y);

                    // find the nearest point in the cell to the player and raycast to that point
                    Vector2 fogoffset = offset.vector2 - new Vector2(Sign(offset.x) * cellsize.x, Sign(offset.y) * cellsize.y) * 0.5f;
                    Vector2 worldoffset = FogOfWarConversion.FogToWorldSize(fogoffset, FogOfWar.current.mapResolution, FogOfWar.current.mapSize);
                    Vector2 worldpos = playerpos + worldoffset;

                    Debug.DrawLine(playerpos, worldpos);

                    int idx = (y + rad) * width + x + rad;

                    // if it is out of range
                    if (worldoffset.magnitude > radius)
                        _visibleCells[idx] = false;
                    else
                    {
                        _visibleCells[idx] = true;
                        RaycastHit2D hit = Physics2D.Raycast(playerpos, worldoffset.normalized, worldoffset.magnitude, lineOfSightMask);
                        _visibleCells[idx] = hit.collider == null;
                    }
                }
            }

            return _visibleCells;
        }

        void FillShape(FogOfWarShape shape)
        {
            if (antiFlicker)
            {
                // snap to nearest fog pixel
                shape.eyePosition = FogOfWarConversion.SnapWorldPositionToNearestFogPixel(FogOfWarConversion.WorldToFogPlane(_transform.position, FogOfWar.current.plane), FogOfWar.current.mapOffset, FogOfWar.current.mapResolution, FogOfWar.current.mapSize);
                shape.eyePosition = FogOfWarConversion.FogPlaneToWorld(shape.eyePosition.x, shape.eyePosition.y, _transform.position.y, FogOfWar.current.plane);
            }
            else
                shape.eyePosition = _transform.position;
            shape.foward = _transform.forward;
            shape.offset = offset;
            shape.radius = radius;
        }

        FogOfWarShape CreateShape()
        {
            if (shapeType == FogOfWarShapeType.Circle)
            {
                FogOfWarShapeCircle shape = new FogOfWarShapeCircle();
                FillShape(shape);
                shape.innerRadius = innerRadius;
                shape.angle = angle;
                return shape;
            }
            else if (shapeType == FogOfWarShapeType.Box)
            {
                FogOfWarShapeBox shape = new FogOfWarShapeBox();
                FillShape(shape);
                return shape;
            }
            else if (shapeType == FogOfWarShapeType.Texture)
            {
                if (texture == null)
                    return null;

                FogOfWarShapeTexture shape = new FogOfWarShapeTexture();
                FillShape(shape);
                shape.texture = texture;
                shape.rotateToForward = rotateToForward;
                return shape;
            }
            return null;
        }

        public FogOfWarShape GetShape(FogOfWarPhysics physics)
        {
            FogOfWarShape shape = CreateShape();
            if (shape == null)
                return null;

            if (cellBased)
            {
                shape.lineOfSight = null;
                shape.visibleCells = CalculateLineOfSightCells(physics, shape.eyePosition);
            }
            else
            {
                shape.lineOfSight = CalculateLineOfSight(physics, shape.eyePosition);
                shape.visibleCells = null;
            }
            return shape;
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            if (shapeType == FogOfWarShapeType.Circle)
                Gizmos.DrawWireSphere(transform.position, radius);
            else if (shapeType == FogOfWarShapeType.Box || shapeType == FogOfWarShapeType.Texture)
                Gizmos.DrawWireCube(transform.position, new Vector3(radius, radius, radius));
        }
    }
}