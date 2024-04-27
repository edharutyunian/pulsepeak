using AutoMapper;
using PulsePeak.Core.Entities.Addresses;
using PulsePeak.Core.Entities.Users;
using PulsePeak.Core.ViewModels;

namespace PulsePeak.BLL.MappingProfiles
{
    // TODO [ED]: this is just a draft version: need to be implemented
    public class MappingCoreProfile : Profile
    {
        public MappingCoreProfile()
        {
            CreateMap<AddressModel, AddressBaseEntity>().ReverseMap();
            CreateMap<UserBaseEnttity, IUserAccount>()
                .ConvertUsing<UserConverter>();
        }
    }

    public class UserConverter : ITypeConverter<UserBaseEnttity, IUserAccount>
    {
        public IUserAccount Convert(UserBaseEnttity source, IUserAccount destination, ResolutionContext context)
        {
            // TODO [ED]: implement
            return source;
        }
    }
}
