namespace Ekstazz.ProtoGames.Settings
{
    using System;
    using System.Collections.Generic;
    using Character;
    using Ekstazz.ProtoGames.StateMachine;
    using UnityEngine;

    
    [CreateAssetMenu(fileName = "PrefabSettings", menuName = "ProtoGames/Settings/PrefabSettings")]
    public class PrefabSettings : ScriptableObject
    {
        public List<PlayerCharacter> players;
    }

    [Serializable]
    public class PlayerCharacter
    {
        public string gameId;
        public CharacterView player;
        public StateMachine stateMachine; 
    }
}
