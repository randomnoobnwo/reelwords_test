using ReelWords;
using Xunit;

namespace ReelWordsTests;

public class ViewTests
{
    [Fact]
    public void TestInit()
    {
        var view = new ConsoleView();
        view.Init(3);
        Assert.Equal(3, view.Letters.Length);
    }
    
    [Fact]
    public void TestUpdateScore()
    {
        var view = new ConsoleView();
        view.Init(1);
        view.UpdateScore(10);
        Assert.Equal(10, view.Score);
    }
    
    [Fact]
    public void TestUpdateStatus()
    {
        var view = new ConsoleView();
        view.Init(1);
        view.UpdateStatus("Test status");
        Assert.Equal("Test status", view.Status);
    }
    
    [Fact]
    public void TestAnimateShift()
    {
        var view = new ConsoleView();
        view.Init(3);
        view.AnimateShift(0, 'a');
        Assert.Equal('a', view.Letters[0]);
    }
}