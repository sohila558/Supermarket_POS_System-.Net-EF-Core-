using AutoMapper;
using MySuperMarket.DTOs;
using MySuperMarket.Models;

namespace MySuperMarket.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Customer, CustomerDTO>();
            CreateMap<Invoice, InvoiceDTO>();
            CreateMap<InvoiceItem, InvoiceItemDTO>();
            CreateMap<Product, ProductDTO>();
            CreateMap<Invoice, AddInvoiceDTO>();
            CreateMap<AddInvoiceDTO, Invoice>();
            CreateMap<InvoiceItemDTO, InvoiceItem>();
            CreateMap<AppUser, CreateUserDTO>();
            CreateMap<CreateUserDTO, AppUser>();
            CreateMap<AppUser, DeleteUserDTO>();
            CreateMap<DeleteUserDTO, AppUser>();
        }
    }
}
