namespace Ekstazz.ProtoGames.Game2.Character
{
    using ProtoGames.Character;
    using UnityEngine;

    
    public class Game2ModelView : MonoBehaviour, IModelView
    {
        [field: SerializeField] public Game2CharacterAnimator Animator { get; private set; }
    }
}
