namespace Ekstazz.Saves.Flow
{
    using System.Threading.Tasks;
    using Zenject;
    using Zenject.Extensions.Commands;

    public class TryUnblockSavesCommand : Command
    {
        [Inject]
        internal ISaveContext SaveContext { get; set; }
        
        public override async Task Execute()
        {
            SaveContext.ApplyBehaviour(SaveBehaviour.SaveEnabled, SaveBlockingContext.Initial);
        }
    }
}