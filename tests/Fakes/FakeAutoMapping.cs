namespace Tests.Fakes;

public class FakeAutoMapping: AutoMapper.Profile
{
    public FakeAutoMapping()
    {
        CreateMap<FakeBusinessEntityRequestView, FakeBusinessEntity>();
        CreateMap<FakeBusinessEntity2RequestView, FakeBusinessEntity2>();
    }
}