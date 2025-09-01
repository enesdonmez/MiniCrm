using Mapster;
using MiniCrmApi.DTOs.CustomerNoteDtos;
using MiniCrmApi.DTOs.CustomerDtos;
using MiniCrmApi.DTOs.DealDtos;
using MiniCrmApi.Entities;

namespace MiniCrmApi.Mapping;

public static class MappingCongfig
{
    public static void Configure()
    {
        TypeAdapterConfig<Customer, CustomerCreateDto>.NewConfig().TwoWays();
        TypeAdapterConfig<Customer, UpdateCustomerDto>.NewConfig().TwoWays();
        
        TypeAdapterConfig<Deal, CreateDealDto>.NewConfig().TwoWays();
        TypeAdapterConfig<Deal, UpdateDealDto>.NewConfig().TwoWays();
        
        TypeAdapterConfig<CreateCustomerNoteDto, CustomerNote>.NewConfig()
            .Ignore(dest => dest.Customer);
        TypeAdapterConfig<CustomerNote, UpdateCustomerNoteDto>.NewConfig().TwoWays();
    }
}