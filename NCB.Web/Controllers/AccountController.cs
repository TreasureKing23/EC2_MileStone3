using AutoMapper;
using AutoMapper.Execution;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NCB.ModelDTO;
using NCB.Models;
using NCB.Repositories.Interfaces;
using NCB.Services.Interfaces;
using System.Collections.Generic;
using System.Data;
using System.Security.Claims;

namespace NCB.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAuthManager _authManager;

        public AccountController(IUnitOfWork unitOfWork, IMapper mapper, IAuthManager authManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _authManager = authManager;
        }


        [HttpGet]

        public async Task<IActionResult> Index()
        {
            var accounts = await _unitOfWork.GenericRepository<Account>().GetAll();
            var results = _mapper.Map<IList<AccountDTO>>(accounts);

            //var currencies = await _unitOfWork.GenericRepository<Currency>().GetAll();
            //var cur_results = _mapper.Map<IList<CurrencyDTO>>(currencies);
            foreach (var account in results)
            {
                var user = await _unitOfWork.GenericRepository<ApplicationUser>().Get(q => q.Id.ToLower() == account.UserId.ToString().ToLower());
                account.User = _mapper.Map<UserDTO>(user);

                var currency = await _unitOfWork.GenericRepository<Currency>().Get(q => q.Id == account.CurrencyId);
                account.Currency = _mapper.Map<Currency>(currency);

            }
            return View(results);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Currencies = new SelectList(await _unitOfWork.GenericRepository<Currency>().GetAll(), "Id", "ShortName");
            ViewBag.Users = new SelectList(await _unitOfWork.GenericRepository<ApplicationUser>().GetAll(), "Id", "Name");
            return View();
        }



        [HttpPost]

        public async Task<IActionResult> Create(AccountDTO model)
        {
            var account = _mapper.Map<Account>(model);
            await _unitOfWork.GenericRepository<Account>().Insert(account);
            await _unitOfWork.Save();
            return RedirectToAction("Index");
        }

       




        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var account = await _unitOfWork.GenericRepository<Account>().Get(q => q.Id == id);
            var viewModel = _mapper.Map<AccountDTO>(account);

            ViewBag.Users = new SelectList(await _unitOfWork.GenericRepository<ApplicationUser>().GetAll(), "Id", "Name");
            ViewBag.Currencies = new SelectList(await _unitOfWork.GenericRepository<Currency>().GetAll(), "Id", "ShortName");
            return View(viewModel);

        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, AccountDTO model)
        {
            var account = _mapper.Map<Account>(model);
            _unitOfWork.GenericRepository<Account>().Update(account);
            await _unitOfWork.Save();
            return RedirectToAction("Index");

        }



        [HttpGet]

        public async Task<IActionResult> Delete(int id)
        {
            var accounts = await _unitOfWork.GenericRepository<Account>().Get(q => q.Id == id);
            var viewModel = _mapper.Map<AccountDTO>(accounts);

            return View(accounts);
        }
        [HttpPost]

        public async Task<IActionResult> Delete(int id, AccountDTO model)
        {
            await _unitOfWork.GenericRepository<Account>().Delete(id);
            await _unitOfWork.Save();
            return RedirectToAction("Index");
        }



        public async Task<IActionResult> Deposit(TransactionDTO transactionDTO)
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
            return RedirectToAction("Index");

        }



        public async Task<IActionResult> Withdrawal(TransactionDTO transactionDTO)
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
                    TransactionDate = DateTime.UtcNow,
                    Source = transactionDTO.Source,
                    TransactionType = TransactionType.Withdrawal,
                    UserId = transactionDTO.UserId
                };
                await _unitOfWork.GenericRepository<Transaction>().Insert(transaction);
            }
            return RedirectToAction("Index");
        }




        [HttpGet]

        public async Task<IActionResult> ViewTransactions()
        {
            var transactions = await _unitOfWork.GenericRepository<Transaction>().GetAll();
            var transactionsDTOList = _mapper.Map<IList<TransactionDTO>>(transactions);

            foreach (TransactionDTO transaction in transactionsDTOList)
            {
                var account = _unitOfWork.GenericRepository<Account>().Get(a => a.AccountNumber == transaction.AccountNumber).Result;
                transaction.UserId = account.UserId.ToString();
                transaction.CardNumber = account.CardNumber;
                var user = _unitOfWork.GenericRepository<ApplicationUser>().Get(u => u.Id == transaction.UserId).Result;
                transaction.User = _mapper.Map<UserDTO>(user);

            }
            return View(transactionsDTOList);
        }
        

        [HttpGet]

        public async Task<IActionResult> CreateTransaction()
        {
            ViewBag.Users = new SelectList(await _unitOfWork.GenericRepository<ApplicationUser>().GetAll(), "Id", "Name");
            //ViewBag. Currencies = new SelectList (await _unitOfWork.GenericRepository<Currency>().GetAll(), "Id", "ShortName");
            return View();
        }
        [HttpPost]


        public async Task<IActionResult> CreateTransaction(TransactionDTO transactionDTO)
        {
            bool isFound = false;
            var tnum = 1;
            var account = _unitOfWork.GenericRepository<Account>().Get(a => a.CardNumber == transactionDTO.CardNumber &&
            a.CardSecurityCode == transactionDTO.CardSecurityCode && a.CardExpirationDate == transactionDTO.CardExpirationDate).Result;
            if (account == null)
            {
                return BadRequest("Card Details Invalid");
            }
            else
            {
                isFound = true;
                if (transactionDTO.TransactionType == TransactionType.Withdrawal)
                {
                    if (account.Balance < transactionDTO.Amount)
                    {
                        return BadRequest("Insufficent Funds");
                    }
                    account.Balance -= transactionDTO.Amount;
                    
                }
                else
                {
                    account.Balance += transactionDTO.Amount;
                    tnum = 0;
                    
                }
                _unitOfWork.GenericRepository<Account>().Update(account);
                await _unitOfWork.Save();
                Transaction transaction = new Transaction
                {
                    AccountNumber = account.AccountNumber,
                    Amount = transactionDTO.Amount,
                    TransactionDate = DateTime.Now,
                    Source = "NCB Web App",
                    TransactionType = transactionDTO.TransactionType,
                    UserId = transactionDTO.UserId
                };

                await _unitOfWork.GenericRepository<Transaction>().Insert(transaction);
                await _unitOfWork.Save();
            }
            return RedirectToAction("ViewTransactions");
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View("AccessDenied");
        }
    }
}

