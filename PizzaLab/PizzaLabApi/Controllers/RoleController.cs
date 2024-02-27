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
    public class RoleController : ControllerBase
    {
        private IRoleRepository roleRepository;
        private readonly IMapper mapper;
        MapperConfiguration config = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
        public RoleController(IRoleRepository roleRepository)
        {
            this.roleRepository = roleRepository;
            mapper = config.CreateMapper();
        }

        [HttpGet("[action]")]
        public IActionResult GetAllRole()
        {
            return Ok(roleRepository.GetRoles().Select(u => mapper.Map<RoleDTO>(u)));
        }
        [HttpGet("[action]")]
        public IActionResult GetARole(int Id)
        {
            Role role = null;
            try
            {
                List<Role> roles = roleRepository.GetRoles().ToList();
                role = roles.FirstOrDefault(u => u.Id == Id);
            }
            catch (Exception ex)
            {
                return Conflict("No Role In DB");
            }
            if (role == null)
                return NotFound("Role doesnt exist");
            return Ok(mapper.Map<RoleDTO>(role));
        }
    }
}
