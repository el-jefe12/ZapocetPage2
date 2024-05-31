using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TexasGuyContractIdentity.Data;
using System.Globalization;
using System;

namespace TexasGuyContractIdentity.Pages.API
{
    [ApiController]
    [Route("report")]
    public class FloodController : ControllerBase
    {
        private ApplicationDbContext _context { get; set; }

        public FloodController(ApplicationDbContext context)
        {
            _context = context;
        }

        //[HttpPost]
        //[Route("add-json")]
        /*
        public IActionResult AddJson(JsonDataHandler json)
        {
            try
            {
                if (json != null && json.TimeStamp != DateTime.MinValue)
                {
                    var river = _context.StationsEntries.Find(json.Station);

                    // Perform operations using 'river' or any other logic here

                    return Ok("Success"); // Assuming you want to return a success response
                }
                else
                {
                    return BadRequest("Invalid JSON data"); // Return a bad request response if JSON or timestamp is invalid
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it accordingly
                return StatusCode(500, "Internal Server Error");
            }
        }
                */
    }
}

