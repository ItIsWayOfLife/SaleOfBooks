﻿using Core.Constants;
using Core.DTO;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApp.Interfaces;
using WebApp.Models.Book;
using Core.Exceptions;

namespace WebApp.Controllers
{
    [Authorize(Roles = "admin, employee")]
    public class BookController : Controller
    {
        private readonly IBookService _bookService;
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly IBookHelper _bookHelper;
        private readonly ILoggerService _loggerService;

        private const string CONTROLLER_NAME = "book";

        public BookController(IBookService bookService,
            IWebHostEnvironment appEnvironment,
            IBookHelper bookHelper,
            ILoggerService loggerService)
        {
            _bookService = bookService;
            _appEnvironment = appEnvironment;
            _bookHelper = bookHelper;
            _loggerService = loggerService;
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult ListFavoriteBook()
        {
            IEnumerable<BookDTO> booksDtos = _bookService.GetFavoriteBooks();
            var books = _bookHelper.GetBooksViewModel(booksDtos);

            _loggerService.LogInformation(CONTROLLER_NAME + "/listfavoritebook", LoggerConstants.TYPE_GET, "list favorite book", GetCurrentUserId());

            return View(new ListBookViewModel() { Books = books });
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Index(string sortString, string stringGenre, string searchFor, string nameSearch, bool isDisplay = false, int page = 1)
        {
            IEnumerable<BookDTO> booksDtos = new List<BookDTO>();

            #region Get books

            if (User.IsInRole("admin") || User.IsInRole("employee"))
            {
                booksDtos = _bookService.GetBooks();

                if (isDisplay)
                {
                    booksDtos = _bookService.GetBooks().ToList().Where(p => p.IsDisplay == true).ToList();
                }
            }
            else
            {
                booksDtos = _bookService.GetBooks().ToList().Where(p => p.IsDisplay == true).ToList();
            }

            var books = _bookHelper.GetBooksViewModel(booksDtos);

            #endregion

            #region Genres

            var listGenres = _bookHelper.GetGenres();

            if (stringGenre != null && stringGenre != string.Empty
                 && stringGenre != listGenres[0])
            {
                books = books.Where(p => p.Genre == stringGenre).ToList();
            }

            #endregion

            #region Sort

            List<string> listSort = new List<string>()
            {
                "Sort",
                "By name (A-Z)",
                "By name (Z-A)",
                "By price (min)",
                "By price (max)"
            };

            if (sortString != null)
            {
                bool isState = true;

                SortStateBook sortStateBook = SortStateBook.NameAsc;

                if (sortString == listSort[1])
                {
                    sortStateBook = SortStateBook.NameDesc;
                }
                else if (sortString == listSort[2])
                {
                    sortStateBook = SortStateBook.NameAsc;
                }
                else if (sortString == listSort[3])
                {
                    sortStateBook = SortStateBook.PriceAsc;
                }
                else if (sortString == listSort[4])
                {
                    sortStateBook = SortStateBook.PriceDesc;
                }
                else
                {
                    isState = false;
                }

                if (isState)
                {
                    books = sortStateBook switch
                    {
                        SortStateBook.NameAsc => books.OrderByDescending(s => s.Name).ToList(),
                        SortStateBook.NameDesc => books.OrderBy(s => s.Name).ToList(),
                        SortStateBook.PriceDesc => books.OrderByDescending(s => s.Price).ToList(),
                        _ => books.OrderBy(s => s.Price).ToList(),
                    };
                }
            }

            #endregion

            #region Search

            List<string> listSearch = new List<string>()
            {
                "Search",
                "Name",
                "Information",
                "Code",
                "Price",
                "Author",
                "Publishing house",
                "Year of writing",
                "Year publishing"
            };

            nameSearch = nameSearch ?? string.Empty;

            if (searchFor != string.Empty && searchFor != null && searchFor != "Search" && nameSearch != null)
            {
                if (searchFor.ToLower() == listSearch[1].ToLower() && nameSearch != string.Empty)
                {
                    books = books.Where(p => p.Name != null && p.Name.ToLower().Contains(nameSearch.ToLower())).ToList();
                }
                if (searchFor.ToLower() == listSearch[1].ToLower() && nameSearch == string.Empty)
                {
                    books = books.Where(p => p.Name == null || p.Name == string.Empty).ToList();
                }
                else if (searchFor.ToLower() == listSearch[2].ToLower() && nameSearch != string.Empty)
                {
                    books = books.Where(p => p.Info != null && p.Info.ToLower().Contains(nameSearch.ToLower())).ToList();
                }
                else if (searchFor.ToLower() == listSearch[2].ToLower() && nameSearch == string.Empty)
                {
                    books = books.Where(p => p.Info == null || p.Info == string.Empty).ToList();
                }
                else if (searchFor.ToLower() == listSearch[3].ToLower() && nameSearch != string.Empty)
                {
                    books = books.Where(p => p.Code != null && p.Code.ToLower().Contains(nameSearch.ToLower())).ToList();
                }
                else if (searchFor.ToLower() == listSearch[3].ToLower() && nameSearch == string.Empty)
                {
                    books = books.Where(p => p.Code == null || p.Code == string.Empty).ToList();
                }
                else if (searchFor.ToLower() == listSearch[4].ToLower() && nameSearch != string.Empty)
                {
                    books = books.Where(p => p.Price.ToString().Contains(nameSearch)).ToList();
                }
                else if (searchFor.ToLower() == listSearch[4].ToLower() && nameSearch == string.Empty)
                {
                    books = books.Where(p => p.Price == 0).ToList();
                }
                else if (searchFor.ToLower() == listSearch[5].ToLower() && nameSearch != string.Empty)
                {
                    books = books.Where(p => p.Author != null && p.Author.ToLower().Contains(nameSearch.ToLower())).ToList();
                }
                else if (searchFor.ToLower() == listSearch[5].ToLower() && nameSearch == string.Empty)
                {
                    books = books.Where(p => p.Author == null || p.Author == string.Empty).ToList();
                }
                else if (searchFor.ToLower() == listSearch[6].ToLower() && nameSearch != string.Empty)
                {
                    books = books.Where(p => p.PublishingHouse != null && p.PublishingHouse.ToLower().Contains(nameSearch.ToLower())).ToList();
                }
                else if (searchFor.ToLower() == listSearch[6].ToLower() && nameSearch == string.Empty)
                {
                    books = books.Where(p => p.PublishingHouse == null || p.PublishingHouse == string.Empty).ToList();
                }
                else if (searchFor.ToLower() == listSearch[7].ToLower() && nameSearch != string.Empty)
                {
                    books = books.Where(p => p.YearOfWriting != null && p.YearOfWriting == nameSearch).ToList();
                }
                else if (searchFor.ToLower() == listSearch[7].ToLower() && nameSearch == string.Empty)
                {
                    books = books.Where(p => p.YearOfWriting == null || p.YearOfWriting == string.Empty).ToList();
                }
                else if (searchFor.ToLower() == listSearch[8].ToLower() && nameSearch != string.Empty)
                {
                    books = books.Where(p => p.YearPublishing != null && p.YearPublishing == nameSearch).ToList();
                }
                else if (searchFor.ToLower() == listSearch[8].ToLower() && nameSearch == string.Empty)
                {
                    books = books.Where(p => p.YearPublishing == null || p.YearPublishing == string.Empty).ToList();
                }
            }

            #endregion


            #region Pagination

            int pageSize = 6;

            var count = books.Count;
            var items = books.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);

            #endregion

            _loggerService.LogInformation(CONTROLLER_NAME + LoggerConstants.ACTION_INDEX, LoggerConstants.TYPE_GET, "index", GetCurrentUserId());

            return View(new ListBookViewModel()
            {
                Books = items,
                ListSort = new SelectList(listSort),
                SortString = sortString,
                ListGenres = new SelectList(listGenres),
                StringGenre = stringGenre,
                ListSearch = new SelectList(listSearch),
                SearchFor = searchFor,
                NameSearch = nameSearch,
                PageViewModel = pageViewModel,
                 IsDisplay = isDisplay
            });
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetBookInfo(int id, string sortString, string stringGenre, string searchFor, string nameSearch, bool isDisplay)
        {
            ViewBag.SortString = sortString;
            ViewBag.StringGenre = stringGenre;
            ViewBag.SearchFor = searchFor;
            ViewBag.NameSearch = nameSearch;
            ViewBag.IsDisplay = isDisplay;

            var bookDto = _bookService.GetBook(id);

            _loggerService.LogInformation(CONTROLLER_NAME + $"/getbookinfo/{id}", LoggerConstants.TYPE_GET, $"get book id: {id} info", GetCurrentUserId());

            return View(_bookHelper.GetBookViewModel(bookDto));
        }

        [HttpPost]
        public IActionResult Delete(int id, string sortString, string stringGenre, string searchFor, string nameSearch, bool isDisplay)
        {
            try
            {
                _bookService.Delete(id);
            }
            catch (ValidationException ex)
            {
                _loggerService.LogWarning(CONTROLLER_NAME + LoggerConstants.ACTION_DELETE + $"/{id}", LoggerConstants.TYPE_POST, $"delete book id: {id} error: {ex.Message}", GetCurrentUserId());

                 return RedirectToAction("Error", "Home", new { requestId = "400", errorInfo = ex.Message });
            }

            _loggerService.LogInformation(CONTROLLER_NAME + LoggerConstants.ACTION_DELETE +$"/{id}", LoggerConstants.TYPE_POST, $"delete book id: {id} successful", GetCurrentUserId());

            return RedirectToAction("Index", new { sortString, stringGenre, searchFor, nameSearch, isDisplay });
        }

        [HttpGet]
        public IActionResult Add(string sortString, string stringGenre, string searchFor, string nameSearch, bool isDisplay)
        {
            ViewBag.SortString = sortString;
            ViewBag.StringGenre = stringGenre;
            ViewBag.SearchFor = searchFor;
            ViewBag.NameSearch = nameSearch;
            ViewBag.IsDisplay = isDisplay;

            _loggerService.LogInformation(CONTROLLER_NAME + LoggerConstants.ACTION_ADD, LoggerConstants.TYPE_GET, "add", GetCurrentUserId());

            return View(new AddBookGenreViewModel { Genres = new SelectList(_bookHelper.GetGenres()) });
        }

        [HttpPost]
        public async Task<IActionResult> Add(IFormFile uploadedFile, [FromForm] AddBookGenreViewModel model,
            string sortString, string stringGenre, string searchFor, string nameSearch, bool isDisplay)
        {
            ViewBag.SortString = sortString;
            ViewBag.StringGenre = stringGenre;
            ViewBag.SearchFor = searchFor;
            ViewBag.NameSearch = nameSearch;
            ViewBag.IsDisplay = isDisplay;

            model.Genres = new SelectList(_bookHelper.GetGenres());

            if (ModelState.IsValid)
            {
                BookDTO bookDto = null;
                string path = null;

                // save img
                if (uploadedFile != null)
                {
                    path = uploadedFile.FileName;
                    // save file wwwroot/files/books/
                    using (var fileStream = new FileStream(_appEnvironment.WebRootPath + PathConstants.PAPH_BOOKS + path, FileMode.Create))
                    {
                        await uploadedFile.CopyToAsync(fileStream);
                    }
                }
                else
                {
                    path = string.Empty;
                }

                int? idGenre = _bookHelper.GetGenreIdByName(model.AddBookViewModel.Genre);

                if (idGenre == null)
                {
                    _loggerService.LogWarning(CONTROLLER_NAME + LoggerConstants.ACTION_ADD, LoggerConstants.TYPE_POST, $"genre not found", GetCurrentUserId());

                    ModelState.AddModelError(string.Empty, "Choose a genre");

                    return View(model);
                }

                bookDto = new BookDTO
                {
                    Name = model.AddBookViewModel.Name,
                    Author = model.AddBookViewModel.Author,
                    IsFavorite = model.AddBookViewModel.IsFavorite,
                    Code = model.AddBookViewModel.Code,
                    Info = model.AddBookViewModel.Info,
                    Price = model.AddBookViewModel.Price,
                    PublishingHouse = model.AddBookViewModel.PublishingHouse,
                    YearOfWriting = model.AddBookViewModel.YearOfWriting,
                    YearPublishing = model.AddBookViewModel.YearPublishing,
                    IsDisplay = model.AddBookViewModel.IsDisplay,
                    Path = path,
                    GenreId = idGenre.Value,
                    IsNew = model.AddBookViewModel.IsNew
                };

                try
                {
                    _bookService.Add(bookDto);

                    _loggerService.LogInformation(CONTROLLER_NAME + LoggerConstants.ACTION_ADD, LoggerConstants.TYPE_POST, $"add book name: {model.AddBookViewModel.Name} successful", GetCurrentUserId());

                    return RedirectToAction("Index", new { sortString, stringGenre, searchFor, nameSearch, isDisplay });
                }
                catch(ValidationException ex)
                {
                    _loggerService.LogWarning(CONTROLLER_NAME + LoggerConstants.ACTION_ADD, LoggerConstants.TYPE_POST, $"add book name: {model.AddBookViewModel.Name} error: {ex}", GetCurrentUserId());

                    ModelState.AddModelError(ex.Property, ex.Message);
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Edit(int id, string sortString, string stringGenre, string searchFor, string nameSearch, bool isDisplay)
        {
            ViewBag.SortString = sortString;
            ViewBag.StringGenre = stringGenre;
            ViewBag.SearchFor = searchFor;
            ViewBag.NameSearch = nameSearch;
            ViewBag.IsDisplay = isDisplay;

            BookDTO bookDTO = _bookService.GetBook(id);
            BookViewModel bookViewModel = _bookHelper.GetBookViewModel(bookDTO);
            var genresList = _bookHelper.GetGenres();

            _loggerService.LogInformation(CONTROLLER_NAME + LoggerConstants.ACTION_EDIT +$"/{id}", LoggerConstants.TYPE_GET, $"edit book id {id}", GetCurrentUserId());

            return View(new EditBookGenreViewModel() { BookViewModel = bookViewModel, Genres = new SelectList(genresList) });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(IFormFile uploadedFile, [FromForm] EditBookGenreViewModel model,
            string sortString, string stringGenre, string searchFor, string nameSearch, bool isDisplay)
        {
            ViewBag.SortString = sortString;
            ViewBag.StringGenre = stringGenre;
            ViewBag.SearchFor = searchFor;
            ViewBag.NameSearch = nameSearch;
            ViewBag.IsDisplay = isDisplay;

            model.Genres = new SelectList(_bookHelper.GetGenres());

            if (ModelState.IsValid)
            {
                BookDTO bookDto = null;
                string path = null;

                // save image
                if (uploadedFile != null)
                {
                    path = uploadedFile.FileName;
                    //save  wwwroot/files/provider/
                    using (var fileStream = new FileStream(_appEnvironment.WebRootPath + PathConstants.PAPH_BOOKS + path, FileMode.Create))
                    {
                        await uploadedFile.CopyToAsync(fileStream);
                    }
                }
                else
                {
                    path = model.BookViewModel.Path;
                }

                int? idGenre = _bookHelper.GetGenreIdByName(model.BookViewModel.Genre);

                if (idGenre == null)
                {
                    _loggerService.LogWarning(CONTROLLER_NAME + LoggerConstants.ACTION_ADD, LoggerConstants.TYPE_POST, $"genre not found", GetCurrentUserId());

                    ModelState.AddModelError(string.Empty, "Choose a genre");

                    return View(model);
                }

                bookDto = new BookDTO()
                {
                    Id = model.BookViewModel.Id,
                    Name = model.BookViewModel.Name,
                    Author = model.BookViewModel.Author,
                    IsFavorite = model.BookViewModel.IsFavorite,
                    YearOfWriting = model.BookViewModel.YearOfWriting,
                    YearPublishing = model.BookViewModel.YearPublishing,
                    Code = model.BookViewModel.Code,
                    Info = model.BookViewModel.Info,
                    Price = model.BookViewModel.Price,
                    PublishingHouse = model.BookViewModel.PublishingHouse,
                    IsDisplay = model.BookViewModel.IsDisplay,
                    IsNew = model.BookViewModel.IsNew,
                    Path = path,
                    GenreId = idGenre.Value
                };

                try
                {
                    _bookService.Edit(bookDto);

                    _loggerService.LogInformation(CONTROLLER_NAME + LoggerConstants.ACTION_EDIT, LoggerConstants.TYPE_POST, $"edit book id: {model.BookViewModel.Id} successful", GetCurrentUserId());

                    return RedirectToAction("Index", new { sortString, stringGenre, searchFor, nameSearch, isDisplay });
                }
                catch (ValidationException ex)
                {
                    _loggerService.LogWarning(CONTROLLER_NAME + LoggerConstants.ACTION_EDIT, LoggerConstants.TYPE_POST, $"edit book id: {model.BookViewModel.Id} error: {ex.Message}", GetCurrentUserId());

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
