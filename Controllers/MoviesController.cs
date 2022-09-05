using CinemaApi2.Data;
using CinemaApi2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaApi2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {

        public ApplicationDbContext _db { get; set; }


        public MoviesController(ApplicationDbContext db)
        {
            _db = db;
        }


        [HttpPost("AddMovie")]
        public async Task<IActionResult> AddMovies([FromForm]MoviesModel NewMovieObject)
        {
            try
            {
                var CheckIfMovieExistInDataBase = await _db.MoviesTable.FirstOrDefaultAsync(a => a.MovieName == NewMovieObject.MovieName);

                if (CheckIfMovieExistInDataBase is null)
                {
                    await _db.MoviesTable.AddAsync(NewMovieObject);
                    await _db.SaveChangesAsync();
                    return Ok("The Movie Has Been Added Successfully to the Database");
                }
                else
                {
                    return BadRequest($"The Movie : {NewMovieObject.MovieName} is already regestered in the database");
                }

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
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
