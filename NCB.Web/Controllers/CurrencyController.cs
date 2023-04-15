using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NCB.ModelDTO;
using NCB.Models;
using NCB.Repositories.Interfaces;

namespace NCB.Web.Controllers
{
    public class CurrencyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CurrencyController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var currencies = await _unitOfWork.GenericRepository<Currency>().GetAll();
            var results = _mapper.Map<IList<CurrencyDTO>>(currencies);
            return View(results);
        }

        [HttpGet]

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CurrencyDTO model)
        {
            var currency = _mapper.Map<Currency>(model);
            await _unitOfWork.GenericRepository<Currency>().Insert(currency); ;
            await _unitOfWork.Save();

            return RedirectToAction("Index");
        }




        [HttpGet]

        public async Task<IActionResult> Edit(int id)
        {
            var currencies = await _unitOfWork.GenericRepository<Currency>().Get(q => q.Id == id);
            var viewModel = _mapper.Map<CurrencyDTO>(currencies);

            return View(viewModel);

        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, CurrencyDTO model)
        {
            var currency = _mapper.Map<Currency>(model);
            _unitOfWork.GenericRepository<Currency>().Update(currency);
            await _unitOfWork.Save();
            return RedirectToAction("Index");
        }



        [HttpGet]

        public async Task<IActionResult> Delete(int id)
        {
            var currencies = await _unitOfWork.GenericRepository<Currency>().Get(q => q.Id == id);
            var viewModel = _mapper.Map<CurrencyDTO>(currencies);
            return View(currencies);
        }

        [HttpPost]

        public async Task<IActionResult> Delete(int id, CurrencyDTO model)
        {
            await _unitOfWork.GenericRepository<Currency>().Delete(id);
            await _unitOfWork.Save();
            return RedirectToAction("Index");
        }
    }
}
