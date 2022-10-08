namespace Ekstazz.ProtoGames.Level
{
    using Ekstazz.LevelBased.SceneLoading;
    using Zenject;


    public interface ILevelViewProvider<TLevel> where TLevel : LevelView
    {
        TLevel LevelView { get; }
    }
    
    public interface ILevelViewFinder
    {
        void FindLevelView();
        LevelView CurrentLevelView { get; }
    }

    public class LevelViewProvider<TLevelView> : ILevelViewProvider<TLevelView>, ILevelViewFinder where TLevelView : LevelView
    {
        [Inject] private ILevelParentProvider levelParentProvider;
        
        public TLevelView LevelView { get; private set; }
        public LevelView CurrentLevelView => LevelView;

        public void FindLevelView()
        {
            LevelView = levelParentProvider.LevelParent.Area.GetComponent<TLevelView>();
        }
    }
}