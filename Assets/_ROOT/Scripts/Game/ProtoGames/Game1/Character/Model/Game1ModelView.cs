namespace Ekstazz.ProtoGames.Game1.Character
{
    using ProtoGames.Character;
    using UnityEngine;

    
    public class Game1ModelView : MonoBehaviour, IModelView
    {
        [field: SerializeField] public Game1CharacterAnimator Animator { get; private set; }
    }
}