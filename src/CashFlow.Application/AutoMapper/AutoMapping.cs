using AutoMapper;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Entities;

namespace CashFlow.Application.AutoMapper
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            RequestToEntity();
            EntityToResponse();
        }

        private void RequestToEntity()
        {
            CreateMap<RequestRegisterUserJson, User>()
                .ForMember(dest => dest.Password, config => config.Ignore());

            CreateMap<RequestExpenseJson, Expense>()
                .ForMember(dest => dest.Tags, config => config.MapFrom(src => src.Tags.Distinct()));

            CreateMap<Communication.Enums.Tag, Tag>()
                .ForMember(dest => dest.Value, config => config.MapFrom(src => src));
        }

        private void EntityToResponse()
        {
            CreateMap<Expense, ResponseExpenseJson>()
                .ForMember(dest => dest.Tags, config => config.MapFrom(src => src.Tags.Select(tag => tag.Value)));
            CreateMap<Expense, ResponseRegisterExpenseJson>();
            CreateMap<Expense, ResponseShortExpenseJson>();
            CreateMap<User, ResponseUserProfileJson>();
        }
    }
}
