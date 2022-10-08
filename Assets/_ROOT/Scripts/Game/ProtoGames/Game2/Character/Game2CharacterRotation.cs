namespace Ekstazz.ProtoGames.Game2.Character
{
    using ProtoGames.Character;
    using UnityEngine;
    using Zenject;

    
    public class Game2CharacterRotation : CharacterRotation, IFixedTickable
    {
        private bool shouldRotate;

        
        public void FixedTick()
        {
            if (shouldRotate)
            {
                Rotate();
            }
        }

        public void InitializeRotation()
        {
        }

        public override void SetMovementParameters(Vector2 direction)
        {
        }
        
        public override void CalculateRotation(Vector2 direction)
        {
        }
        
        public void ActivateRotation()
        {
            shouldRotate = true;
        }

        public void DeactivateRotation()
        {
            shouldRotate = false;
        }

        public override void Rotate()
        {
        }
    }
}