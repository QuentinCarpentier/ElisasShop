using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ElisasShop.Models;
using ElisasShop.ViewModels;

namespace ElisasShop.Controllers
{
    public class PieController : Controller
    {
        private readonly IPieRepository _pieRepository;
        private readonly ICategoryRepository _categoryRepository;

        // Dependency injection through constructor injection
        public PieController(IPieRepository pieRepository, ICategoryRepository categoryRepository)
        {
            _pieRepository = pieRepository;
            _categoryRepository = categoryRepository;
        }

        public ViewResult List()
        {
            PiesListViewModel piesListViewModel = new PiesListViewModel();

            // Pies property of our ViewModel (Pie Model) is the same as Pies property of our PieRepository
            piesListViewModel.Pies = _pieRepository.Pies;
            // Set the CurrentCategory property of our ViewModel
            piesListViewModel.CurrentCategory = "Cheese cakes";

            // Behind the scenes, it's ViewData.Model
            return View(piesListViewModel);
        }
    }
}