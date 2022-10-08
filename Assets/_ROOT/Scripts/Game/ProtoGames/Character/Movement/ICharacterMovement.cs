namespace Ekstazz.ProtoGames.Character
{
    using System;
    using UnityEngine;
    using Zenject;

    public interface ICharacterMovement : IFixedTickable
    {
        event Action<Vector3> OnVelocityUpdate;
        Vector3 Velocity { get; }
    }
}