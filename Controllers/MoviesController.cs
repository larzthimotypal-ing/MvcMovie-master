using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcMovie.Data;
using MvcMovie.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MvcMovie.Controllers
{
    public class MoviesController : Controller
    {
        private readonly MvcMovieContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public MoviesController(MvcMovieContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        // GET: Movies
        public async Task<IActionResult> Index(string movieGenre, string searchString)
        {
            IQueryable<string> genreQuery = from m in _context.Movies
                                            orderby m.Genre
                                            select m.Genre;

            var movies = from m in _context.Movies
                         select m;

            if (!string.IsNullOrEmpty(searchString))
            {
                movies = movies.Where(s => s.Title.ToLower().Contains(searchString.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(movieGenre))
            {
                movies = movies.Where(x => x.Genre.ToLower() == movieGenre.ToLower());
            }

            var movieGenreVM = new MovieGenreViewModel
            {
                Genres = new SelectList(await genreQuery.Distinct().ToListAsync()),
                Movies = await movies.ToListAsync()
            };

            return View(movieGenreVM);
        }

        [HttpPost]
        public string Index(string searchString, bool notUsed)
        {
            return $"From [HttpPost] Index: filter on {searchString}";
        }

        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .FirstOrDefaultAsync(m => m.ID == id);

            if (movie == null)
            {
                return NotFound();
            }

            var director = await _context.Directors.Where(i => i.ID == movie.DirectorID).ToListAsync();
            var directorName = "";
            foreach( var i in director)
            {
                directorName = i.Name;
            }
            ViewBag.directorName = directorName;
            return View(movie);

        }

        #region Create
        // GET: Movies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Title,ReleaseDate,Genre,Price,Rating,PosterFile")] Movie movie)
        {
            if (ModelState.IsValid)
            {
                if(movie.PosterFile == null)
                {
                    movie.PosterName = "default_poster.jpg";
                }
                else
                {
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = Path.GetFileNameWithoutExtension(movie.PosterFile.FileName);
                    string extension = Path.GetExtension(movie.PosterFile.FileName);
                    movie.PosterName = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    string path = Path.Combine(wwwRootPath + "/poster/", fileName);
                    using var fileStream = new FileStream(path, FileMode.Create);
                    await movie.PosterFile.CopyToAsync(fileStream);
                }

                _context.Add(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }


        #endregion

        #region Edit
        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
          
            ViewData["Directors"] = CreateDirectorDropdown(); 


            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }

    

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Title,ReleaseDate,Genre,Price,Rating,DirectorID,PosterFile")] Movie movie)
        {

           
            if (id != movie.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var newDirectorName = Request.Form["textbox"];
                    //add director
                    if (!string.IsNullOrWhiteSpace(newDirectorName))
                    {
                        Director director = new Director(newDirectorName);
                        _context.Update(director);
                        await _context.SaveChangesAsync();
                        movie.DirectorID = director.ID;

                    }
                    //change poster
                    var currentPoster = _context.Movies.AsNoTracking().FirstOrDefault(m => m.ID == id).PosterName;
                    if (movie.PosterFile != null)
                    {
                        if (currentPoster != "default_poster.jpg")
                        {
                            if(currentPoster != null){
                                var posterPath = Path.Combine(_hostEnvironment.WebRootPath, "poster", currentPoster);
                                if (System.IO.File.Exists(posterPath))
                                {
                                    System.IO.File.Delete(posterPath);
                                }
                            }
                            

                            string wwwRootPath = _hostEnvironment.WebRootPath;
                            string fileName = Path.GetFileNameWithoutExtension(movie.PosterFile.FileName);
                            string extension = Path.GetExtension(movie.PosterFile.FileName);
                            movie.PosterName = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                            string path = Path.Combine(wwwRootPath + "/poster/", fileName);
                            using var fileStream = new FileStream(path, FileMode.Create);
                            await movie.PosterFile.CopyToAsync(fileStream);
                        }
                    }
                    else
                    {
                        if(currentPoster != null)
                        {
                            movie.PosterName = currentPoster;
                        }
                        else
                        {
                            movie.PosterName = "default_poster.jpg";
                        }

                    }

                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["Directors"] = CreateDirectorDropdown(); ;

            return View(movie);
        }


        #endregion


        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .FirstOrDefaultAsync(m => m.ID == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie = await _context.Movies.FindAsync(id);

            if (movie.PosterName != "default_poster.jpg" &&
                movie.PosterName != null)
            {
                //delete image from static folder
                var posterPath = Path.Combine(_hostEnvironment.WebRootPath, "poster", movie.PosterName);
                if (System.IO.File.Exists(posterPath))
                {
                    System.IO.File.Delete(posterPath);
                }
            }

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.ID == id);
        }


        private SelectList CreateDirectorDropdown()
        {
            var directors = _context.Directors.AsNoTracking().ToArray();

            var selectList = new SelectList(
                directors.Select(i => new SelectListItem { Text = i.Name, Value = i.ID.ToString() }),
                 "Value",
                "Text");
            return selectList;
        }

    }
}