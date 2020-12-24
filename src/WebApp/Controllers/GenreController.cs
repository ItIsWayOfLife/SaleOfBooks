using AutoMapper;
using Core.DTO;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Models.Genre;

namespace WebApp.Controllers
{
    [Authorize(Roles = "admin, employee")]
    public class GenreController : Controller
    {
        private IGenreService _genreService;

        public GenreController(IGenreService genreService)
        {
            _genreService = genreService;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Index()
        {
            IEnumerable<GenreDTO> genresDto = _genreService.GetGenresWithCountBooks();
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<GenreDTO, GenreViewModel>()).CreateMapper();
            var genres = mapper.Map<IEnumerable<GenreDTO>, List<GenreViewModel>>(genresDto);

            return View(new ListGenresViewModel() { Genres = genres });
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(AddGenreViewModel model)
        {
            _genreService.Add(new GenreDTO() { Name = model.Name});

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            _genreService.Delete(id);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            GenreDTO genreDTO = _genreService.GetGenre(id);

            return View(new EditGenreViewModel() { Id = genreDTO.Id, Name = genreDTO.Name });
        }

        [HttpPost]
        public IActionResult Edit(EditGenreViewModel model)
        {
            _genreService.Edit(new GenreDTO() { Id = model.Id, Name = model.Name });

            return RedirectToAction("Index");
        }
    }
}
