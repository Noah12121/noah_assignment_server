using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly UserDbContext _context;
        public UserController(UserDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        //get all users
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {


                string serverUrl = "https://dummy.restapiexample.com/api/v1/employees";
                WebRequest request = WebRequest.Create(serverUrl);
                request.Method = "GET";

                using (WebResponse response = request.GetResponse())
                using (Stream dataStream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(dataStream))
                {
                    string responseFromServer = reader.ReadToEnd();

                    try

                    {
                        List<User> userList = JsonConvert.DeserializeObject<JsonUser>(responseFromServer).users;
                        //save to db
                        using (var context = new UserDbContext())
                        {
                  
                            try
                            {
                               
                                foreach (var user in userList)
                                {
                                    _context.Users.Add(user);
                                }
                                await _context.SaveChangesAsync();

                               
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.InnerException.Message);
                            }

                           
                        }
                        
                        return Ok(userList);
                    }

                    catch (JsonException) // Invalid JSON

                    {
                        Console.WriteLine("Invalid JSON.");
                    }

                    return Ok(responseFromServer);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred: " + ex.Message);
            }
        }

        //get user by id
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            return await _context.Users.Where(u => u.id == id).FirstOrDefaultAsync();
        }
        //update
        [HttpPost]
        public async Task<ActionResult<User>> Post([FromBody] User user)
        {

            var u = await _context.Users.FindAsync(user.id);
            if (u == null)
            {
                return null;
            }

            u.employee_name = user.employee_name;
            u.employee_salary = user.employee_salary;
            u.employee_age = user.employee_age;
            u.profile_image = user.profile_image;


            await _context.SaveChangesAsync();
            return Ok();

        }


    }
}
   