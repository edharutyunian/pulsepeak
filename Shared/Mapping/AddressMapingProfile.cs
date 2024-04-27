using AutoMapper;

public class AddressMappingProfile : Profile
{
    public AddressMappingProfile()
    {
        CreateMap<AddressModel, AddressBaseEntity>();
        CreateMap<AddressBaseEntity, AddressModel>();
    }
}