namespace Ekstazz.ProtoGames
{
    using System.Linq;
    using Character;
    using Settings;
    using UnityEngine;
    using Zenject;


    public class Factories
    {
        public class CharacterFactory : PlaceholderFactory<Object, CharacterView>
        {
            [Inject] private LevelGameProvider levelGameProvider;
            [Inject] private PrefabSettings prefabSettings;

            private readonly DiContainer container;

            public CharacterFactory(DiContainer container)
            {
                this.container = container;
            }

            public override CharacterView Create(Object param)
            {
                var characterView = base.Create(param);
                SetupStateMachine(characterView);
                return characterView;
            }

            private void SetupStateMachine(CharacterView characterView)
            {
                var stateMachine = prefabSettings.players
                    .First(player => player.gameId == levelGameProvider.CurrentLevelGame.Id);
                var stateMachineInstance = container
                    .InstantiatePrefabForComponent<StateMachine.StateMachine>(stateMachine.stateMachine, characterView.transform);
                characterView.SetUpStateMachine(stateMachineInstance);
            }
        }
    }
}
