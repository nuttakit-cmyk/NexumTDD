using System;
using System.Collections.Generic;
using Exam01;
using Moq;
using Xunit;

namespace Exam01.Tests;

public class SpecialNumberFinderTests
{
    public static IEnumerable<object[]> UnsortedValueCases()
    {
        // Normal: จำนวนคี่ -> ค่ากลาง
        yield return new object[] { new[] { 5, 1, 3 }, 3.0 };
        // Normal: จำนวนคู่ -> ค่าเฉลี่ยของสองค่ากลาง
        yield return new object[] { new[] { 7, 1, 5, 3 }, 4.0 };
        // Normal: ค่าเดียว
        yield return new object[] { new[] { 42 }, 42.0 };
        // Normal: มีค่าเท่ากันหลายตัว
        yield return new object[] { new[] { 3, 2, 2, 3 }, 2.5 };
        // Normal: ข้อมูลเรียงกลับด้าน
        yield return new object[] { new[] { 5, 4, 3, 2, 1 }, 3.0 };
    }

    public static IEnumerable<object?[]> ExceptionalCases()
    {
        // Exception: ว่าง
        yield return new object?[] { Array.Empty<int>(), 0.0 };
        // Exception: เป็น null
        yield return new object?[] { null, 0.0 };
    }

    [Theory(DisplayName = "คำนวณเลขพิเศษจากข้อมูลที่ไม่ได้เรียง")]
    [MemberData(nameof(UnsortedValueCases))]
    public void Find_WithUnsortedValues_ReturnsExpectedMedian(int[] values, double expected)
    {
        // Mock INumberGenerator
        var mockNumberGenerator = new Mock<INumberGenerator>();
        mockNumberGenerator.Setup(x => x.Random()).Returns(values);

        // Explicitly verify each expectation...
        mockNumberGenerator.Verify(mock => mock.Random(), Times.Once());

        // Arrange สิ่งที่เราจะเทส sut(system under test)
        var sut = new SpecialNumberFinder(mockNumberGenerator.Object);

        // Act เรียกส่วนที่เราจะทดสอบ
        double result = sut.FindSpecialNumber();

        // Assert ตรวจสอบผลลัพท์
        Assert.Equal(expected, result);
    }

    [Theory(DisplayName = "รองรับกรณีข้อมูลผิดปกติ (ค่าว่าง/ค่า null)")]
    [MemberData(nameof(ExceptionalCases))]
    public void Find_WithExceptionalValues_ReturnsZero(int[]? values, double expected)
    {
        // Mock INumberGenerator
        var mockNumberGenerator = new Mock<INumberGenerator>();
        mockNumberGenerator.Setup(x => x.Random()).Returns(values ?? Array.Empty<int>());

        // Explicitly verify each expectation...
        mockNumberGenerator.Verify(mock => mock.Random(), Times.Once());

        // Arrange สิ่งที่เราจะเทส
        var sut = new SpecialNumberFinder(mockNumberGenerator.Object);

        // Act เรียกส่วนที่เราจะทดสอบ
        double result = sut.FindSpecialNumber();

        // Assert ตรวจสอบผลลัพท์
        Assert.Equal(expected, result);
    }
}
