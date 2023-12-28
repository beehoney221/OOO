using Hwdtech;
using Hwdtech.Ioc;
using System.Collections;
using Moq;

namespace SpaceBattle.Lib.Tests;

public class CheckCollisionTests
{
    public CheckCollisionTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>(
            "Scopes.Current.Set",
            IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))
        ).Execute();

        IoC.Resolve<Hwdtech.ICommand>(
        "IoC.Register",
        "Game.IUObject.GetProperty",
        (object[] args) =>
        {
            var obj = (IUObject)args[0];
            var key = (string)args[1];

            var vec = obj.GetProperty(key);
            return vec;
        }
        ).Execute();

        var Hashtree = new Hashtable(){
                {1, new Hashtable(){
                    {1, new Hashtable(){
                        {0, new Hashtable(){
                            {-1, new Hashtable()}
                    }
                }
                }
            }
            }
        }
        };

        IoC.Resolve<Hwdtech.ICommand>(
        "IoC.Register",
        "Game.BuildTree",
        (object[] args) => Hashtree
        ).Execute();

    }

    [Fact]
    public void CheckCollision_Succefully()
    {
        var collisionCommand = new Mock<SpaceBattle.Lib.ICommand>();
        collisionCommand.Setup(c => c.Execute()).Verifiable("collisionCommand wasn't called");

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Collision",
            (object[] args) => collisionCommand.Object
        ).Execute();

        var obj1 = new Mock<IUObject>();
        var obj2 = new Mock<IUObject>();

        obj1.Setup(o => o.GetProperty("Position")).Returns(new int[] { 1, 1});
        obj2.Setup(o => o.GetProperty("Position")).Returns(new int[] { 2, 2 });
        obj1.Setup(o => o.GetProperty("Velocity")).Returns(new int[] { 0, 1 });
        obj2.Setup(o => o.GetProperty("Velocity")).Returns(new int[] { 0, 2 });

        var ccm = new CheckCollisionCommand(obj1.Object, obj2.Object);

        ccm.Execute();

        collisionCommand.VerifyAll();
    }
}

