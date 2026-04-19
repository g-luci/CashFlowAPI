using AutoMapper;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Repositories.Expenses;

namespace CashFlow.Application.UseCases.Expenses.GetAll
{
    public class GetAllExpenseUseCase : IGetAllExpenseUseCase
    {
        private readonly IMapper _mapper;
        private readonly IExpensesReadOnlyRepository _repository;
        public GetAllExpenseUseCase(IMapper mapper, IExpensesReadOnlyRepository repository)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<ResponseExpensesJson> Execute()
        {
            var result = await _repository.GetAll();

            return new ResponseExpensesJson
            {
                Expenses = _mapper.Map<List<ResponseShortExpenseJson>>(result)
            };
        }
    }
}
