using Hwdtech;
using Hwdtech.Ioc;
using Moq;

namespace SpaceBattle.Lib.Tests;
public class BuildSolTreeTest
{
    public BuildSolTreeTest()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        var der = new Dictionary<int, object>();
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "BuildTree",
            (object[] args) =>
            {
                return der;
            }
        ).Execute();

    }

    [Fact]
    public void BuildSolTreeTestPositive()
    {
        var read = new Mock<IRead>();

        var path = "../../../test.txt";
        var vectors = File.ReadAllLines(path).Select(line => line.Split().Select(int.Parse).ToArray()).ToList();
        read.Setup(i => i.ReadFile()).Returns(vectors);

        var buildTree = new BuildSolTree(read.Object);
        buildTree.Execute();

        var decisionTree = IoC.Resolve<Dictionary<int, object>>("BuildTree");

        Assert.NotNull(decisionTree);
        Assert.True(decisionTree.ContainsKey(1));

        var tree = (IDictionary<int, object>)decisionTree[1];
        Assert.True(tree.ContainsKey(2));

        var tree2 = (Dictionary<int, object>)tree[2];
        Assert.True(tree2.ContainsKey(3));

        var tree3 = (Dictionary<int, object>)tree2[3];
        Assert.True(tree3.ContainsKey(4));
    }

    [Fact]
    public void BuildSolTreeTestNegative()
    {
        var read = new Mock<IRead>();
        read.Setup(x => x.ReadFile()).Returns(() => throw new Exception());

        var build = new BuildSolTree(read.Object);

        Assert.Throws<Exception>(() => build.Execute());
    }
}