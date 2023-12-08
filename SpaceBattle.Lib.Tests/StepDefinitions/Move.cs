using Moq;
using TechTalk.SpecFlow;

namespace SpaceBattle.Lib.Tests;

[Binding]
public class Move
{
    private readonly Mock<IMovable> _movable;

    private Action commandExecutionLambda;

    public Move()
    {
        _movable = new Mock<IMovable>();

        commandExecutionLambda = () => { };

    }

    [Given(@"космический корабль находится в точке пространства с координатами \((.*), (.*)\)")]
    public void ДопустимКосмическийКорабльНаходитсяВТочкеПространстваСКоординатами(int p0, int p1)
    {
        _movable.SetupGet(m => m.Position).Returns(new Vector(new int[] { p0, p1 }));
    }

    [Given(@"космический корабль находится в точке трехмерноного пространства с координатами \((.*), (.*), (.*)\)")]
    public void ДопустимКосмическийКорабльНаходитсяВТочкеТрехмерноногоПространстваСКоординатами(int p0, int p1, int p2)
    {
        _movable.SetupGet(m => m.Position).Returns(new Vector(new int[] { p0, p1,p2 }));
    }

    [Given(@"имеет мгновенную скорость \((.*), (.*)\)")]
    public void ДопустимИмеетМгновеннуюСкорость(int p0, int p1)
    {
        _movable.SetupGet(m => m.Velocity).Returns(new Vector(new int[] { p0, p1 }));
    }

    [Given(@"объект находится в точке пространства \(\)")]
    public void ДопустимОбъектНаходитсяВТочкеПространства()
    {
        commandExecutionLambda = () =>_movable.SetupGet(m => m.Position).Returns(new Vector(Array.Empty<int>()));
    }

    [When(@"происходит прямолинейное равномерное движение без деформации")]
    public void КогдаПроисходитПрямолинейноеРавномерноеДвижениеБезДеформации()
    {
        var mc = new MoveCommand(_movable.Object);
        commandExecutionLambda = () => mc.Execute();
    }

    [Then(@"космический корабль перемещается в точку пространства с координатами \((.*), (.*)\)")]
    public void ТоКосмическийКорабльПеремещаетсяВТочкуПространстваСКоординатами(int p0, int p1)
    {
        commandExecutionLambda();
        _movable.VerifySet(m => m.Position = new Vector(new int[] { p0, p1 }), Times.Once);
    }

    [Given(@"космический корабль, положение в пространстве которого невозможно определить")]
    public void ДопустимКосмическийКорабльПоложениеВПространствеКоторогоНевозможноОпределить()
    {
        _movable.SetupGet(m => m.Position).Throws<Exception>();
    }

    [Given(@"скорость корабля определить невозможно")]
    public void ДопустимСкоростьКорабляОпределитьНевозможно()
    {
        _movable.SetupGet(m => m.Velocity).Throws<Exception>();
    }

    [Given(@"изменить положение в пространстве космического корабля невозможно")]
    public void ДопустимИзменитьПоложениеВПространствеКосмическогоКорабляНевозможно()
    {
        _movable.SetupGet(m => m.Velocity).Throws<Exception>();
        
    }

    [Then(@"возникает ошибка Exception")]
    public void ТоВозникаетОшибкаException()
    {
        Assert.Throws<Exception>(() => commandExecutionLambda());

    }
    [Then(@"возникает ошибка NullException")]
    public void ТоВозникаетОшибкаNullException()
    {
        Assert.Throws<Exception>(() => commandExecutionLambda());
    }
    
}
