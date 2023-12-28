using Hwdtech;
using Hwdtech.Ioc;
using System.Collections;

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
        "Game.Object.GetProperty",
        (object[] args) =>
        {
            var obj = (IUObject)args[0];
            var key = (string)args[1];

            var vec = obj.GetProperty(key);
            return vec;
        }
        ).Execute();

        var Hashtree = new Hashtable(){
                {0, new Hashtable(){
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
}

