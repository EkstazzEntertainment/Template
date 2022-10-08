namespace Ekstazz.Tools
{
    using UnityEngine;

    public enum GizmosType
    {
        None,
        Cube,
        WireCube,
        Sphere,
        WireSphere,
        Mesh,
        WireMesh,
        Ray
    }

    public enum GizmosDisplayType
    {
        Always,
        OnlyWhenSelected
    }
    
    public class GizmosDrawer : MonoBehaviour
    {
        [Header("Display settings")]
        [SerializeField,Tooltip("Define when gizmos should be visible")] 
        public GizmosDisplayType displayType = GizmosDisplayType.Always;
        
        [Header("Gizmo settings")]
        [SerializeField] public Color gizmoColor = Color.black;
        [SerializeField] public GizmosType type;
        [SerializeField] public Vector3 gizmosCenterOffset = Vector3.zero;

        [Header("Cube settings")]
        [SerializeField]
        public Vector3 cubeSize = Vector3.one;

        [Header("Sphere settings")]
        [SerializeField] public float sphereRadius = 1f;

        [Header("Mesh settings")]
        [SerializeField] public Mesh mesh;
        [SerializeField] public Vector3 meshScale = Vector3.one;
        [SerializeField, Tooltip("Represents mesh rotation in degrees")]
        public Vector3 meshRotation = Vector3.zero;
        
        [Header("Ray settings")]
        [SerializeField, Tooltip("Represents rotation of ray direction in degrees")]
        public Vector3 rayRotation = Vector3.zero;
        [SerializeField] public float rayLength = 1f;
            
        
        private void OnDrawGizmos()
        {
            if (displayType == GizmosDisplayType.Always)
            {
                DrawGizmos();
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (displayType == GizmosDisplayType.OnlyWhenSelected)
            {
                DrawGizmos();
            }
        }

        private void DrawGizmos()
        {
            Gizmos.color = gizmoColor;
            switch (type)
            {
                case GizmosType.Cube: DrawCube(); break;
                case GizmosType.WireCube: DrawWireCube(); break;
                case GizmosType.Sphere : DrawSphere(); break;
                case GizmosType.WireSphere : DrawWireSphere(); break;
                case GizmosType.Mesh : DrawMesh(); break;
                case GizmosType.WireMesh : DrawWireMesh(); break;
                case GizmosType.Ray : DrawRay(); break;
            }
        }

        private void DrawCube()
        {
            Gizmos.DrawCube(transform.position + gizmosCenterOffset, cubeSize);
        }

        private void DrawWireCube()
        {
            Gizmos.DrawWireCube(transform.position + gizmosCenterOffset, cubeSize);
        }

        private void DrawSphere()
        {
            Gizmos.DrawSphere(transform.position + gizmosCenterOffset, sphereRadius);
        }

        private void DrawWireSphere()
        {
            Gizmos.DrawWireSphere(transform.position + gizmosCenterOffset, sphereRadius);
        }

        private void DrawMesh()
        {
            Gizmos.DrawMesh(mesh, transform.position + gizmosCenterOffset, Quaternion.Euler(meshRotation), meshScale);
        }

        private void DrawWireMesh()
        {
            Gizmos.DrawWireMesh(mesh, transform.position + gizmosCenterOffset, Quaternion.Euler(meshRotation), meshScale);
        }

        private void DrawRay()
        {
            var direction = (Quaternion.Euler(rayRotation) * transform.forward).normalized* rayLength;
            Gizmos.DrawRay(transform.position + gizmosCenterOffset, direction);
        }
    }
}