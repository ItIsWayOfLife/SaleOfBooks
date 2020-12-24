﻿using AutoMapper;
using Core.Constants;
using Core.DTO;
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
            _genreService.Add(new GenreDTO() { Name = model.Name});

            _loggerService.LogInformation(CONTROLLER_NAME + LoggerConstants.ACTION_ADD, LoggerConstants.TYPE_POST, "add successful", GetCurrentUserId());

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            _genreService.Delete(id);

            _loggerService.LogInformation(CONTROLLER_NAME + LoggerConstants.ACTION_DELETE, LoggerConstants.TYPE_POST, "delete successful", GetCurrentUserId());

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            GenreDTO genreDTO = _genreService.GetGenre(id);

            if (genreDTO == null)
            {
                return RedirectToAction("Error", "Home", new { requestId = "400" });
            }

            _loggerService.LogInformation(CONTROLLER_NAME + LoggerConstants.ACTION_EDIT, LoggerConstants.TYPE_GET, "edit", GetCurrentUserId());

            return View(new EditGenreViewModel() { Id = genreDTO.Id, Name = genreDTO.Name });
        }

        [HttpPost]
        public IActionResult Edit(EditGenreViewModel model)
        {
            _genreService.Edit(new GenreDTO() { Id = model.Id, Name = model.Name });

            _loggerService.LogInformation(CONTROLLER_NAME + LoggerConstants.ACTION_EDIT, LoggerConstants.TYPE_POST, "edit successful", GetCurrentUserId());

            return RedirectToAction("Index");
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