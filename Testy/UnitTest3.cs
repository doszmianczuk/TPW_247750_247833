using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using BallSimulator;
using System.Linq;

[TestFixture]
public class DataServiceTests
{
    private IDataService _dataService;

    [SetUp]
    public void Setup()
    {
        _dataService = new Data();
    }

    [Test]
    public void AddBall_AddsBallToCollection()
    {
        var initialCount = _dataService.GetBalls().Count();
        var ball = new Model { Diameter = 5, Mass = 1 };
        _dataService.AddBall(ball);

        Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(initialCount + 1, _dataService.GetBalls().Count());
        Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(_dataService.GetBalls().Contains(ball));
    }

    [Test]
    public void RemoveBall_RemovesBallFromCollection()
    {
        var ball = new Model { Diameter = 5, Mass = 1 };
        _dataService.AddBall(ball);
        _dataService.RemoveBall(ball);

        Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsFalse(_dataService.GetBalls().Contains(ball));
    }

    [Test]
    public void LogDiagnostics_DoesNotAffectBallBehavior()
    {
        var data = new Data();
        data.AddBall(new Model { Diameter = 5, Mass = 1 });
        data.LogDiagnostics("diagnostics.json");

        var count = data.GetBalls().Count();
        NUnit.Framework.Assert.AreEqual(1, count);
    }

    [TestMethod]
    public void Concurrent_AddBall_ShouldNotThrowException()
    {
        var data = new Data();
        Parallel.For(0, 1000, i =>
        {
            data.AddBall(new Model { Diameter = 5, Mass = 1 });
        });

        var count = data.GetBalls().Count();
        NUnit.Framework.Assert.AreEqual(1000, count);
    }


}
