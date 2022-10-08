namespace Game.Editor
{
    using Ekstazz.ProtoGames.Cameras;
    using UnityEditor;
    using UnityEngine;
    using Editor = UnityEditor.Editor;

    
    [CustomEditor(typeof(VirtualCamera), true)]
    public class VirtualCameraEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var targetView = (VirtualCamera) target;

            if ( Application.isPlaying)
            {
                if (GUILayout.Button("Cache TransPoser and Composer configurations"))
                {
                    CacheCurrentCameraConfigurations(targetView);
                    EditorUtility.SetDirty(target);
                }
            }
        }

        private void CacheCurrentCameraConfigurations(VirtualCamera view)
        {
            view.SaveCurrentConfigurations();
        }
    }
}
