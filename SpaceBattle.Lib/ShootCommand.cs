using Hwdtech;
using SpaceBattle.Lib;

namespace SpaceBattle
{

    public class ShootCommand : ICommand
    {
        private readonly IMovable _ship;

        public ShootCommand(IMovable ship)
        {
            _ship = ship;
        }

        public void Execute()
        {
            var position = _ship.Position;
            var velocity = _ship.Velocity;

            var createTorpedoCommand = new CreateTorpedoCommand(_ship, position, velocity);

            createTorpedoCommand.Execute();
        }
    }
}
