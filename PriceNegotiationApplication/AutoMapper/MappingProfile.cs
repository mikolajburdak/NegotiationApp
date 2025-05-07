using AutoMapper;
using PriceNegotiationApp.DTOs;
using PriceNegotiationApp.Models;

namespace PriceNegotiationApp.AutoMapper
{
    /// <summary>
    /// Defines the AutoMapper configuration for the application's model-to-DTO mappings.
    /// </summary>
    public class MappingProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MappingProfile"/> class.
        /// </summary>
        public MappingProfile()
        {
            // Mapping between CreateProductDto and Product.
            CreateMap<CreateProductDto, Product>();

            // Mapping between Product and ProductDto.
            CreateMap<Product, ProductDto>();

            // Mapping between StartNegotiationDto and Negotiation.
            CreateMap<StartNegotiationDto, Negotiation>();

            // Mapping between CreatePriceProposalDto and PriceProposal.
            CreateMap<CreatePriceProposalDto, PriceProposal>();

            // Mapping between Negotiation and NegotiationDto.
            CreateMap<Negotiation, NegotiationDto>();
        }
    }
}