
using System.Reflection;
using UnitTesting02.Projects;
using Xunit;

namespace UnitTesting02.ProjectsTests;

public class IssueTests
{
    [Fact]
    public void Constructor_IssueWithDiscIsNull_ThrowsInvalidIssueDescriptionException()
    {
        //Arrange

        //Act
        Action ctor= () => new Issue(null ,Priority.Urgent,Category.Software,DateTime.Now );
        //Assert
        Assert.Throws<InvalidIssueDescriptionException>(()=>ctor());
    }

    [Fact]
    public void Constructor_IssueWithDiscIsWhitespace_ThrowsInvalidIssueDescriptionException()
    {
        //Arrange

        //Act
        Action ctor = () => new Issue(" ", Priority.Urgent, Category.Software, DateTime.Now);
        //Assert
        Assert.Throws<InvalidIssueDescriptionException>(() => ctor());
    }

    [Fact]
    public void Constructor_IssueWithCreatedAtIsNull_ReturnsCurrentDateTime()
    {
        //Arrange
        var sut = new Issue("DateTime is null", Priority.Urgent, Category.Software);

        //Act
        var actual=sut.CreatedAt;

        //Assert
        Assert.False(actual == default(DateTime));
    }

    [Fact]
    public void Constructor_IssueWithValidProperties_Return18CharIssueKey()
    {
        //Arrange
        var sut = new Issue("HardWare", Priority.Urgent, Category.Hardware,new DateTime (2002,10,30,12,00,00));

        //Act
        MethodInfo method = typeof(Issue).GetMethod("GenerateKey",BindingFlags.NonPublic | BindingFlags.Instance);
        var actual=method.Invoke(sut,null).ToString();
        var expectd = "HW-2002-U-ABCD1234";

        //Assert
        Assert.Equal(actual.Length,expectd.Length);
    }

    [Fact]
    public void Constructor_IssueWithSoftWareCategory_ReturnSWinFirstSegment()
    {
        //Arrange
        var sut = new Issue("SoftWare Issue", Priority.High, Category.Software, new DateTime(2002, 10, 30, 12, 00, 00));

        //Act
        MethodInfo method = typeof(Issue).GetMethod("GenerateKey", BindingFlags.NonPublic | BindingFlags.Instance);
        var actual = method.Invoke(sut, null).ToString().Split("-")[0];
        var expectd = "SW";

        //Assert
        Assert.Equal(actual, expectd);
    }

    [Fact]
    public void Constructor_IssueWithDateTime_ReturnYYYYinSecondSegment()
    {
        //Arrange
        var sut = new Issue("DateTime Issue", Priority.Urgent, Category.Software, new DateTime(2002, 10, 30, 12, 00, 00));

        //Act
        MethodInfo method = typeof(Issue).GetMethod("GenerateKey", BindingFlags.NonPublic | BindingFlags.Instance);
        var actual = method.Invoke(sut, null).ToString().Split("-")[1];
        var expectd = "2002";

        //Assert
        Assert.Equal(actual, expectd);
    }

    [Fact]
    public void Constructor_IssueWithPriorityUrgent_ReturnUinThirdSegment()
    {
        //Arrange
        var sut = new Issue("Priority Urgent Issue", Priority.Urgent, Category.Software, new DateTime(2002, 10, 30, 12, 00, 00));

        //Act
        MethodInfo method = typeof(Issue).GetMethod("GenerateKey", BindingFlags.NonPublic | BindingFlags.Instance);
        var actual = method.Invoke(sut, null).ToString().Split("-")[2];
        var expectd = "U";

        //Assert
        Assert.Equal(actual, expectd);
    }

    [Fact]
    public void Constructor_IssueWithFourthSegment_ReturnFourthSegmentInAlphaNumeric()
    {
        //Arrange
        var sut = new Issue("Fourt Segment Issue", Priority.Urgent, Category.Software, new DateTime(2002, 10, 30, 12, 00, 00));

        //Act
        MethodInfo method = typeof(Issue).GetMethod("GenerateKey", BindingFlags.NonPublic | BindingFlags.Instance);
        var actual = method.Invoke(sut, null).ToString().Split("-")[3];
        var isAlphaNum = actual.All(c => char.IsLetterOrDigit(c));
        

        //Assert
        Assert.True(isAlphaNum);
    }

    [Theory]
    [InlineData("HardWare Issue", Priority.Urgent, Category.Hardware, "2000-03-25","HW-2000-U-ABCD1234")]
    [InlineData("SoftWAre Issue", Priority.Urgent, Category.Software, "2002-10-30","SW-2002-U-ABCD1234")]
    [InlineData("UnKnown Issue",Priority.Low, Category.UnKnown, "2000-03-25", "NA-2000-L-ABCD1234")]
    [InlineData("Urgent Issue",Priority.Urgent, Category.Hardware, "2002-10-30","HW-2002-U-ABCD1234")]
    [InlineData("High Issue",Priority.High, Category.Software, "2000-03-25","SW-2000-H-ABCD1234")]
    [InlineData("Low Issue",Priority.Low, Category.UnKnown, "1996-02-07","NA-1996-L-ABCD1234")]
    [InlineData("Issue #7",Priority.Low, Category.Software, "1961-02-08","SW-1961-L-ABCD1234")]
    public void Constructor_IssueWithValidProperties_ReturnsExpectedKey(string desc, Priority priority, Category category, string createdAt,string expected)
    {
        //Arrange
        var sut = new Issue(desc, priority, category, DateTime.Parse(createdAt));

        //Act
        MethodInfo method = typeof(Issue).GetMethod("GenerateKey", BindingFlags.NonPublic | BindingFlags.Instance);
        var actual = method.Invoke(sut, null).ToString().Substring(0,10);
        var expect = expected.Substring(0, 10);


        //Assert
        Assert.Equal(actual, expect);
    }

}
