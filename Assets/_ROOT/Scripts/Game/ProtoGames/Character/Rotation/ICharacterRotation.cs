namespace Ekstazz.ProtoGames.Character
{
    using UnityEngine;

    public interface ICharacterRotation
    {
        void CalculateRotation(Vector2 direction);
        void SetMovementParameters(Vector2 direction);
        void Rotate();
    }
}