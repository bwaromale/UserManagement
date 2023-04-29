using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Text;
using UserManagement_DataAccess;
using Usermanagement_Domain.DTOs;
using Usermanagement_Domain.Interfaces;
using Usermanagement_Domain.Models;

namespace UserManagement_Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class UsersController : ControllerBase
    {
        private readonly IUser _repo;
        private readonly UserManagementContext _context;
        private readonly IDistributedCache distributedCache;

        public UsersController(IUser repo, UserManagementContext context, IDistributedCache distributedCache)
        {
            _repo = repo;
            _context = context;
            this.distributedCache = distributedCache;
        }

        [HttpGet("AllUsers")]
        public async Task<IEnumerable<User>> GetUsers()
        {
            IEnumerable<User> users = await _repo.GetAllAsync();
            return users;
        }
        [HttpGet("User/{username}")]
        public async Task<IActionResult> GetUser(string username)
        {

            var user = await _repo.GetAsync(u=>u.UserName == username);
            if (user == null)
            {
                return NotFound($"User '{username}' not found");
            }
            return Ok(user);
        }
        [HttpPost]
        public async Task<ActionResult<UpsertDTO>> RegisterUser(UpsertDTO regDTO)
        {
            if (regDTO == null)
            {
                return BadRequest("Invalid input");
            }

            var userExist = await _repo.GetAsync(u=>u.UserName == regDTO.UserName);
            if(userExist != null)
            {
                return BadRequest("Username exists");
            }
            User newuser = new User()
            {
                UserName = regDTO.UserName,
                Password = regDTO.Password,
                Email = regDTO.Email,
                FirstName = regDTO.FirstName,
                LastName = regDTO.LastName,
                Age = regDTO.Age,
                Gender = regDTO.Gender,
                MaritalStatus = regDTO.MaritalStatus,
                Address = regDTO.Address,
                City = regDTO.City,
                State = regDTO.State,
                Country = regDTO.Country,
                RegisteredAt = DateTime.Now
            };
            await _repo.CreateAsync(newuser);
            return Ok("User creation successful");
        }
        [HttpPut]
        public async Task<ActionResult<UpsertDTO>> UpdateUser(UpsertDTO updateDTO)
        {
            if(updateDTO == null)
            {
                return BadRequest("Invalid Input");
            }

            var userExist = await _repo.GetAsync(u=>u.UserName == updateDTO.UserName);
            if(userExist == null)
            {
                return NotFound($"Username '{updateDTO.UserName}' does not exist");
            }


            userExist.Password = updateDTO.Password;
            userExist.Email = updateDTO.Email;
            userExist.FirstName = updateDTO.FirstName;
            userExist.LastName = updateDTO.LastName;
            userExist.Age = updateDTO.Age;
            userExist.Gender = updateDTO.Gender;
            userExist.MaritalStatus = updateDTO.MaritalStatus;
            userExist.Address = updateDTO.Address;
            userExist.City = updateDTO.City;
            userExist.State = updateDTO.State;
            userExist.Country = updateDTO.Country;
            userExist.UpdatedAt = DateTime.Now;

            await _repo.UpdateAsync(userExist);
            return Ok("User Updated Successfully");
        }
        [HttpPost("Filterusers")]
        public async Task<IActionResult> FilterUsers([FromBody] UserFilter filter)
        {
            if (filter == null)
            {
                return BadRequest("Invalid Input");
            }

            var users = await _repo.GetFilteredUsersAsync(filter);

            return Ok(users);
        }
        [HttpDelete("user/{username}")]
        public async Task<IActionResult> DeleteUser(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return BadRequest("Invalid Input");
            }

            var userExist = await _repo.GetAsync(u => u.UserName == username);
            if(userExist == null)
            {
                return NotFound($"Username '{username}' not found");
            }

            await _repo.DeleteAsync(d=>userExist.UserName == username);
            return Ok($"'{username}' successfully deleted");
        }
        [HttpDelete("DeleteAll")]
        public async Task<IActionResult> DeleteAllUsers()
        {
            await _repo.DeleteAllAsync();
            return Ok();
        }
        //[HttpGet("getAllWithRedis")]
        //public async Task<IActionResult> GetAllWithRedis()
        //{
        //    var cacheKey = "userList";
        //    string serialisedAllUsers;    
        //    var userList  = await _repo.GetAllAsync();
        //    var redisUserList = distributedCache.GetAsync(cacheKey);
        //    if (redisUserList != null)
        //    {
        //        serialisedAllUsers = Encoding.UTF8.GetString(redisUserList);
        //        userList = JsonConvert.DeserializeObject<IEnumerable<User>>(redisUserList);
        //    }
        //    else
        //    {
        //        userList =  await _repo.GetAllAsync();
        //    }
        //    return Ok();
        //}
    }
}
