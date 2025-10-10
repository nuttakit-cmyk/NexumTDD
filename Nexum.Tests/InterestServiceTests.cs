//using Moq;
//using Nexum.Server.DAC;
//using Nexum.Server.Models;
//using Nexum.Server.Services;
//using Xunit;

//namespace Nexum.Tests
//{
//    public class InterestServiceTests
//    {
//        private readonly Mock<INexumConfigDAC> _mockNexumConfigDAC;
//        private readonly InterestService _interestService;

//        public InterestServiceTests()
//        {
//            _mockNexumConfigDAC = new Mock<INexumConfigDAC>();
//            _interestService = new InterestService(_mockNexumConfigDAC.Object);
//        }

//        #region Normal Cases - กรณีที่เกิดขึ้นบ่อยๆในระบบ

//        public static IEnumerable<object[]> PerMonthTestData()
//        {
//            // เงินต้น 10000 บาท อัตราดอกเบี้ย 15% ต่อปี ดอกเบี้ยสะสม 500 บาท
//            // 10000 * 0.15 = 1500
//            // ดอกเบี้ยรอบนี้ 1500 บาท ดอกเบี้ยสะสม 2000 บาท
//            // 500 + 1500 = 2000
//            yield return new object[] { 10000m, 0.15m, 500m, 1500m, 2000m };
//            // เงินต้น 5000 บาท อัตราดอกเบี้ย 12% ต่อปี ดอกเบี้ยสะสม 200 บาท
//            // 5000 * 0.12 = 600
//            // ดอกเบี้ยรอบนี้ 600 บาท ดอกเบี้ยสะสม 800 บาท
//            // 200 + 600 = 800
//            yield return new object[] { 5000m, 0.12m, 200m, 600m, 800m };
//            // เงินต้น 20000 บาท อัตราดอกเบี้ย 18% ต่อปี ดอกเบี้ยสะสม 1000 บาท
//            // 20000 * 0.18 = 3600
//            // ดอกเบี้ยรอบนี้ 3600 บาท ดอกเบี้ยสะสม 4600 บาท
//            // 1000 + 3600 = 4600
//            yield return new object[] { 20000m, 0.18m, 1000m, 3600m, 4600m };
//            // เงินต้น 1000000 บาท อัตราดอกเบี้ย 12% ต่อปี ดอกเบี้ยสะสม 50000 บาท
//            // 1000000 * 0.12 = 120000
//            // ดอกเบี้ยรอบนี้ 120000 บาท ดอกเบี้ยสะสม 170000 บาท
//            // 50000 + 120000 = 170000
//            yield return new object[] { 1000000m, 0.12m, 50000m, 120000m, 170000m };
//        }

//        [Theory]
//        [MemberData(nameof(PerMonthTestData))]
//        // ทดสอบการคำนวณดอกเบี้ยแบบรายเดือนด้วยยอดเงินต้นและอัตราดอกเบี้ยต่างๆ
//        public void CalculateInterest_PerMonthInterestType_ShouldCalculateCorrectInterest(
//            decimal principalBalance,
//            decimal interestRate,
//            decimal currentAccumulated,
//            decimal expectedInterest,
//            decimal expectedAccumulated)
//        {
//            // Arrange
//            var request = new CalculateInterestRequest
//            {
//                PrincipalBalance = principalBalance,
//                InterestRate = interestRate,
//                InterestType = "PerMonth",
//                ProductContactId = 1,
//                InterestFreePeriodDays = default(DateTime),
//                MaxInterestAmount = 1000000m // ตั้งค่าสูงเพื่อไม่ให้จำกัดการคำนวณดอกเบี้ย
//            };

//            var accumulatedInterest = new AccumulatedInterest
//            {
//                AccumulatedInterestId = 1,
//                ProductContactId = 1,
//                AccumInterestRemain = currentAccumulated
//            };

//            _mockNexumConfigDAC.Setup(x => x.GetAccumulatedInterest(1))
//                .Returns(accumulatedInterest);

//            // Act
//            var result = _interestService.CalculateInterest(request);

//            // Assert
//            Assert.Equal(expectedInterest, result.InterestAmount);
//            Assert.Equal(expectedAccumulated, result.AccumInterestRemain);

//            // Verify DAC calls
//            _mockNexumConfigDAC.Verify(x => x.GetAccumulatedInterest(1), Times.Once);
//            _mockNexumConfigDAC.Verify(x => x.CreateStatementInterest(It.IsAny<StatementInterest>()), Times.Once);
//            _mockNexumConfigDAC.Verify(x => x.UpdateAccumulatedInterest(expectedAccumulated), Times.Once);
//        }

//        public static IEnumerable<object[]> PerDayTestData()
//        {
//            // เงินต้น 10000 บาท อัตราดอกเบี้ย 15% ต่อปี ดอกเบี้ยสะสม 300 บาท
//            // 10000 * 0.15 / 365 = 4.11
//            // ดอกเบี้ยรอบนี้ 4.11 บาท ดอกเบี้ยสะสม 304.11 บาท
//            // 300 + 4.11 = 304.11
//            yield return new object[] { 10000m, 0.15m, 300m, 4.11m, 304.11m };
//            // เงินต้น 5000 บาท อัตราดอกเบี้ย 12% ต่อปี ดอกเบี้ยสะสม 100 บาท
//            // 5000 * 0.12 / 365 = 1.64
//            // ดอกเบี้ยรอบนี้ 1.64 บาท ดอกเบี้ยสะสม 101.64 บาท
//            // 100 + 1.64 = 101.64
//            yield return new object[] { 5000m, 0.12m, 100m, 1.64m, 101.64m };
//            // เงินต้น 20000 บาท อัตราดอกเบี้ย 18% ต่อปี ดอกเบี้ยสะสม 500 บาท
//            // 20000 * 0.18 / 365 = 9.86
//            // ดอกเบี้ยรอบนี้ 9.86 บาท ดอกเบี้ยสะสม 509.86 บาท
//            // 500 + 9.86 = 509.86
//            yield return new object[] { 20000m, 0.18m, 500m, 9.86m, 509.86m };
//            // เงินต้น 100000 บาท อัตราดอกเบี้ย 20% ต่อปี ดอกเบี้ยสะสม 1000 บาท
//            // 100000 * 0.20 / 365 = 54.79
//            // ดอกเบี้ยรอบนี้ 54.79 บาท ดอกเบี้ยสะสม 1054.79 บาท
//            // 1000 + 54.79 = 1054.79
//            yield return new object[] { 100000m, 0.20m, 1000m, 54.79m, 1054.79m };
//        }

//        [Theory]
//        [MemberData(nameof(PerDayTestData))]
//        // ทดสอบการคำนวณดอกเบี้ยแบบรายวันโดยแปลงจากอัตรารายปีเป็นรายวัน
//        public void CalculateInterest_PerDayInterestType_ShouldCalculateCorrectInterest(
//            decimal principalBalance,
//            decimal interestRate,
//            decimal currentAccumulated,
//            decimal expectedInterest,
//            decimal expectedAccumulated)
//        {
//            // Arrange
//            var request = new CalculateInterestRequest
//            {
//                PrincipalBalance = principalBalance,
//                InterestRate = interestRate,
//                InterestType = "PerDay",
//                ProductContactId = 1,
//                InterestFreePeriodDays = default(DateTime),
//                MaxInterestAmount = 1000000m // ตั้งค่าสูงเพื่อไม่ให้จำกัดการคำนวณดอกเบี้ย
//            };

//            var accumulatedInterest = new AccumulatedInterest
//            {
//                AccumulatedInterestId = 1,
//                ProductContactId = 1,
//                AccumInterestRemain = currentAccumulated
//            };

//            _mockNexumConfigDAC.Setup(x => x.GetAccumulatedInterest(1))
//                .Returns(accumulatedInterest);

//            // Act
//            var result = _interestService.CalculateInterest(request);

//            // Assert
//            Assert.Equal(expectedInterest, result.InterestAmount);
//            Assert.Equal(expectedAccumulated, result.AccumInterestRemain);

//            // Verify DAC calls
//            _mockNexumConfigDAC.Verify(x => x.GetAccumulatedInterest(1), Times.Once);
//            _mockNexumConfigDAC.Verify(x => x.CreateStatementInterest(It.IsAny<StatementInterest>()), Times.Once);
//            _mockNexumConfigDAC.Verify(x => x.UpdateAccumulatedInterest(expectedAccumulated), Times.Once);
//        }

//        public static IEnumerable<object[]> ZeroValuesTestData()
//        {
//            // กรณีที่ยอดเงินต้นเป็นศูนย์ อัตราดอกเบี้ย 15% ต่อเดือน ดอกเบี้ยสะสมคงเดิม 100 บาท
//            // 0 * 0.15 = 0
//            // ดอกเบี้ยรอบนี้ 0 บาท ดอกเบี้ยสะสมคงเดิม 100 บาท
//            yield return new object[] { 0m, 0.15m, 100m, 0m, 100m };
//            // กรณีที่อัตราดอกเบี้ยเป็นศูนย์ อัตราดอกเบี้ย 0% ต่อเดือน ดอกเบี้ยสะสมคงเดิม 200 บาท
//            // 10000 * 0 = 0
//            // ดอกเบี้ยรอบนี้ 0 บาท ดอกเบี้ยสะสมคงเดิม 200 บาท
//            yield return new object[] { 10000m, 0m, 200m, 0m, 200m };
//            // กรณีที่ทั้งยอดเงินต้นและอัตราดอกเบี้ยเป็นศูนย์ ดอกเบี้ยสะสมคงเดิม 50 บาท
//            // 0 * 0 = 0
//            // ดอกเบี้ยรอบนี้ 0 บาท ดอกเบี้ยสะสมคงเดิม 50 บาท
//            yield return new object[] { 0m, 0m, 50m, 0m, 50m };
//        }

//        [Theory]
//        [MemberData(nameof(ZeroValuesTestData))]
//        // ทดสอบกรณีพิเศษที่ควรได้ดอกเบี้ยเป็นศูนย์
//        public void CalculateInterest_ZeroValues_ShouldReturnZeroInterest(
//            decimal principalBalance,
//            decimal interestRate,
//            decimal currentAccumulated,
//            decimal expectedInterest,
//            decimal expectedAccumulated)
//        {
//            // Arrange
//            var request = new CalculateInterestRequest
//            {
//                PrincipalBalance = principalBalance,
//                InterestRate = interestRate,
//                InterestType = "PerMonth",
//                ProductContactId = 1,
//                InterestFreePeriodDays = default(DateTime),
//                MaxInterestAmount = 1000000m // ตั้งค่าสูงเพื่อไม่ให้จำกัดการคำนวณดอกเบี้ย
//            };

//            var accumulatedInterest = new AccumulatedInterest
//            {
//                AccumulatedInterestId = 1,
//                ProductContactId = 1,
//                AccumInterestRemain = currentAccumulated
//            };

//            _mockNexumConfigDAC.Setup(x => x.GetAccumulatedInterest(1))
//                .Returns(accumulatedInterest);

//            // Act
//            var result = _interestService.CalculateInterest(request);

//            // Assert
//            Assert.Equal(expectedInterest, result.InterestAmount);
//            Assert.Equal(expectedAccumulated, result.AccumInterestRemain);

//            // Verify DAC calls
//            _mockNexumConfigDAC.Verify(x => x.GetAccumulatedInterest(1), Times.Once);
//            _mockNexumConfigDAC.Verify(x => x.CreateStatementInterest(It.IsAny<StatementInterest>()), Times.Once);
//            _mockNexumConfigDAC.Verify(x => x.UpdateAccumulatedInterest(expectedAccumulated), Times.Once);
//        }

//        #endregion

//        #region Alternative Cases - กรณีที่เกิดขึ้นไม่ค่อยบ่อย นานๆจะเกิดขึ้นครั้ง

//        public static IEnumerable<object[]> InterestFreePeriodTestData()
//        {
//            // กรณีที่อยู่ในช่วงยกเว้นดอกเบี้ย (วันสิ้นสุดยังไม่มาถึง)
//            // วันสิ้นสุด = วันนี้ + 15 วัน
//            // ดอกเบี้ยรอบนี้ 0 บาท ดอกเบี้ยสะสมคงเดิม 500 บาท
//            // หมายเหตุ: "ยกเว้นการคำนวณดอกเบี้ย"
//            yield return new object[] { DateTime.Now.AddDays(15), 10000m, 0.15m, 500m, 0m, 500m, "ยกเว้นการคำนวณดอกเบี้ย", false };
//            // กรณีที่ช่วงยกเว้นดอกเบี้ยหมดอายุแล้ว (วันสิ้นสุดผ่านไปแล้ว)
//            // วันสิ้นสุด = วันนี้ - 5 วัน
//            // 10000 * 0.15 = 1500
//            // ดอกเบี้ยรอบนี้ 1500 บาท ดอกเบี้ยสะสม 1800 บาท
//            // 300 + 1500 = 1800
//            yield return new object[] { DateTime.Now.AddDays(-5), 10000m, 0.15m, 300m, 1500m, 1800m, "ดอกเบี้ยรอบนี้", true };
//            // กรณีที่วันสิ้นสุดช่วงยกเว้นดอกเบี้ยเป็นวันนี้พอดี
//            // วันสิ้นสุด = วันนี้
//            // 10000 * 0.15 = 1500
//            // ดอกเบี้ยรอบนี้ 1500 บาท ดอกเบี้ยสะสม 1700 บาท
//            // 200 + 1500 = 1700
//            yield return new object[] { DateTime.Now, 10000m, 0.15m, 200m, 1500m, 1700m, "ดอกเบี้ยรอบนี้", true };
//        }

//        [Theory]
//        [MemberData(nameof(InterestFreePeriodTestData))]
//        // ทดสอบกรณีต่างๆ ของช่วงยกเว้นดอกเบี้ย
//        public void CalculateInterest_InterestFreePeriodCases_ShouldCalculateCorrectly(
//            DateTime interestFreePeriodDays,
//            decimal principalBalance,
//            decimal interestRate,
//            decimal currentAccumulated,
//            decimal expectedInterest,
//            decimal expectedAccumulated,
//            string expectedRemark,
//            bool shouldUpdateAccumulated)
//        {
//            // Arrange
//            var request = new CalculateInterestRequest
//            {
//                PrincipalBalance = principalBalance,
//                InterestRate = interestRate,
//                InterestType = "PerMonth",
//                ProductContactId = 1,
//                InterestFreePeriodDays = interestFreePeriodDays,
//                MaxInterestAmount = 1000000m // ตั้งค่าสูงเพื่อไม่ให้จำกัดการคำนวณดอกเบี้ย
//            };

//            var accumulatedInterest = new AccumulatedInterest
//            {
//                AccumulatedInterestId = 1,
//                ProductContactId = 1,
//                AccumInterestRemain = currentAccumulated
//            };

//            _mockNexumConfigDAC.Setup(x => x.GetAccumulatedInterest(1))
//                .Returns(accumulatedInterest);

//            // Act
//            var result = _interestService.CalculateInterest(request);

//            // Assert
//            Assert.Equal(expectedInterest, result.InterestAmount);
//            Assert.Equal(expectedAccumulated, result.AccumInterestRemain);

//            // Verify DAC calls
//            _mockNexumConfigDAC.Verify(x => x.GetAccumulatedInterest(1), Times.Once);
//            _mockNexumConfigDAC.Verify(x => x.CreateStatementInterest(It.Is<StatementInterest>(s =>
//                s.InterestAmount == expectedInterest &&
//                s.Remark == expectedRemark)), Times.Once);

//            if (shouldUpdateAccumulated)
//            {
//                _mockNexumConfigDAC.Verify(x => x.UpdateAccumulatedInterest(expectedAccumulated), Times.Once);
//            }
//            else
//            {
//                _mockNexumConfigDAC.Verify(x => x.UpdateAccumulatedInterest(It.IsAny<decimal>()), Times.Never);
//            }
//        }

//        #endregion

//        #region Max Interest Amount - กรณีดอกเบี้ยเกินเพดานต่อรอบบิล

//        public static IEnumerable<object[]> MaxInterestAmountTestData()
//        {
//            // PerMonth: 1,000,000 * 0.20 = 200,000 แต่เพดาน = 150,000
//            // ดอกเบี้ยรอบนี้ = 150,000, ดอกเบี้ยสะสมใหม่ = 50,000 + 150,000 = 200,000
//            yield return new object[] { "PerMonth", 1000000m, 0.20m, 50000m, 150000m, 200000m, 150000m };

//            // PerDay: 1,000,000 * 0.365 / 365 = 1,000 แต่เพดาน = 500
//            // ดอกเบี้ยรอบนี้ = 500, ดอกเบี้ยสะสมใหม่ = 2,000 + 500 = 2,500
//            yield return new object[] { "PerDay", 1000000m, 0.365m, 2000m, 500m, 2500m, 500m };
//        }

//        [Theory]
//        [MemberData(nameof(MaxInterestAmountTestData))]
//        // ทดสอบกรณีที่ดอกเบี้ยคำนวณได้เกิน MaxInterestAmount ต้องถูกจำกัดตามเพดาน
//        public void CalculateInterest_ExceedsMaxInterestAmount_ShouldCapAtMax(
//            string interestType,
//            decimal principalBalance,
//            decimal interestRate,
//            decimal currentAccumulated,
//            decimal expectedInterest,
//            decimal expectedAccumulated,
//            decimal maxInterestAmount)
//        {
//            // Arrange
//            var request = new CalculateInterestRequest
//            {
//                PrincipalBalance = principalBalance,
//                InterestRate = interestRate,
//                InterestType = interestType,
//                ProductContactId = 1,
//                InterestFreePeriodDays = default(DateTime),
//                MaxInterestAmount = maxInterestAmount
//            };

//            var accumulatedInterest = new AccumulatedInterest
//            {
//                AccumulatedInterestId = 1,
//                ProductContactId = 1,
//                AccumInterestRemain = currentAccumulated
//            };

//            _mockNexumConfigDAC.Setup(x => x.GetAccumulatedInterest(1))
//                .Returns(accumulatedInterest);

//            // Act
//            var result = _interestService.CalculateInterest(request);

//            // Assert
//            Assert.Equal(expectedInterest, result.InterestAmount);
//            Assert.Equal(expectedAccumulated, result.AccumInterestRemain);

//            // Verify DAC calls และตรวจสอบ Remark ว่าเป็นข้อความกรณีเกินเพดาน
//            _mockNexumConfigDAC.Verify(x => x.GetAccumulatedInterest(1), Times.Once);
//            _mockNexumConfigDAC.Verify(x => x.CreateStatementInterest(It.Is<StatementInterest>(s =>
//                s.InterestAmount == expectedInterest &&
//                s.AccumulatedAmount == expectedAccumulated &&
//                s.Remark == "ดอกเบี้ยรอบนี้สูงกว่าอัตราดอกเบี้ยสูงสุดต่อรอบบิล")), Times.Once);
//            _mockNexumConfigDAC.Verify(x => x.UpdateAccumulatedInterest(expectedAccumulated), Times.Once);
//        }

//        #endregion

//        #region Exception Cases - กรณีที่เจอข้อผิดแปลกจากสิ่งที่มันควรจะเป็น

//        public static IEnumerable<object[]> ValidationErrorTestData()
//        {
//            // กรณีที่ยอดเงินต้นเป็นค่าลบ
//            // ระบบควร throw ArgumentException พร้อมข้อความ "PrincipalBalance cannot be negative"
//            yield return new object[] { -1000m, 0.15m, "PerMonth", "PrincipalBalance", "PrincipalBalance cannot be negative" };
//            // กรณีที่อัตราดอกเบี้ยเป็นค่าลบ
//            // ระบบควร throw ArgumentException พร้อมข้อความ "InterestRate cannot be negative"
//            yield return new object[] { 10000m, -0.15m, "PerMonth", "InterestRate", "InterestRate cannot be negative" };
//            // กรณีที่ InterestType เป็น null (ตั้งใจ)
//            // ระบบควร throw ArgumentException พร้อมข้อความ "InterestType is required"
//            yield return new object[] { 10000m, 0.15m, null!, "InterestType", "InterestType is required" };
//            // กรณีที่ InterestType เป็น empty string
//            // ระบบควร throw ArgumentException พร้อมข้อความ "InterestType is required"
//            yield return new object[] { 10000m, 0.15m, "", "InterestType", "InterestType is required" };
//            // กรณีที่ InterestType เป็น whitespace เท่านั้น
//            // ระบบควร throw ArgumentException พร้อมข้อความ "InterestType is required"
//            yield return new object[] { 10000m, 0.15m, "   ", "InterestType", "InterestType is required" };
//            // กรณีที่ InterestType ไม่ถูกต้อง (ไม่ใช่ PerMonth หรือ PerDay)
//            // ระบบควร throw ArgumentException พร้อมข้อความ "InterestType is invalid"
//            yield return new object[] { 10000m, 0.15m, "InvalidType", "InterestType", "InterestType is invalid" };
//            // กรณีที่ MaxInterestAmount เป็นค่าติดลบ
//            // ระบบควร throw ArgumentException พร้อมข้อความ "MaxInterestAmount cannot be negative"
//            // หมายเหตุ: ใช้ interestType ที่ถูกต้องเพื่อให้ชน validation ที่ MaxInterestAmount
//            yield return new object[] { 10000m, 0.15m, "PerMonth", "MaxInterestAmount", "MaxInterestAmount cannot be negative" };
//        }

//        [Theory]
//        [MemberData(nameof(ValidationErrorTestData))]
//        // ทดสอบกรณี validation errors ต่างๆ
//        public void CalculateInterest_ValidationErrors_ShouldThrowArgumentException(
//            decimal principalBalance,
//            decimal interestRate,
//            string interestType,
//            string expectedParamName,
//            string expectedMessage)
//        {
//            // Arrange
//            var request = new CalculateInterestRequest
//            {
//                PrincipalBalance = principalBalance,
//                InterestRate = interestRate,
//                InterestType = interestType,
//                ProductContactId = 1,
//                InterestFreePeriodDays = default(DateTime),
//                MaxInterestAmount = expectedParamName == "MaxInterestAmount" ? -1m : 1000000m // เจาะจงให้ติดลบเมื่อทดสอบเคสนี้
//            };

//            // Act & Assert
//            var exception = Assert.Throws<ArgumentException>(() =>
//                _interestService.CalculateInterest(request));

//            Assert.Equal(expectedParamName, exception.ParamName);
//            Assert.Contains(expectedMessage, exception.Message);

//            // Verify DAC is not called when validation fails
//            _mockNexumConfigDAC.Verify(x => x.GetAccumulatedInterest(It.IsAny<int>()), Times.Never);
//            _mockNexumConfigDAC.Verify(x => x.CreateStatementInterest(It.IsAny<StatementInterest>()), Times.Never);
//            _mockNexumConfigDAC.Verify(x => x.UpdateAccumulatedInterest(It.IsAny<decimal>()), Times.Never);
//        }

//        [Fact]
//        // ทดสอบกรณีที่ส่ง request เป็น null ควร throw ArgumentNullException
//        public void CalculateInterest_NullRequest_ShouldThrowArgumentNullException()
//        {
//            // Arrange
//            CalculateInterestRequest? request = null;

//            // Act & Assert
//            var exception = Assert.Throws<ArgumentNullException>(() =>
//                _interestService.CalculateInterest(request!));

//            Assert.Equal("calculateInterestRequest", exception.ParamName);
//            Assert.Contains("Request cannot be null", exception.Message);
//        }



//        [Fact]
//        // ทดสอบกรณีที่ DAC throw exception ควรส่งต่อ exception นั้น
//        public void CalculateInterest_DACThrowsException_ShouldPropagateException()
//        {
//            // Arrange
//            var request = new CalculateInterestRequest
//            {
//                PrincipalBalance = 10000m,
//                InterestRate = 0.15m,
//                InterestType = "PerMonth",
//                ProductContactId = 1,
//                InterestFreePeriodDays = default(DateTime),
//                MaxInterestAmount = 1000000m // ตั้งค่าสูงเพื่อไม่ให้จำกัดการคำนวณดอกเบี้ย
//            };

//            _mockNexumConfigDAC.Setup(x => x.GetAccumulatedInterest(1))
//                .Throws(new InvalidOperationException("Database connection failed"));

//            // Act & Assert
//            var exception = Assert.Throws<InvalidOperationException>(() =>
//                _interestService.CalculateInterest(request));

//            Assert.Contains("Database connection failed", exception.Message);
//        }

//        [Fact]
//        // ทดสอบกรณีที่มี validation errors หลายตัว ควร throw error แรกที่เจอ
//        public void CalculateInterest_MultipleValidationErrors_ShouldThrowFirstError()
//        {
//            // Arrange
//            var request = new CalculateInterestRequest
//            {
//                PrincipalBalance = -1000m, // Negative balance
//                InterestRate = -0.15m,    // Negative rate
//                InterestType = null,      // Null type
//                ProductContactId = 1,
//                InterestFreePeriodDays = default(DateTime),
//                MaxInterestAmount = 1000000m // ตั้งค่าสูงเพื่อไม่ให้จำกัดการคำนวณดอกเบี้ย
//            };

//            // Act & Assert
//            var exception = Assert.Throws<ArgumentException>(() =>
//                _interestService.CalculateInterest(request));

//            // Should throw the first validation error (PrincipalBalance)
//            Assert.Equal("PrincipalBalance", exception.ParamName);
//            Assert.Contains("PrincipalBalance cannot be negative", exception.Message);
//        }

//        #endregion
//    }
//}
