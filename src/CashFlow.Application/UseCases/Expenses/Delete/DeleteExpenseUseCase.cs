using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionBase;

namespace CashFlow.Application.UseCases.Expenses.Delete
{
    public class DeleteExpenseUseCase : IDeleteExpenseUseCase
    {
        private readonly IExpensesReadOnlyRepository _expenseReadOnly;
        private readonly IExpensesWriteOnlyRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggedUser _loggedUser;

        public DeleteExpenseUseCase(IExpensesWriteOnlyRepository repository,
            IExpensesReadOnlyRepository expenseReadOnly,
            IUnitOfWork unitOfWork,
            ILoggedUser loggedUser)
        {
            _repository = repository;
            _expenseReadOnly = expenseReadOnly;
            _unitOfWork = unitOfWork;
            _loggedUser = loggedUser;
        }

        public async Task Execute(long id)
        {
            var loggedUser = await _loggedUser.Get();

            var expense = await _expenseReadOnly.GetById(loggedUser, id);

            if(expense is null)
            {
                throw new NotFoundException(ResourceErrorMessages.EXPENSE_NOT_FOUND);
            }

            await _repository.Delete(id);

            await _unitOfWork.Commit();
        }
    }
}
