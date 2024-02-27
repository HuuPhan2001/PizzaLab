using AutoMapper;
using BusinessObject.Models;
using DataAccess.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PizzaLabApi.DTO;
using PizzaLabApi.Models;

namespace PizzaLabApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserRepository userRepository;
        private readonly IMapper mapper;
        MapperConfiguration config = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
        public UserController(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
            mapper = config.CreateMapper();
        }

        [HttpGet("[action]")]
        public IActionResult GetAllUser()
        {
            return Ok(userRepository.GetUsers().Select(u => mapper.Map<UserDTO>(u)));
        }
        [HttpGet("[action]")]
        public IActionResult GetAnUser(int Id)
        {
            User user = null;
            try
            {
                List<User> users = userRepository.GetUsers().ToList();
                user = users.FirstOrDefault(u => u.Id == Id);
            }
            catch (Exception ex)
            {
                return Conflict("No User In DB");
            }
            if (user == null)
                return NotFound("User doesnt exist");
            return Ok(mapper.Map<UserDTO>(user));
        }
        [HttpPost("[action]")]
        public IActionResult GetAnUserLogin(string email,string password)
        {
            User user = null;
            try
            {
                List<User> users = userRepository.GetUsers().ToList();
                user = users.FirstOrDefault(u => u.Email == email&& u.Password == password);
            }
            catch (Exception ex)
            {
                return Conflict("No User In DB");
            }
            if (user == null)
                return NotFound("User doesnt exist");
            return Ok(mapper.Map<UserDTO>(user));
        }
        [HttpPost("[action]")]
        public IActionResult AddAnUser(UserDTO userDTO)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    User user = mapper.Map<User>(userDTO);
                    userRepository.SaveUser(user);
                }
                catch (Exception ex)
                {
                    return Conflict(ex.Message);
                }
                return Ok(userDTO);
            }
            return BadRequest(ModelState.Select(x => x.Value.Errors)
                           .Where(y => y.Count > 0)
                           .ToList());

        }
        [HttpPut("[action]")]
        public IActionResult UpdateAnUser(UserDTO userDTO)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    User user = mapper.Map<User>(userDTO);
                    userRepository.UpdateUser(user);
                }
                catch (Exception ex)
                {
                    return Conflict(ex.Message);
                }
                return Ok(userDTO);
            }
            return BadRequest(ModelState.Select(x => x.Value.Errors)
                           .Where(y => y.Count > 0)
                           .ToList());

        }
        [HttpDelete("[action]")]
        public IActionResult DeleteAnUser(int Id)
        {
            User user = null;
            try
            {
                List<User> users = userRepository.GetUsers().ToList();
                user = users.FirstOrDefault(u => u.Id == Id);
                if (user == null)
                    return NotFound("User doesnt exist");
                userRepository.DeleteUser(user);
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }

            return Ok(userRepository.GetUsers());
        }
    }
}
