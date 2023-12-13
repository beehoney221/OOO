using Moq;
using TechTalk.SpecFlow;

namespace SpaceBattle.Lib.Tests
{
    [Binding]
    public class Rotate
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly Mock<IRotatable> _rotatable;

        private Action commandExecutionLambda;
        public Rotate(ScenarioContext scenarioContext)
        {

            _scenarioContext = scenarioContext;

            _rotatable = new Mock<IRotatable>();

            commandExecutionLambda = () => { };

        }
        
        [Given(@"космический корабль имеет угол наклона (.*) град к оси OX")]
        public void ДопустимКосмическийКорабльИмеетУголНаклонаГрадКОсиOX(int p0)
        {
            _rotatable.SetupGet(m => m.Angle).Returns(new Angle(p0));
        }
        
        [Given(@"космический корабль, угол наклона которого невозможно определить")]
        public void ДопустимКосмическийКорабльУголНаклонаКоторогоНевозможноОпределить()
        {
            _rotatable.SetupGet(m => m.Angle).Throws<Exception>();
        }
        
        [Given(@"мгновенную угловую скорость невозможно определить")]
        public void ДопустимМгновеннуюУгловуюСкоростьНевозможноОпределить()
        {
            _rotatable.SetupGet(m => m.AngleVelocity).Throws<Exception>();
        }

        [Given(@"невозможно изменить угол наклона к оси OX космического корабля")]
        public void ДопустимНевозможноИзменитьУголНаклонаКОсиOXКосмическогоКорабля()
        {
            _rotatable.SetupGet(m => m.Angle).Throws<Exception>();
        }

        [Given(@"имеет мгновенную угловую скорость (.*) град")]
        public void ДопустимИмеетМгновеннуюУгловуюСкоростьГрад(int p0)
        {
            _rotatable.SetupGet(m => m.AngleVelocity).Returns(new Angle(p0));
        }
        
        [When(@"происходит вращение вокруг собственной оси")]
        public void КогдаПроисходитВращениеВокругСобственнойОси()
        {
            var rc = new RotateCommand(_rotatable.Object);
            commandExecutionLambda = () => rc.Execute();
        }
        
        [Then(@"угол наклона космического корабля к оси OX составляет (.*) град")]
        public void ТоУголНаклонаКосмическогоКорабляКОсиOXСоставляетГрад(int p0)
        {
            commandExecutionLambda();
            _rotatable.VerifySet(m => m.Angle = It.Is<Angle>(e => e.tilt == p0));
        }

        [Then(@"возникает ошибка Exception")]
        public void ТоВозникаетОшибкаException()
        {
            Assert.Throws<Exception>(() => commandExecutionLambda());
        }
    }
}
