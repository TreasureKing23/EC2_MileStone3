using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NCB.ModelDTO;
using NCB.Models;
using NCB.Repositories.Interfaces;

namespace NCB.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AccountController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpPut("Deposit")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<bool>> Deposit([FromForm]TransactionDTO transactionDTO)
        {
            bool isFound = false;
            var account = _unitOfWork.GenericRepository<Account>().Get(a => a.CardNumber == transactionDTO.CardNumber &&
                a.CardSecurityCode == transactionDTO.CardSecurityCode && a.CardExpirationDate == transactionDTO.CardExpirationDate).Result;
            if (account == null)
            {
                return BadRequest("Card Details Invalid");
            }
            else
            {
                isFound = true;
                account.Balance += transactionDTO.Amount;

                _unitOfWork.GenericRepository<Account>().Update(account);
                await _unitOfWork.Save();
                Transaction transaction = new Transaction
                {
                    AccountNumber = account.AccountNumber,
                    Amount = transactionDTO.Amount,
                    TransactionDate = DateTime.UtcNow,
                    Source = transactionDTO.Source,
                    TransactionType = TransactionType.Deposit,
                    UserId = transactionDTO.UserId
                };
                await _unitOfWork.GenericRepository<Transaction>().Insert(transaction);
                await _unitOfWork.Save();
            }
            return Ok(isFound);

        }


        [HttpPut("Withdrawal")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<bool>> Withdrawal([FromForm] TransactionDTO transactionDTO)
        {
            bool isFound = false;
            var account = _unitOfWork.GenericRepository<Account>().Get(a => a.CardNumber == transactionDTO.CardNumber &&
            a.CardSecurityCode == transactionDTO.CardSecurityCode && a.CardExpirationDate == transactionDTO.CardExpirationDate).Result;
            if (account == null)
            {
                return NotFound("Card Details Invalid");
            }
            else
            {


                isFound = true;
                if (account.Balance < transactionDTO.Amount)
                {
                    return BadRequest("Insufficient Funds");
                }
                account.Balance -= transactionDTO.Amount;
                _unitOfWork.GenericRepository<Account>().Update(account);
                await _unitOfWork.Save();

                Transaction transaction = new Transaction
                {
                    AccountNumber = account.AccountNumber,
                    Amount = transactionDTO.Amount,
                    TransactionDate = DateTime.Now,
                    Source = transactionDTO.Source,
                    TransactionType = TransactionType.Withdrawal,
                    UserId = transactionDTO.UserId
                };
                await _unitOfWork.GenericRepository<Transaction>().Insert(transaction);
                await _unitOfWork.Save();
            }
            return Ok(isFound); ;
        }

    }
}
