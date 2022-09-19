using CinemaApi2.Data;
using CinemaApi2.Models;
using CinemaApi2.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace CinemaApi2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {

        public  ApplicationDbContext _db { get; set; }
        private IWebHostEnvironment _env { get; set; }

        public MoviesController(ApplicationDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }


        [HttpPost("AddMovie")]
        public async Task<IActionResult> AddMovies([FromForm]MoviesViewModel NewMovieObject)
        {
            try
            {
                var CheckIfMovieExists = await _db.MoviesTable.FirstOrDefaultAsync(a => a.MovieName == NewMovieObject.MovieName);

                if (CheckIfMovieExists is null)
                {
                    var DbModel = new MoviesModel();

                    DbModel.MovieName = NewMovieObject.MovieName;
                    DbModel.MovieCategory = NewMovieObject.MovieCategory;
                    DbModel.MovieVideoLink = NewMovieObject.MovieVideoLink;
                    DbModel.MovieRating = NewMovieObject.MovieRating;
                    DbModel.DateOfRelease = NewMovieObject.DateOfRelease;
                    DbModel.DirectorName = NewMovieObject.DirectorName;
                    DbModel.MainStarName = NewMovieObject.MainStarName;
                    DbModel.MovieThumbNail = await InputImage(NewMovieObject.ImageFile);

                    await _db.MoviesTable.AddAsync(DbModel);
                    await _db.SaveChangesAsync();
                    return Ok("The Movie Had Been Added To The System");

                }
                else
                {
                    return Ok("The Movie Already Exists in the database");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }


        }


        [NonAction]
        private async Task<string> InputImage(IFormFile MovieImage)
        {
            var FileName = MovieImage.FileName;
            var FullName = Guid.NewGuid().ToString() + FileName ;

            var FolderDirectory = $"{_env.WebRootPath}//Images";
            var FullPath = Path.Combine(FolderDirectory , FullName);

            var memorystream = new MemoryStream();
            await MovieImage.OpenReadStream().CopyToAsync(memorystream);

            await using (var fs = new FileStream(FullPath, FileMode.Create, FileAccess.Write))
            {
                memorystream.WriteTo(fs);
            }

            return $"https://localhost:7110/Images/{FullName}";
        }








        [HttpPut("EditMovie")]                                     // Id : 10
        public async Task<IActionResult> EditMovies(MoviesModel NewMovieInformations)
        {
            try
            {
                var OldMoiveInformationsInDataBase = await _db.MoviesTable.FirstOrDefaultAsync(a => a.Id == NewMovieInformations.Id);

                OldMoiveInformationsInDataBase.MovieName = NewMovieInformations.MovieName;
                OldMoiveInformationsInDataBase.DirectorName = NewMovieInformations.DirectorName;
                OldMoiveInformationsInDataBase.MainStarName = NewMovieInformations.MainStarName;
                OldMoiveInformationsInDataBase.MovieCategory = NewMovieInformations.MovieCategory;
                OldMoiveInformationsInDataBase.DateOfRelease = NewMovieInformations.DateOfRelease;
                OldMoiveInformationsInDataBase.MovieRating = NewMovieInformations.MovieRating;

                _db.MoviesTable.Update(OldMoiveInformationsInDataBase);
                await _db.SaveChangesAsync();

                return Ok($"The Movie {OldMoiveInformationsInDataBase.MovieName} has been updated successfully");

            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }



        [HttpGet("GetMoviesByCatagory")]
        public async Task<IActionResult> GetMoviesByCatagory1(string Catagory)
        {
            var ListOfMovies = await _db.MoviesTable.Where(a=>a.MovieCategory.ToLower().Contains(Catagory.ToLower())).ToListAsync();
            return Ok(ListOfMovies);
        }



        [HttpGet("GetMovies")]
        public async Task<IActionResult> GetAllMovies()
        {
            var ListOfMovies = await _db.MoviesTable.ToListAsync();
            return Ok(ListOfMovies);
        }



        [HttpGet]
        [Route("GetMovieById")]
        public async Task<IActionResult> GetMovieByMovieId(int idSentByUser)
        {
            var Movie = await _db.MoviesTable.SingleOrDefaultAsync(a=>a.Id == idSentByUser);
            if (Movie is null)
            {
                return BadRequest($"The Movie With the Id {idSentByUser} is not available in the database");
            }
            else
            {
                return Ok(Movie);
            }
        }


        [HttpDelete]
        [Route("DeleteMovieById")]
        public async Task<IActionResult> DeleteMovieByMovieId(int IdOfTheMovieToBeDeleted)
        {
            // Get The Id from the user and Delete the movie from the databas
            //using EntityFrameWork 
            try
            {

                var Movie = await _db.MoviesTable.SingleOrDefaultAsync(a => a.Id == IdOfTheMovieToBeDeleted);
                if (Movie is null)
                {
                    return Ok("The Movie Is not available in the datatabse for now");
                }
                else
                {
                    _db.MoviesTable.Remove(Movie);
                    await _db.SaveChangesAsync();
                    return Ok($"The Movie with the Id {IdOfTheMovieToBeDeleted} Has been deleted Successfully");
                }

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }












    }
}
