namespace Tests
{
    using Ekstazz.Currencies;
    using NUnit.Framework;

    
    public class MoneyTests
    {
        private MoneyFactory factory;

        [SetUp]
        public void SetUp()
        {
            factory = new MoneyFactory();
            factory.Register<StubCurrency>();
            factory.Register<StubCurrency2>();
        }

        [Test]
        public void _01SingleMoneyHasAmountAndCurrency()
        {
        }

        [Test]
        public void _02MoneyCreate_CreatesMoneyWithSpecifiedCurrency()
        {
            var money = factory.Create<StubCurrency>(10);

            Assert.That(money.Type, Is.TypeOf<StubCurrency>());
        }

        [Test]
        public void _03MoneyCreate_CreatesMoneyWithSpecifiedAmount()
        {
            var money = factory.Create<StubCurrency>(10);

            Assert.That(money.Amount, Is.EqualTo((Amount) 10));
        }

        [Test]
        public void _04WhenAddingMoneyWithSameCurrency_SumIsReturned()
        {
            var money1 = factory.Create<StubCurrency>(10);
            var money2 = factory.Create<StubCurrency>(10);

            var sum = money1.AddSame(money2);

            Assert.That(sum.Type, Is.SameAs(money1.Type));
            Assert.That(sum.Amount, Is.EqualTo((Amount) 20));
        }

        [Test]
        public void _05WhenAddingMoneyWithDifferentCurrency_ExceptionIsThrown()
        {
            var money1 = factory.Create<StubCurrency>(10);
            var money2 = factory.Create<StubCurrency2>(10);

            Assert.That(() => money1.AddSame(money2), Throws.Exception);
        }

        [Test]
        public void _06MoneyBagContainsMultipleCurrencies()
        {
            var money1 = factory.Create<StubCurrency>(10);
            var money2 = factory.Create<StubCurrency2>(10);

            var moneyBag = new MoneyBag(money1, money2);
        }

        [Test]
        public void _07MoneyBag_HasCurrencies()
        {
            var money1 = factory.Create<StubCurrency>(10);
            var money2 = factory.Create<StubCurrency2>(10);

            var moneyBag = new MoneyBag(money1, money2);

            Assert.That(moneyBag.Has<StubCurrency>());
        }

        [Test]
        public void _08MoneyBag_HasCurrenciesWithAmount()
        {
            var money1 = factory.Create<StubCurrency>(10);

            var moneyBag = new MoneyBag(money1);

            Assert.That(moneyBag.Has<StubCurrency>(10), Is.True);
            Assert.That(moneyBag.Has<StubCurrency>(20), Is.False);
        }

        [Test]
        public void _08MoneyBagAdd_AddsToAlreadyExistingAmount()
        {
            var money1 = factory.Create<StubCurrency>(10);
            var moneyBag = new MoneyBag(money1);

            moneyBag.Add(factory.Create<StubCurrency>(10));
            Assert.That(moneyBag.Has<StubCurrency>(20));
        }

        [Test]
        public void _09MoneyBagAdd_AddsToNewAmountIfCurrencyIsNotInTheBag()
        {
            var money1 = factory.Create<StubCurrency>(10);
            var moneyBag = new MoneyBag(money1);

            moneyBag.Add(factory.Create<StubCurrency2>(10));

            Assert.That(moneyBag.Has<StubCurrency2>(10));

            Assert.That(moneyBag.Has<StubCurrency>(10));
            Assert.That(moneyBag.Has<StubCurrency>(11), Is.False);
        }

        [Test]
        public void _10CanCreateMoneyWithOfExtension()
        {
            MoneyExt.Factory = factory;
            var money = 10.Of<StubCurrency>();
            
            Assert.That(money.Type, Is.TypeOf<StubCurrency>());
        }
        
        [Test]
        public void _11CanCreateMoneyWithOfExtensionWithAmount()
        {
            MoneyExt.Factory = factory;
            var money = ((Amount)10).Of<StubCurrency>();
            
            Assert.That(money.Type, Is.TypeOf<StubCurrency>());
        }
        
        [Test]
        public void _12MoneyFactoryCanCreateMoneyByTypeName()
        {
            var money = factory.Create("Stub", 10);
            Assert.That(money.Type, Is.TypeOf<StubCurrency>());
        }

        [Test]
        public void _13MoneyFactoryCanGiveTypeByTypeName()
        {
            var currencyType = factory.TypeOf("Stub");
            var stubCurrency = new StubCurrency();
            
            Assert.That(currencyType, Is.SameAs(stubCurrency.SingleInstance));
        }
        
        [Test]
        public void _11CanCreateMoneyWithOfExtensionUsingICurrencyType()
        {
            MoneyExt.Factory = factory;
            StubCurrency stubCurrency = new StubCurrency();
            var currency = stubCurrency.SingleInstance;
            var money = ((Amount)10).Of(currency);
            
            Assert.That(money.Type, Is.TypeOf<StubCurrency>());
        }

    }


    public class StubCurrency : CurrencyType<StubCurrency>
    {
        public override string Name => "Stub";
    }

    public class StubCurrency2 : CurrencyType<StubCurrency2>
    {
        public override string Name => "Stub2";
    }
}