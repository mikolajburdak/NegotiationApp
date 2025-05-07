using AutoMapper;
using PriceNegotiationApp.Models;

namespace PriceNegotiationApp.AutoMapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateProductDto, Product>();
        CreateMap<Product, ProductDto>(); 
        CreateMap<StartNegotiationDto, Negotiation>();
        CreateMap<CreatePriceProposalDto, PriceProposal>();
        CreateMap<Negotiation, NegotiationDto>();
    }
}