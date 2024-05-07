using AutoMapper;
using PulsePeak.Core.ViewModels;
using PulsePeak.Core.Entities.Users;
using PulsePeak.Core.Entities.Orders;
using PulsePeak.Core.Entities.Products;
using PulsePeak.Core.Entities.Payments;
using PulsePeak.Core.Entities.Addresses;
using PulsePeak.Core.Entities.ShoppingCart;
using PulsePeak.Core.ViewModels.UserViewModels;
using PulsePeak.Core.ViewModels.UserViewModels.CustomerViewModels;
using PulsePeak.Core.ViewModels.UserViewModels.MerchantViewModels;

namespace PulsePeak.BLL.MappingProfiles
{
    // TODO [ED]: this is just a draft version: need to be fully implemented
    public class MappingCoreProfile : Profile
    {
        public MappingCoreProfile()
        {
            CreateMap<UserModel, UserBaseEnttity>().ReverseMap();
            CreateMap<CustomerModel, CustomerEntity>().ReverseMap();
            CreateMap<MerchantModel, MerchantEntity>().ReverseMap();
            CreateMap<AddressModel, AddressBaseEntity>().ReverseMap();
            CreateMap<ProductModel, ProductBaseEntity>().ReverseMap();
            CreateMap<PaymentMethodModel, PaymentMehodBaseEntity>().ReverseMap();
            CreateMap<ShoppingCartModel, ShoppingCartBaseEntity>().ReverseMap();
            CreateMap<OrderModel, OrderBaseEntity>().ReverseMap();

            // not sure on this; probably not necessary
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
