namespace Ekstazz.Tools
{
    using UnityEditor;
    using UnityEngine;

    [CustomEditor(typeof(GizmosDrawer))]
    [CanEditMultipleObjects]
    public class GizmosDrawerEditor : Editor
    {
        private GizmosDrawer drawer;

        private GizmosSettingsEditor currentGizmoEditor;
        
        private void OnEnable()
        {
            drawer = (GizmosDrawer) target;
            currentGizmoEditor = GetGizmoEditor(drawer.type);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            EditorGUILayout.LabelField(label : "Drawer parameters:", style: EditorStyles.boldLabel);
            drawer.displayType = (GizmosDisplayType)EditorGUILayout.EnumPopup("Gizmos visibility:", drawer.displayType);
            drawer.type = (GizmosType) EditorGUILayout.EnumPopup("Gizmos to draw:", drawer.type);
            if (drawer.type != currentGizmoEditor?.Type )
            {
                currentGizmoEditor = GetGizmoEditor(drawer.type);
            }
            
            EditorGUILayout.Space();
            drawer.gizmoColor = EditorGUILayout.ColorField("Gizmos color:", drawer.gizmoColor);
            drawer.gizmosCenterOffset = EditorGUILayout.Vector3Field("Gizmos center offset:", drawer.gizmosCenterOffset);
            EditorGUILayout.Space();
            
            currentGizmoEditor?.OnInspectorGUI();

            if (GUILayout.Button("Save"))
            {
                drawer.gameObject.SetSceneDirty();
            }
        }

        private GizmosSettingsEditor GetGizmoEditor(GizmosType type)
        {
            switch (type)
            {
                case GizmosType.Cube: return new CubeGizmosEditor(drawer);
                case GizmosType.WireCube: return new WireCubeGizmosEditor(drawer);
                case GizmosType.Sphere: return new SphereGizmosEditor(drawer);
                case GizmosType.WireSphere: return new WireSphereGizmosEdtitor(drawer);
                case GizmosType.Mesh: return new MeshGizmosEditor(drawer);
                case GizmosType.WireMesh: return new WireMeshGizmosEditor(drawer);
                case GizmosType.Ray: return new RayGizmosEditor(drawer);
            }
            return null;
        }
        
        
    }

    internal abstract class GizmosSettingsEditor
    {
        protected readonly GizmosDrawer drawer;
        
        internal abstract GizmosType Type { get; }
        
        internal abstract void OnInspectorGUI();

        public GizmosSettingsEditor(GizmosDrawer drawer)
        {
            this.drawer = drawer;
        }
    }
    
    
    internal class CubeGizmosEditor: GizmosSettingsEditor
    {
        internal override GizmosType Type => GizmosType.Cube;
        
        public CubeGizmosEditor(GizmosDrawer drawer) : base(drawer) 
        {
        }

        internal override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField(label : "Cube parameters:", style: EditorStyles.boldLabel);
            drawer.cubeSize = EditorGUILayout.Vector3Field("Cube size:", drawer.cubeSize);
        }
    }
    
    internal class WireCubeGizmosEditor : CubeGizmosEditor
    {
        internal override GizmosType Type => GizmosType.WireCube;
        
        public WireCubeGizmosEditor(GizmosDrawer drawer) : base(drawer)
        {
        }
    }
    
    internal class SphereGizmosEditor : GizmosSettingsEditor
    {
        public SphereGizmosEditor(GizmosDrawer drawer) : base(drawer)
        {
        }

        internal override GizmosType Type => GizmosType.Sphere;
        
        internal override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField(label : "Sphere parameters:", style: EditorStyles.boldLabel);
            drawer.sphereRadius = EditorGUILayout.FloatField("Sphere radius:", drawer.sphereRadius);
        }
    }
    
    internal class WireSphereGizmosEdtitor : SphereGizmosEditor
    {
        public WireSphereGizmosEdtitor(GizmosDrawer drawer) : base(drawer)
        {
        }

        internal override GizmosType Type => GizmosType.WireSphere;
    }
    
    internal class MeshGizmosEditor : GizmosSettingsEditor
    {
        public MeshGizmosEditor(GizmosDrawer drawer) : base(drawer)
        {
        }

        internal override GizmosType Type => GizmosType.Mesh;
        
        internal override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField(label : "Mesh parameters:", style: EditorStyles.boldLabel);
            drawer.mesh = (Mesh)EditorGUILayout.ObjectField("Mesh to draw:", drawer.mesh,typeof(Mesh),true);
            drawer.meshScale = EditorGUILayout.Vector3Field("Mesh scale:", drawer.meshScale);
            drawer.meshRotation = EditorGUILayout.Vector3Field("Mesh rotation:", drawer.meshRotation);
            EditorGUILayout.HelpBox("Rotation is specified using degrees around axis",MessageType.Info);
        }
    }
    
    internal class WireMeshGizmosEditor : MeshGizmosEditor
    {
        public WireMeshGizmosEditor(GizmosDrawer drawer) : base(drawer)
        {
        }

        internal override GizmosType Type => GizmosType.WireMesh;
    }

    internal class RayGizmosEditor : GizmosSettingsEditor
    {
        public RayGizmosEditor(GizmosDrawer drawer) : base(drawer)
        {
        }

        internal override GizmosType Type => GizmosType.Ray;
        
        internal override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField(label : "Ray parameters:", style: EditorStyles.boldLabel);
            drawer.rayRotation = EditorGUILayout.Vector3Field("Ray rotation:", drawer.rayRotation);
            drawer.rayLength = EditorGUILayout.FloatField("Ray length:", drawer.rayLength);
            EditorGUILayout.HelpBox("Rotation is specified using degrees around axis",MessageType.Info);
            
        }
    }
}