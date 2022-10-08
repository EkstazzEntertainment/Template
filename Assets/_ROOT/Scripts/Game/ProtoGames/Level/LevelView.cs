namespace Ekstazz.ProtoGames.Level
{
    using Character;
    using UnityEngine;

    public class LevelView : MonoBehaviour
    {
        [field: SerializeField] public Transform PlayerSpawnPoint { get; private set; }
        [field: SerializeField] public Transform EnvironmentRoot { get; private set; }

        public CharacterView PlayerView { get; protected set; }

        
        public virtual void InitPlayer(CharacterView characterView)
        {
            PlayerView = characterView;
        }
    }
}
