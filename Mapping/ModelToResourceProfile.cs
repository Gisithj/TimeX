using AutoMapper;
using Microsoft.AspNetCore.Identity;
using TimeX.DTO.Admin;
using TimeX.DTO.Business;
using TimeX.DTO.Customer;
using TimeX.DTO.Facility;
using TimeX.DTO.User;
using TimeX.Models;

namespace TimeX.Mapping
{
    public class ModelToResourceProfile:Profile
    {
        public ModelToResourceProfile()
        {
            //admin DTOs
            CreateMap<Admin, CreateAdminDTO>().ReverseMap();
            CreateMap<Admin, GetAdminDTO>().ReverseMap();
            

            //BusinessDTOs
            CreateMap<Business, CreateBusinessDTO>().ReverseMap();
            CreateMap<Business, GetBusinessDTO>().ReverseMap();

            //Customer DTOs
            CreateMap<Customer, CreateCustomerDto>().ReverseMap();
            CreateMap<Customer, GetCustomerDto>().ReverseMap();

            //User DTOs
            CreateMap<IdentityUser, CreateAdminDTO>().ReverseMap();
            CreateMap<IdentityUser, CreateBusinessDTO>().ReverseMap();
            CreateMap<IdentityUser, CreateCustomerDto>().ReverseMap();


            //FacilityDTOs
            CreateMap<Facility, CreateFacilityDto>().ReverseMap();
            CreateMap<Facility, GetFacilityDto>().ReverseMap();

        }
    }
}
