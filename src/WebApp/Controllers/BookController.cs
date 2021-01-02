using Core.Constants;
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
        public IActionResult Index(string sortString, string stringGenre, string searchFor, string nameSearch, bool isActive = false, int page = 1)
        {
            IEnumerable<BookDTO> booksDtos = new List<BookDTO>();

            #region Get books

            if (User.IsInRole("admin") || User.IsInRole("employee"))
            {
                booksDtos = _bookService.GetBooks();

                if (isActive)
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

            if (stringGenre != null && stringGenre != ""
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
                "Publisher",
                "Year of writing",
                "Year publishing"
            };

            if (searchFor != "" && searchFor != null && searchFor != "Search"
                && nameSearch != "" && nameSearch != null)
            {
                if (searchFor.ToLower() == listSearch[1].ToLower())
                {
                    books = books.Where(p => p.Name.ToLower().Contains(nameSearch.ToLower())).ToList();
                }
                else if (searchFor.ToLower() == listSearch[2].ToLower())
                {
                    books = books.Where(p => p.Info.ToLower().Contains(nameSearch.ToLower())).ToList();
                }
                else if (searchFor.ToLower() == listSearch[3].ToLower())
                {
                    books = books.Where(p => p.Code.ToLower().Contains(nameSearch.ToLower())).ToList();
                }
                else if (searchFor.ToLower() == listSearch[4].ToLower())
                {
                    books = books.Where(p => p.Price.ToString() == nameSearch).ToList();
                }
                else if (searchFor.ToLower() == listSearch[5].ToLower())
                {
                    books = books.Where(p => p.Author.ToLower().Contains(nameSearch.ToLower())).ToList();
                }
                else if (searchFor.ToLower() == listSearch[6].ToLower())
                {
                    books = books.Where(p => p.PublishingHouse.ToLower().Contains(nameSearch.ToLower())).ToList();
                }
                else if (searchFor.ToLower() == listSearch[7].ToLower())
                {
                    books = books.Where(p => p.YearOfWriting == nameSearch).ToList();
                }
                else if (searchFor.ToLower() == listSearch[8].ToLower())
                {
                    books = books.Where(p => p.YearPublishing == nameSearch).ToList();
                }
            }

            #endregion


            #region Pagination

            int pageSize = 12;

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
                PageViewModel = pageViewModel
            });
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetBookInfo(int id)
        {
            var bookDto = _bookService.GetBook(id);

            _loggerService.LogInformation(CONTROLLER_NAME + $"/getbookinfo/{id}", LoggerConstants.TYPE_GET, "get book info", GetCurrentUserId());

            return View(_bookHelper.GetBookViewModel(bookDto));
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            _bookService.Delete(id);

            _loggerService.LogInformation(CONTROLLER_NAME + LoggerConstants.ACTION_DELETE +$"{id}", LoggerConstants.TYPE_POST, "delete successful", GetCurrentUserId());

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Add()
        {
            _loggerService.LogInformation(CONTROLLER_NAME + LoggerConstants.ACTION_ADD, LoggerConstants.TYPE_GET, "add", GetCurrentUserId());

            return View(new AddBookGenreViewModel { Genres = new SelectList(_bookHelper.GetGenres()) });
        }

        [HttpPost]
        public async Task<IActionResult> Add(IFormFile uploadedFile, [FromForm] AddBookGenreViewModel model)
        {
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
                    path = "";
                }

                int? idGenre = _bookHelper.GetGenreIdByName(model.AddBookViewModel.Genre);

                if (idGenre == null)
                {
                    _loggerService.LogWarning(CONTROLLER_NAME + LoggerConstants.ACTION_ADD, LoggerConstants.TYPE_POST, $"genre not found", GetCurrentUserId());

                    return RedirectToAction("Error", "Home", new { requestId = "400" });
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

                _bookService.Add(bookDto);

                _loggerService.LogInformation(CONTROLLER_NAME + LoggerConstants.ACTION_ADD, LoggerConstants.TYPE_POST, $"add {model.AddBookViewModel.Name} book successful", GetCurrentUserId());

                return RedirectToAction("Index");
            }

            model.Genres = new SelectList(_bookHelper.GetGenres());

            return View(model);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            BookDTO bookDTO = _bookService.GetBook(id);
            BookViewModel bookViewModel = _bookHelper.GetBookViewModel(bookDTO);
            var genresList = _bookHelper.GetGenres();

            _loggerService.LogInformation(CONTROLLER_NAME + LoggerConstants.ACTION_EDIT +$"/{id}", LoggerConstants.TYPE_GET, "edit", GetCurrentUserId());

            return View(new EditBookGenreViewModel() { BookViewModel = bookViewModel, Genres = new SelectList(genresList) });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(IFormFile uploadedFile, [FromForm] EditBookGenreViewModel model)
        {
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
                    return RedirectToAction("Error", "Home", new { requestId = "400" });
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

                _bookService.Edit(bookDto);

                return RedirectToAction("Index");
            }

            model.Genres = new SelectList(_bookHelper.GetGenres());

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
