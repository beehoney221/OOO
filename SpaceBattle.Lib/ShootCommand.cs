using Hwdtech;

namespace SpaceBattle.Lib
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

            var createTorpedoCommand = new CreateTorpedoCommand(position, velocity);

            createTorpedoCommand.Execute();
        }
    }
}
