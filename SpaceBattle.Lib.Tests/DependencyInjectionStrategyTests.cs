using Hwdtech;
using Hwdtech.Ioc;
using Moq;

namespace SpaceBattle.Lib.Tests
{
    public class DependencyInjectionStrategyTests
    {
        public interface ITestDependency { 
            public void SomeMethod();
        }
        public class TestTypeSingleConstructor
        {
            public ITestDependency Dep { get; }
            public TestTypeSingleConstructor(ITestDependency dep) => Dep = dep;
        }
        public class TestTypeMultipleConstructors
        {
            public ITestDependency? Dep { get; }
            public TestTypeMultipleConstructors() { }
            public TestTypeMultipleConstructors(ITestDependency dep) => Dep = dep;
        }

        public DependencyInjectionStrategyTests()
        {
            new InitScopeBasedIoCImplementationCommand().Execute();
            IoC.Resolve<Hwdtech.ICommand>(
                "Scopes.Current.Set",
                IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))
            ).Execute();
        }

        [Fact]
        public void DependencyInjectionStrategyThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new DependencyInjectionStrategy(null!));
        }

        [Fact]
        public void DependencyInjectionStrategyThrowsMultipleConstructorsException()
        {
            var type = typeof(TestTypeMultipleConstructors);
            var strategy = new DependencyInjectionStrategy(type);

            Assert.Throws<InvalidOperationException>(() => strategy.Execute());
        }

        [Fact]
        public void DependencyInjectionStrategyTestsSuccesful()
        {
            var type = typeof(TestTypeSingleConstructor);
            var strategy = new DependencyInjectionStrategy(type);

            var mockDependency = new Mock<ITestDependency>().Object;
            IoC.Resolve<Hwdtech.ICommand>(
                "IoC.Register",
                typeof(ITestDependency).ToString(),
                (object[] args) => mockDependency
            ).Execute();

            strategy.Execute();

            var instance = IoC.Resolve<TestTypeSingleConstructor>(type.ToString());

            Assert.IsType<TestTypeSingleConstructor>(instance);
            var typedInstance = (TestTypeSingleConstructor)instance;
            Assert.Equal(mockDependency, instance.Dep);
        }
        [Fact]
        public void DependencyTestsSuccesful()
        {
            var type = typeof(TestTypeSingleConstructor);
            var strategy = new DependencyInjectionStrategy(type);

            var mockDependency = new Mock<ITestDependency>();
            mockDependency.Setup(m => m.SomeMethod());

            IoC.Resolve<Hwdtech.ICommand>(
                "IoC.Register",
                typeof(ITestDependency).ToString(),
                (object[] args) => mockDependency.Object
            ).Execute();

            strategy.Execute();
            var instance = IoC.Resolve<TestTypeSingleConstructor>(type.ToString());

            instance.Dep.SomeMethod();

            Assert.IsType<TestTypeSingleConstructor>(instance);
            Assert.Equal(mockDependency.Object, instance.Dep);
            
            mockDependency.Verify(m => m.SomeMethod(), Times.AtLeastOnce());
        }
    }
}
