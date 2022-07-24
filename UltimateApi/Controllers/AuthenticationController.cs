using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UltimateApi.ActionFilters;

namespace UltimateApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IAuthentificationManager _authentificationManager;
        public AuthenticationController(
            ILoggerManager logger,
            IMapper mapper,
            UserManager<User> userManager,
            IAuthentificationManager authentificationManager
            )
        {
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
            _authentificationManager = authentificationManager;
        }
        [HttpPost]
        [ServiceFilter(typeof(ValidationAttributeFilter))]
        public async Task<IActionResult> RegisterUser([FromBody]UserForRegistrationDto registrationDto)
        {
            var user = _mapper.Map<User>(registrationDto);
            var result =  await _userManager.CreateAsync(user, registrationDto.Password);
            if(!result.Succeeded)
            {
                foreach(var error in result.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }
                return BadRequest(ModelState);
            }
            await _userManager.AddToRolesAsync(user, registrationDto.Roles);
            return StatusCode(201);
        }
        [HttpPost("login")]
        [ServiceFilter(typeof (ValidationAttributeFilter))]
        public async Task<IActionResult> Authenticate([FromBody] UserForAuthenticationDto authenticationDto)
        {
            if(!await _authentificationManager.ValidateUser(authenticationDto))
            {
                _logger.LogWarn($"{nameof(Authenticate)} : Authentication failed. Wrong user name or password.");
                return Unauthorized();
            }
            return Ok(new { Token = await _authentificationManager.CreateToken() });
        }
    }
}
