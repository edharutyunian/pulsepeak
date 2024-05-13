using AutoMapper;
using PulsePeak.Core.ViewModels;
using PulsePeak.Core.Entities.Users;
using PulsePeak.Core.Entities.Orders;
using PulsePeak.Core.Entities.Products;
using PulsePeak.Core.Entities.Payments;
using PulsePeak.Core.Entities.Addresses;
using PulsePeak.Core.Entities.ShoppingCart;
using PulsePeak.Core.ViewModels.AddressModels;
using PulsePeak.Core.ViewModels.UserViewModels.CustomerViewModels;
using PulsePeak.Core.ViewModels.UserViewModels.MerchantViewModels;

namespace PulsePeak.BLL.MappingProfiles
{
    // TODO [ED]: this is just a draft version: need to be fully implemented
    public class MappingCoreProfile : Profile
    {
        public MappingCoreProfile()
        {
            CreateMap<CustomerModel, CustomerEntity>().ReverseMap();
            CreateMap<MerchantModel, MerchantEntity>().ReverseMap();
            CreateMap<AddressModel, AddressBaseEntity>().ReverseMap();
            CreateMap<BillingAddressInfoModel, AddressModel>().ReverseMap();
            CreateMap<ShippingAddressInfoModel, AddressModel>().ReverseMap();
            CreateMap<BillingAddressInfoModel, AddressBaseEntity>().ReverseMap();
            CreateMap<ShippingAddressInfoModel, AddressBaseEntity>().ReverseMap();
            CreateMap<ProductModel, ProductBaseEntity>().ReverseMap();
            CreateMap<PaymentMethodModel, PaymentMehodBaseEntity>().ReverseMap();
            CreateMap<ShoppingCartModel, ShoppingCartBaseEntity>().ReverseMap();
            CreateMap<OrderModel, OrderBaseEntity>().ReverseMap();
        }
    }
}
