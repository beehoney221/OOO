using Hwdtech;

namespace SpaceBattle.Lib
{
    public class DependencyInjectionStrategy : ICommand
    {
        private readonly Type _type;

        public DependencyInjectionStrategy(Type type)
        {
            _type = type ?? throw new ArgumentNullException(nameof(type));
        }

        public void Execute()
        {
            var constructor = _type.GetConstructors().Single();
            var parameters = constructor.GetParameters();

            IoC.Resolve<ICommand>(
                "IoC.Register",
                _type.ToString(),
                (object[] args) =>
                {
                    var resolvedArgs = parameters
                        .Select(p => IoC.Resolve<object>(p.ParameterType.ToString()))
                        .ToArray();

                    return constructor.Invoke(resolvedArgs);
                }
            ).Execute();
        }
    }
}
