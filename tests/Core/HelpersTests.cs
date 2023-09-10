using System.Text.RegularExpressions;
using Oluso.Helpers;
using Shouldly;
using Tests.Fakes;

namespace Tests.Core;

public class HelpersTests
{
    #region Embedded Resource Manager

    [Fact]
    public void Helpers_EmbeddedResourceManager_Sync()
    {
        const string value = "test value";
        var content = EmbeddedResourceManager.ReadString("Tests.test.txt", typeof(HelpersTests));
        content.ShouldNotBeNull();
        content.ShouldBe(value);
    }

    [Fact]
    public async Task Helpers_EmbeddedResourceManager_Async()
    {
        const string value = "test value";
        var content = await EmbeddedResourceManager.ReadStringAsync("Tests.test.txt", typeof(HelpersTests));
        content.ShouldNotBeNull();
        content.ShouldBe(value);
    }

    #endregion

    #region Random Generator

    [Fact]
    public void HelpersTests_RandomGenerator()
    {
        var r1 = RandomGenerator.NewRandomString();
        var r2 = RandomGenerator.NewRandomString();
        r1.ShouldNotBe(r2);
    }

    [Fact]
    public void HelpersTests_RandomGenerator_Length()
    {
        var r1 = RandomGenerator.NewRandomString(2);
        var r2 = RandomGenerator.NewRandomString(100);
        r1.Length.ShouldBe(2);
        r2.Length.ShouldBe(100);
    }

    [Fact]
    public void HelpersTests_RandomGenerator_Codebase()
    {
        var r1 = RandomGenerator.NewRandomString(3, "AAA");
        r1.ShouldBe("AAA");
    }

    #endregion

    #region SysVersion

    [Fact]
    public void HelpersTests_SysVersion()
    {
        // THIS TEST COULD FAIL IF Shouldly version is not 4.0.3
        // check the Test.csproj to check this out if the package was upgrade
        // change this value to the one in the Test.csproj
        var version = SysVersion.AssemblyVersion(typeof(Shouldly.Should));
        version.ShouldNotBeNull();
        version.ShouldBe("4.0.3.0");
    }

    #endregion
    
    #region Regexp

    [Fact]
    public void Helpers_Regexp_ForDate()
    {
        Regex.IsMatch("09/09/2023 17:30:00", Regexp.ForDate).ShouldBeTrue();
        Regex.IsMatch("09/09/2023 23:30:00", Regexp.ForDate).ShouldBeTrue();
        Regex.IsMatch("59/09/2023 17:30:00", Regexp.ForDate).ShouldBeFalse();
        Regex.IsMatch("09/09/2023 33:30:00", Regexp.ForDate).ShouldBeFalse();
        Regex.IsMatch("09/13/2023 33:30:00", Regexp.ForDate).ShouldBeFalse();
    }

    [Fact]
    public void Helpers_Regexp_Decimal()
    {
        Regex.IsMatch("3,14", Regexp.ForDecimal).ShouldBeTrue();
        Regex.IsMatch("3", Regexp.ForDecimal).ShouldBeTrue();
        Regex.IsMatch("3.14", Regexp.ForDecimal).ShouldBeTrue();
    }

    [Fact]
    public void Helpers_Regexp_Email()
    {
        Regex.IsMatch("user@email.com", Regexp.ForEmail).ShouldBeTrue();
        Regex.IsMatch("user.lastname@email.com", Regexp.ForEmail).ShouldBeTrue();
        Regex.IsMatch("user.lastname@email", Regexp.ForEmail).ShouldBeFalse();
        Regex.IsMatch("@email", Regexp.ForEmail).ShouldBeFalse();
        Regex.IsMatch("@email.com", Regexp.ForEmail).ShouldBeFalse();
    }

    [Fact]
    public void Helpers_Regexp_Hex()
    {
        Regex.IsMatch("#f4d", Regexp.ForHex).ShouldBeTrue();
        Regex.IsMatch("#f4dffe", Regexp.ForHex).ShouldBeTrue();
        Regex.IsMatch("#FFF", Regexp.ForHex).ShouldBeTrue();
        Regex.IsMatch("#F4DFFE", Regexp.ForHex).ShouldBeTrue();
        Regex.IsMatch("#F", Regexp.ForHex).ShouldBeFalse();
        Regex.IsMatch("#F4", Regexp.ForHex).ShouldBeFalse();
        Regex.IsMatch("#F4DF", Regexp.ForHex).ShouldBeFalse();
        Regex.IsMatch("#F4DFF", Regexp.ForHex).ShouldBeFalse();
    }

    [Fact]
    public void Helpers_Regexp_ForInteger()
    {
        Regex.IsMatch("1", Regexp.ForInteger).ShouldBeTrue();
        Regex.IsMatch("-1", Regexp.ForInteger).ShouldBeTrue();
        Regex.IsMatch("12", Regexp.ForInteger).ShouldBeTrue();
        Regex.IsMatch("1234567890", Regexp.ForInteger).ShouldBeTrue();
        Regex.IsMatch("0.2", Regexp.ForInteger).ShouldBeFalse();
        Regex.IsMatch("3,2", Regexp.ForInteger).ShouldBeFalse();
    }

    [Fact]
    public void Helpers_Regexp_ForTag()
    {
        Regex.IsMatch("<html></html>", Regexp.ForTag).ShouldBeTrue();
        Regex.IsMatch("<html/>", Regexp.ForTag).ShouldBeTrue();
        Regex.IsMatch("<html attribute=\"somValue\"></html>", Regexp.ForTag).ShouldBeTrue();
        Regex.IsMatch("<html>", Regexp.ForTag).ShouldBeFalse();
    }

    [Fact]
    public void Helpers_Regexp_ForTime()
    {
        Regex.IsMatch("12:45", Regexp.ForTime).ShouldBeTrue();
        Regex.IsMatch("07:45", Regexp.ForTime).ShouldBeTrue();
        Regex.IsMatch("23:45", Regexp.ForTime).ShouldBeTrue();
        Regex.IsMatch("00:45", Regexp.ForTime).ShouldBeTrue();
        Regex.IsMatch("80:45", Regexp.ForTime).ShouldBeFalse();
    }

    [Fact]
    public void Helpers_Regexp_ForUrl()
    {
        Regex.IsMatch("http://somedomain.com", Regexp.ForUrl).ShouldBeTrue();
        Regex.IsMatch("https://somedomain.com", Regexp.ForUrl).ShouldBeTrue();
        Regex.IsMatch("https://ww.somedomain.com", Regexp.ForUrl).ShouldBeTrue();
        Regex.IsMatch("ftp://somedomain.com", Regexp.ForUrl).ShouldBeTrue();
        Regex.IsMatch("ftp://ww.somedomain.com", Regexp.ForUrl).ShouldBeTrue();
        Regex.IsMatch("ftps://somedomain.com", Regexp.ForUrl).ShouldBeTrue();
        Regex.IsMatch("ftps://ww.somedomain.com", Regexp.ForUrl).ShouldBeTrue();
        Regex.IsMatch("file://somedomain.com", Regexp.ForUrl).ShouldBeTrue();
        Regex.IsMatch("file://ww.somedomain.com", Regexp.ForUrl).ShouldBeTrue();
        Regex.IsMatch("http://ww.somedomain.com", Regexp.ForUrl).ShouldBeTrue();
        Regex.IsMatch("http://somedomain", Regexp.ForUrl).ShouldBeFalse();
        Regex.IsMatch("https://somedomain", Regexp.ForUrl).ShouldBeFalse();
        Regex.IsMatch("ftp://somedomain", Regexp.ForUrl).ShouldBeFalse();
        Regex.IsMatch("ftps://somedomain", Regexp.ForUrl).ShouldBeFalse();
        Regex.IsMatch("file://somedomain", Regexp.ForUrl).ShouldBeFalse();
    }

    [Fact]
    public void Helpers_Regexp_ForName()
    {
        Regex.IsMatch("George", Regexp.ForName).ShouldBeTrue();
        Regex.IsMatch("George Washington", Regexp.ForName).ShouldBeTrue();
        Regex.IsMatch("George Washington III", Regexp.ForName).ShouldBeTrue();
        Regex.IsMatch("George Washington 3", Regexp.ForName).ShouldBeFalse();
        Regex.IsMatch("@george", Regexp.ForName).ShouldBeFalse();
    }

    [Fact]
    public void Helpers_Regexp_ForSubdomain()
    {
        Regex.IsMatch(".example", Regexp.ForSubdomain).ShouldBeTrue();
        Regex.IsMatch(".example.com", Regexp.ForSubdomain).ShouldBeTrue();
        Regex.IsMatch("example2.com", Regexp.ForSubdomain).ShouldBeTrue();
        Regex.IsMatch("2.example2.com", Regexp.ForSubdomain).ShouldBeTrue();
    }

    [Fact]
    public void Helpers_Regexp_ForDomain()
    {
        Regex.IsMatch("example.com", Regexp.ForDomain).ShouldBeTrue();
        Regex.IsMatch("2example.com", Regexp.ForDomain).ShouldBeTrue();
        Regex.IsMatch("2.example.com", Regexp.ForDomain).ShouldBeFalse();
    }

    [Fact]
    public void Helpers_Regexp_Hostname()
    {
        // yes this is my laptop, please help me to buy me a new one ;D
        Regex.IsMatch("HpNotebook200", Regexp.ForHostname).ShouldBeTrue();
        Regex.IsMatch("HpNotebook@200", Regexp.ForHostname).ShouldBeFalse();
    }
    
    #endregion

    #region Enums

    [Fact]
    public void Helpers_Enums_Count()
    {
        Enums.ToCount<FakeTypes.ValueTypes.EnumerationTypes.ExampleEnumeration>().ShouldBe(5);
    }

    [Fact]
    public void Helpers_Enum_Values()
    {
        var values = Enums.ToValues<FakeTypes.ValueTypes.EnumerationTypes.ExampleEnumeration>();
        values.Count().ShouldBe(5);
        values.First().ShouldBe("a");
    }

    [Fact]
    public void Helpers_Enum_List()
    {
        var values = Enums.ToList<FakeTypes.ValueTypes.EnumerationTypes.ExampleEnumeration>();
        values.Count().ShouldBe(5);
        values.First().Value.ShouldBe(0);
        values.First().Text.ShouldBe("a");
        values.First().Selected.ShouldBeFalse();
    }

    #endregion
}