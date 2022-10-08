namespace Game.Editor
{
    using Ekstazz.ProtoGames.StateMachine;
    using UnityEditor;
    using UnityEngine;

    
    [CustomEditor(typeof(State), true)]
    public class StateEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Find Transitions"))
            {
                var state = (State)target;
                FindTransitions(state);
                EditorUtility.SetDirty(target);
            }
        }

        private void FindTransitions(State state)
        {
            state.transitions = state.GetComponents<Transition>();
        }
    }
}