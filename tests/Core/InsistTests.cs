using Oluso;
using Shouldly;
using Tests.Fakes;
using Xunit.Sdk;

namespace Tests.Core;

public class InsistTests
{
    [Fact]
    public void Insist_MustBe_NotNull_Ok() => 
        Insist.MustBe.NotNull(FakeTypes.TestValues.FakeUserName1);

    [Fact]
    public void Insist_MustBe_NotNull_Fail()
    {
        try
        {
            Insist.MustBe.NotNull(FakeTypes.TestValues.String_Null);
        }
        catch (Exception e)
        {
            e.Message.ShouldNotBeNullOrWhiteSpace();
        }
    }

    [Fact]
    public void Insist_MustBe_NotNullOrWhiteSpace_Ok() =>
        Insist.MustBe.NotNullOrWhiteSpace(FakeTypes.TestValues.FakeUserName1);

    [Fact]
    public void Insist_MustBe_NotNullOrWhiteSpace_Fail()
    {
        try
        {
            Insist.MustBe.NotNullOrWhiteSpace(FakeTypes.TestValues.String_WhiteSpace);
        }
        catch (Exception e)
        {
            e.Message.ShouldNotBeNullOrWhiteSpace();
        }
    }

    [Fact]
    public void Insist_MustBe_True_Ok_Lambda() => 
        Insist.MustBe.True<FakeException>(true, () => "This is true");

    [Fact]
    public void Insist_MustBe_True_Ok_Func() =>
        Insist.MustBe.True<FakeException>(true, () =>
        {
            // execute some code
            return "This is true";
        });

    [Fact]
    public void Insists_MustBe_Ok_Msg() =>
        Insist.MustBe.True<FakeException>(true, "This is true");

    [Fact]
    public void Insist_MustBe_Fail_Lambda()
    {
        try
        {

        }
        catch (Exception e)
        {
            e.Message.ShouldNotBeNullOrWhiteSpace();
        }
    }

    [Fact]
    public void Insist_MustBe_False_Ok_Lambda() =>
        Insist.MustBe.False<FakeException>(false, () => "This is false");

    [Fact]
    public void Insist_Mustbe_False_Ok_Func() =>
        Insist.MustBe.False<FakeException>(false, () =>
        {
            // execute some code
            return "This is false";
        });

    [Fact]
    public void Insist_MustBe_False_Ok_Msg() =>
        Insist.MustBe.False<FalseException>(false, "This is false");
}