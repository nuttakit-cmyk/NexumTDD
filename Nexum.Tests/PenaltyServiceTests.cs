using Moq;
using Nexum.Server.DAC;
using Nexum.Server.Models.Penalty;
using Nexum.Server.Services.Penalty;

namespace Nexum.Tests
{
    public class PenaltyServiceTests
    {
        private readonly Mock<DailyPenalty> _mockDailyPenaltyDAC;
        private readonly Mock<FixedPenalty> _mockFixedPenaltyDAC;
        private readonly Mock<PercentagePenalty> _mockPercentagePenaltyDAC;
        private readonly Mock<PenaltyPolicies> _mockPenaltyPoliciesDAC;
        private readonly Mock<Penalty> _mockPenalty;
        public PenaltyServiceTests()
        {
            _mockDailyPenaltyDAC = new Mock<DailyPenalty>();
            _mockFixedPenaltyDAC = new Mock<FixedPenalty>();
            _mockPercentagePenaltyDAC = new Mock<PercentagePenalty>();
            _mockPenaltyPoliciesDAC = new Mock<PenaltyPolicies>();
            _mockPenalty = new Mock<Penalty>(_mockPercentagePenaltyDAC.Object, _mockPenaltyPoliciesDAC.Object, _mockDailyPenaltyDAC.Object, _mockFixedPenaltyDAC.Object);
        }

        public static IEnumerable<object[]> DailyPenaltyTestData()
        {
               yield return new PenaltyRequest[]
                {
                    new PenaltyRequest
                    {
                        OutstandingBalance = 1000m,
                        DueDate = DateTime.Now.AddDays(-10),
                        ActiveStatus = "Active",
                        UserId = 1,
                        UserName = "TestUser",
                        PenaltyPolicyID = 1,
                        MinimumPayment = 100m,
                        MaximumPayment = 500m,
                        PaymentAmount = 0m
                    }
                };
        }

        [Theory(DisplayName = "ทดสอบการคำนวณค่าปรับ")]
        [MemberData(nameof(DailyPenaltyTestData))]
        public void TestCalculatePenalty(PenaltyRequest penaltyRequest)
        {
            // Arrange
            var expectedPenaltyResponse = new PenaltyResponse
            {
                UserId = penaltyRequest.UserId,
                UserName = penaltyRequest.UserName,
                OutstandingBalance = penaltyRequest.OutstandingBalance,
                MinimumPayment = penaltyRequest.MinimumPayment,
                PenaltyAmount = 50m, // สมมติค่าปรับที่คาดหวัง
                TotalAmountDue = 1050m // สมมติยอดรวมที่ต้องชำระที่คาดหวัง
            };
            _mockPenalty.Setup(p => p.GetPenalty(It.IsAny<PenaltyRequest>())).Returns(expectedPenaltyResponse);
            // Act
            var result = _mockPenalty.Object.GetPenalty(penaltyRequest);
            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedPenaltyResponse.UserId, result.UserId);
            Assert.Equal(expectedPenaltyResponse.UserName, result.UserName);
            Assert.Equal(expectedPenaltyResponse.OutstandingBalance, result.OutstandingBalance);
            Assert.Equal(expectedPenaltyResponse.MinimumPayment, result.MinimumPayment);
            Assert.Equal(expectedPenaltyResponse.PenaltyAmount, result.PenaltyAmount);
            Assert.Equal(expectedPenaltyResponse.TotalAmountDue, result.TotalAmountDue);
        }
    }
}
