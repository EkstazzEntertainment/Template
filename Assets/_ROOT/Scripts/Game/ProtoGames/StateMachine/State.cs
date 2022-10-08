namespace Ekstazz.ProtoGames.StateMachine
{
    using UnityEngine;

    public class State : MonoBehaviour, IState
    {
        [SerializeField] public Transition[] transitions;
    }
}
