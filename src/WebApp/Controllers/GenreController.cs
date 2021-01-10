using AutoMapper;
using Core.Constants;
using Core.DTO;
using Core.Exceptions;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using WebApp.Interfaces;
using WebApp.Models.Genre;

namespace WebApp.Controllers
{
    [Authorize(Roles = "admin, employee")]
    public class GenreController : Controller
    {
        private IGenreService _genreService;
        private readonly ILoggerService _loggerService;

        private const string CONTROLLER_NAME = "genre";

        public GenreController(IGenreService genreService,
            ILoggerService loggerService)
        {
            _genreService = genreService;
            _loggerService = loggerService;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Index()
        {
            IEnumerable<GenreDTO> genresDto = _genreService.GetGenresWithCountBooks();
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<GenreDTO, GenreViewModel>()).CreateMapper();
            var genres = mapper.Map<IEnumerable<GenreDTO>, List<GenreViewModel>>(genresDto);

            _loggerService.LogInformation(CONTROLLER_NAME + LoggerConstants.ACTION_INDEX, LoggerConstants.TYPE_GET, "index", GetCurrentUserId());

            return View(new ListGenresViewModel() { Genres = genres });
        }

        [HttpGet]
        public IActionResult Add()
        {
            _loggerService.LogInformation(CONTROLLER_NAME + LoggerConstants.ACTION_ADD, LoggerConstants.TYPE_GET, "add", GetCurrentUserId());

            return View();
        }

        [HttpPost]
        public IActionResult Add(AddGenreViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _genreService.Add(new GenreDTO() { Name = model.Name });

                    _loggerService.LogInformation(CONTROLLER_NAME + LoggerConstants.ACTION_ADD, LoggerConstants.TYPE_POST, $"add genre name: {model.Name} successful", GetCurrentUserId());

                    return RedirectToAction("Index");
                }
                catch (ValidationException ex)
                {
                    _loggerService.LogWarning(CONTROLLER_NAME + LoggerConstants.ACTION_ADD, LoggerConstants.TYPE_POST, $"add genre name: {model.Name} error: {ex.Message}", GetCurrentUserId());

                    ModelState.AddModelError(ex.Property, ex.Message);
                }
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            try
            {
                _genreService.Delete(id);
            }
            catch (ValidationException ex)
            {
                _loggerService.LogInformation(CONTROLLER_NAME + LoggerConstants.ACTION_DELETE + $"/{id}", LoggerConstants.TYPE_POST, $"delete genre id: {id} error: {ex.Message}", GetCurrentUserId());

                return RedirectToAction("Error", "Home", new { requestId = "400", errorInfo = ex.Message });
            }

            _loggerService.LogInformation(CONTROLLER_NAME + LoggerConstants.ACTION_DELETE +$"/{id}", LoggerConstants.TYPE_POST, $"delete genre id: {id} successful", GetCurrentUserId());

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            GenreDTO genreDTO = _genreService.GetGenre(id);

            if (genreDTO == null)
            {
                return RedirectToAction("Error", "Home", new { requestId = "400", errorInfo = "Genre not found" });
            }

            _loggerService.LogInformation(CONTROLLER_NAME + LoggerConstants.ACTION_EDIT +$"/{id}", LoggerConstants.TYPE_GET, $"edit genre id {id}", GetCurrentUserId());

            return View(new EditGenreViewModel() { Id = genreDTO.Id, Name = genreDTO.Name });
        }

        [HttpPost]
        public IActionResult Edit(EditGenreViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _genreService.Edit(new GenreDTO() { Id = model.Id, Name = model.Name });

                    _loggerService.LogInformation(CONTROLLER_NAME + LoggerConstants.ACTION_EDIT, LoggerConstants.TYPE_POST, $"edit genre id: {model.Id} successful", GetCurrentUserId());

                    return RedirectToAction("Index");
                }
                catch (ValidationException ex)
                {
                    _loggerService.LogWarning(CONTROLLER_NAME + LoggerConstants.ACTION_EDIT, LoggerConstants.TYPE_POST, $"edit genre id: {model.Id} error: {ex.Message}", GetCurrentUserId());

                    ModelState.AddModelError(ex.Property, ex.Message);
                }              
            }

            return View(model);
        }

        private string GetCurrentUserId()
        {
            if (User.Identity.IsAuthenticated)
            {
                return User.FindFirst(ClaimTypes.NameIdentifier).Value;
            }
            else
            {
                return null;
            }
        }
    }
}
