namespace Ekstazz.Wallet
{
    using Core.Modules;
    using Currencies;
    using Ui;
    using Views;
    using Zenject;


    [AutoInstalledModule]
    public class WalletInstaller : ModuleInstaller
    {
        public override IModuleInitializer ModuleInitializer => new Initializer();
        public override string Name => "Ekstazz.Wallet";

        
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<Wallet>().AsSingle();
            Container.Bind<IBackpack>().To<Backpack>().AsSingle();
            Container.Bind<IAccountFactory>().To<StrangeAccountFactory>().AsSingle();

            var moneyFactory = new MoneyFactory();
            Container.Bind<MoneyFactory>().FromInstance(moneyFactory).AsSingle();
            MoneyExt.Factory = moneyFactory;

            Container.DeclareSignal<TransactionCompleted>();

            Icons.Register<WalletIcons>();
        }

        private class Initializer : IModuleInitializer
        {
            public void Prepare()
            {
            }
        }
    }
}