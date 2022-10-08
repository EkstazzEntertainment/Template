namespace Ekstazz.ProtoGames.Character
{
    using System;
    using UnityEngine;

    public abstract class CharacterMovement : ICharacterMovement
    {
        public Vector3 Velocity { get; }
        public float Speed { get; }
        public bool ShouldMove { get; set; }
        public bool ShouldBehave { get; set; }
        
        public event Action<Vector3> OnVelocityUpdate;
        
        
        public virtual void FixedTick()
        {
            
        }
    }
}