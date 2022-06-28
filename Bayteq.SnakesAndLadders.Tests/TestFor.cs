using NUnit.Framework;

namespace Bayteq.SnakesAndLadders.Tests;

[TestFixture]
public abstract class TestFor<T> where T : class
{
    protected T Subject { get; set; }

    [SetUp]
    protected void TestForSetupAsync()
    {
        Setup();
        Subject = Given();
        WhenAsync();
    }

    protected virtual void Setup() { }

    protected abstract T Given();

    protected abstract void WhenAsync();

    [TearDown]
    protected virtual void TearDown()
    {
    }
}