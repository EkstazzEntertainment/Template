namespace Ekstazz.ProtoGames.Game2.Character
{
    using System;
    using ProtoGames.Character;
    using UnityEngine;
    using Zenject;


    public class Game2CharacterMovement : CharacterMovement, IInitializable, IDisposable
    {
        private Vector3 moveDirection;
        private bool shouldMove;

        public new Vector3 Velocity => velocity;
        public new float Speed => velocity.magnitude;
        
        private Vector3 velocity;

        
        public void Initialize()
        {
        }

        public override void FixedTick()
        {
        }
        
        public void InitMovement()
        {
        }
        
        public void ActivateMovement()
        {
            shouldMove = false;
        }
        
        public void DeactivateMovement()
        {
            shouldMove = false;
        }
        
        public void SetMovementParameters(Vector2 direction)
        {
        }
        
        public void Dispose()
        {
        }
    }
}