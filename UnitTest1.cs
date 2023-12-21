using System.Linq.Expressions;
using FoxLottery.DB;
using FoxORM;

namespace UnitTestMOD;

public class Tests
{
    private FoxOrm _foxOrm = null!;

    [SetUp]
    public void Setup()
    {
        _foxOrm = new FoxOrm("./testSqlite/test.sqlite");
        _foxOrm.RegisterTable<LotteryModel>();
        _foxOrm.RegisterTable<TicketModel>();
    }

    [Test, Order(1)]
    public async Task TestSave()
    {
        // Arrange
        var lotteryModel = new LotteryModel { montant = 10, enterpriseName = "ABC Corp", bizID = 1, price = 10.0f };

        // Act
        var result = await _foxOrm.Save<LotteryModel>(lotteryModel);

        // Assert 
        Assert.That(result, Is.True);
    }
    
    [Test, Order(2)]
    public async Task TestQueryByID()
    {
        // Arrange
        var lotteryModel = new LotteryModel { montant = 20, enterpriseName = "XYZ Corp", bizID = 2, price = 20.0f };
        await _foxOrm.Save<LotteryModel>(lotteryModel);

        // Act
        var queriedLottery = await _foxOrm.Query<LotteryModel>(2);

        // Assert
        Assert.That(queriedLottery, Is.Not.Null);
        Assert.That(queriedLottery.enterpriseName, Is.EqualTo("XYZ Corp"));
    }

    [Test, Order(3)]
    public async Task TestQueryAll()
    {
        // Act
        var allLotteries = await _foxOrm.QueryAll<LotteryModel>();

        // Assert
        Assert.That(allLotteries, Is.Not.Null);
        Assert.That(allLotteries, Has.Count.GreaterThanOrEqualTo(2)); // at least two lotteries should be inserted by now
    }
    
    [Test, Order(4)]
    public async Task TestJoinAsync()
    {
        // Arrange
        Expression<Func<LotteryModel, bool>> predicate = (lottery) => lottery.montant > 10;
        // Act
        var result = await _foxOrm.JoinAsync<LotteryModel, TicketModel>(predicate);
        // Assert
        Assert.That(result, Is.Not.Null);
    }
    
    [Test, Order(5)]
    public async Task TestDelete()
    {
        // Arrange
        var lotteryModel = new LotteryModel { montant = 30, enterpriseName = "DEF Corp", bizID = 3, price = 30.0f };
        await _foxOrm.Save<LotteryModel>(lotteryModel);
        // Act
        var result = await _foxOrm.Delete<LotteryModel>(lotteryModel);
        // Assert
        Assert.That(result, Is.True);
    }
}