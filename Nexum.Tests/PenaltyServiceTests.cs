using Moq;
using Nexum.Server.DAC;
using Nexum.Server.Models.Penalty;
using Nexum.Server.Services.Penalty;

namespace Nexum.Tests
{
    public record Case(
        PenaltyRequest Request,
        PenaltyPoliciesResponse Policy,
        PenaltyResponse Expected
    );
    public class PenaltyServiceTests
    {
        private readonly Mock<IPercentagePenalty> _percentage = new();
        private readonly Mock<IPenaltyPolicies> _policies = new();
        private readonly Mock<IDailyPenalty> _daily = new();
        private readonly Mock<IFixedPenalty> _fixed = new();

        private Penalty CreateSut(PenaltyPoliciesResponse policy)
        {
            _policies.Setup(p => p.penaltyPolicies(It.IsAny<PenaltyPoliciesRequest>()))
                     .Returns(policy);
            return new Penalty(_percentage.Object, _policies.Object, _daily.Object, _fixed.Object);
        }

        public static IEnumerable<object[]> Cases()
        {
            var now = DateTime.Now.Date;
            var req = new PenaltyRequest
            {
                UserId = 1,
                OutstandingBalance = 500m,
                PaymentAmount = 0m,            // = 10%
                DueDate = now.AddDays(-9),        // ยังไม่ถึงกำหนด
                ActiveStatus = "Active",
                PenaltyPolicyID = 1
            };
            var policy = new PenaltyPoliciesResponse
            {
                PenaltyPolicyID = 2,
                PenaltyType = "Daily",
                FixedAmount = 100m,
                TotalCap = 1000m,
                GracePeriodDays = 5
            };
            PenaltyResponse expected = new PenaltyResponse
            {
                UserId = 1,
                OutstandingBalance = 500m,
                MinimumPayment = 50m,
                PenaltyAmount = 500m,
            };

            yield return new object[] { new Case(req, policy, expected) };
        }

        [Theory(DisplayName = "คำนวณค่าปรับตามสัญญา")]
        [MemberData(nameof(Cases))]
        public void CalculatePenalty_NormalCase(Case c)
        {
            //Arrange
            var sut = CreateSut(c.Policy);

            var now = DateTime.Now.Date;
            var minPayment = c.Request.OutstandingBalance * 0.1m;
            var isOverdue = c.Request.DueDate < now;
            var underMinPayment = c.Request.PaymentAmount < minPayment;
            var overdueDays = Math.Max(0, (now - c.Request.DueDate.Date).Days);
            var beyondGracePeriod = overdueDays > c.Policy.GracePeriodDays;

            var expectCalculatorCall = (isOverdue || underMinPayment) && beyondGracePeriod && c.Expected.PenaltyAmount > 0m;
            // ตั้ง expected ของ calculators ต่อเคส

            if (expectCalculatorCall)
            {
                switch (c.Policy.PenaltyType)
                {
                    case "Daily":
                        _daily.Setup(d => d.Calculate(It.IsAny<PenaltyContext>()))
                              .Returns(c.Expected.PenaltyAmount);
                        break;
                    case "Percentage":
                        _percentage.Setup(p => p.Calculate(It.IsAny<PenaltyContext>()))
                                   .Returns(c.Expected.PenaltyAmount);
                        break;
                    case "Fixed":
                        _fixed.Setup(f => f.Calculate(It.IsAny<PenaltyContext>()))
                              .Returns((PenaltyContext ctx) => ctx.FixedAmount);
                        break;
                }
            }

            //Act
            var result = sut.GetPenalty(c.Request);

            //Assert
            Console.WriteLine($"[EXP] UserId={c.Expected.UserId}, " +
                  $"Outstanding={c.Expected.OutstandingBalance}, Min={c.Expected.MinimumPayment}, " +
                  $"Penalty={c.Expected.PenaltyAmount}");
            Console.WriteLine($"[ACT] UserId={result.UserId}," +
                                $"Outstanding={result.OutstandingBalance}, Min={result.MinimumPayment}, " +
                              $"Penalty={result.PenaltyAmount}");
            Assert.NotNull(result);
            Assert.Equal(c.Expected.UserId, result.UserId);
            Assert.Equal(c.Expected.OutstandingBalance, result.OutstandingBalance);
            Assert.Equal(c.Expected.MinimumPayment, result.MinimumPayment);
            Assert.Equal(c.Expected.PenaltyAmount, result.PenaltyAmount);

            _daily.Verify(d => d.Calculate(It.IsAny<PenaltyContext>()),
                c.Policy.PenaltyType == "Daily" ? Times.Once() : Times.Never());
            _fixed.Verify(f => f.Calculate(It.IsAny<PenaltyContext>()),
                c.Policy.PenaltyType == "Fixed" ? Times.Once() : Times.Never());
            _percentage.Verify(p => p.Calculate(It.IsAny<PenaltyContext>()),
                c.Policy.PenaltyType == "Percentage" ? Times.Once() : Times.Never());
        }

        //private readonly Mock<IPenalty> _mockPenalty;
        //public PenaltyServiceTests()
        //{
        //    _mockPenalty = new Mock<IPenalty>();
        //}

        //public static IEnumerable<object[]> DailyPenaltyTestData()
        //{
        //    yield return new object[]
        //     {
        //            new MockPenalty
        //            {
        //                UserId = 1,
        //                UserName = "TestUser",
        //                OutstandingBalance = 1000m,
        //                MinimumPayment = 100m,
        //                MaximumPayment = 500m,
        //                PenaltyAmount = 0m,
        //                TotalAmountDue = 1100m,
        //                DueDate = DateTime.Now.AddDays(-10),
        //                ActiveStatus = "Active",
        //                PenaltyPolicyID = 1,
        //                //Expected values for assertions
        //                ExpectedUserId = 1, 
        //                ExpectedUserName = "TestUser",
        //                ExpectedOutstandingBalance = 1000m,
        //                ExpectedPenaltyAmount = 100m, // สมมติค่าปรับที่คาดหวัง
        //                ExpectedTotalAmountDue = 1100m, // สมมติยอดรวมที่ต้องชำระที่คาดหวัง  
        //            }
        //     };
        //}

        //[Theory(DisplayName = "ทดสอบการคำนวณค่าปรับ")]
        //[MemberData(nameof(DailyPenaltyTestData))]
        //public void TestCalculatePenalty(MockPenalty testValue)
        //{
        //    // Arrange
        //    var penaltyRequestData = new PenaltyRequest
        //    {
        //        UserId = testValue.UserId,
        //        UserName = testValue.UserName,
        //        OutstandingBalance = testValue.OutstandingBalance,
        //        MinimumPayment = testValue.MinimumPayment,
        //        MaximumPayment = testValue.MaximumPayment,
        //        DueDate = testValue.DueDate,
        //        ActiveStatus = testValue.ActiveStatus,
        //        PenaltyPolicyID = testValue.PenaltyPolicyID
        //    };
        //    var expectedPenaltyResponse = new PenaltyResponse
        //    {
        //        UserId = testValue.UserId,
        //        UserName = testValue.UserName,
        //        OutstandingBalance = testValue.OutstandingBalance,
        //        MinimumPayment = testValue.MinimumPayment,
        //        PenaltyAmount = testValue.PenaltyAmount, // สมมติค่าปรับที่คาดหวัง
        //        TotalAmountDue = testValue.TotalAmountDue // สมมติยอดรวมที่ต้องชำระที่คาดหวัง
        //    };
        //    _mockPenalty.Setup(p => p.GetPenalty(It.IsAny<PenaltyRequest>())).Returns(expectedPenaltyResponse);
        //    // Act
        //    var result = _mockPenalty.Object.GetPenalty(penaltyRequestData);
        //    Console.WriteLine($"Expected: {expectedPenaltyResponse.PenaltyAmount}, UserName: {expectedPenaltyResponse.TotalAmountDue}");
        //    Console.WriteLine($"Penalty Amount: {result.PenaltyAmount}, Total Amount Due: {result.TotalAmountDue}");
        //    // Assert
        //    Assert.NotNull(result);
        //    Assert.Equal(testValue.ExpectedUserId, result.UserId);
        //    Assert.Equal(testValue.ExpectedUserName, result.UserName);
        //    Assert.Equal(testValue.ExpectedOutstandingBalance, result.OutstandingBalance);
        //    Assert.Equal(testValue.ExpectedPenaltyAmount, result.PenaltyAmount);
        //    Assert.Equal(testValue.ExpectedTotalAmountDue, result.TotalAmountDue);

        //    _mockPenalty.Verify(p => p.GetPenalty(It.IsAny<PenaltyRequest>()), Times.Once);
        //}
    }
}
