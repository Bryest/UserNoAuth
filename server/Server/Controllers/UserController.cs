using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Server.API.Security.Domain.Services;
using Server.API.Server.Domain.Models;
using Server.API.Server.Domain.Services;
using Server.API.Server.Resources.User;
using Server.API.Shared.Extensions;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mail;

namespace Server.API.Server.Controllers
{
    [ApiController]
    [Route("/api/v1/[controller]")]
    [Produces("application/json")]
    [SwaggerTag("Get all, get by id, get by email, create, update and delete User")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;


        private const string MimeType = "image/png";
        private const string FileName = "Image.png";

        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }


        [HttpGet]
        [SwaggerOperation(Summary = "Get all users")]
        public async Task<IActionResult> GetAllAsync()
        {
            var users = await _userService.ListAsync();

            if (users.IsNullOrEmpty())
                return BadRequest(
                    new
                    {
                        Success = true,
                        Message = "We don't have any user... Yet!.",
                        Resource = string.Empty
                    });

            var resources = _mapper.Map<IEnumerable<User>, IEnumerable<UserResource>>(users);

            return Ok(resources);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get a user by id")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.FindByIdAsync(id);

            if (user == null)
                return BadRequest(
                    new
                    {
                        Success = true,
                        Message = "User don't exist.",
                        Resource = string.Empty
                    });

            var resource = _mapper.Map<User, UserResource>(user);

            return Ok(resource);
        }

        [HttpGet("id/{id}/imageprofile")]
        [SwaggerResponse(200, FileName)]
        [SwaggerOperation(Summary = "Get a user by id")]
        public async Task<IActionResult> GetUserImageProfileById(int id)
        {
            var user = await _userService.FindByIdAsync(id);

            if (user == null)
                return BadRequest(
                    new
                    {
                        Success = true,
                        Message = "User don't exist.",
                        Resource = string.Empty
                    });

            if (user.ImageProfile.Length == 0)
                return BadRequest(
                    new
                    {
                        Success = true,
                        Message = "User don't have an Image Profile.",
                        Resource = string.Empty
                    });


            MemoryStream ms = new MemoryStream(user.ImageProfile);

            return new FileStreamResult(ms, MimeType);
        }

        [HttpGet("email/{email}")]
        [SwaggerOperation(Summary = "Get a user by email")]
        public async Task<UserResource> GetUserByEmail(string email)
        {
            var user = await _userService.FindByEmailAsync(email);
            var resource = _mapper.Map<User, UserResource>(user);

            return resource;
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Create a new User")]
        [SwaggerResponse(200, "The operation was successful", typeof(UserResource))]
        [SwaggerResponse(400, "Data is not valid")]
        public async Task<IActionResult> PostAsync([FromForm, SwaggerRequestBody("User infromation to add.")] SaveUserResource resource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            //Valid email format
            var isEmail = new MailAddress(resource.Email);
            if (isEmail == null)
                return BadRequest(ModelState.GetErrorMessages());


            // Convert FromFile to byte[] to save image in SQL
            var newUser = new User();
            using (var memoryStream = new MemoryStream())
            {
                if (resource.ImageProfile != null)
                    await resource.ImageProfile.CopyToAsync(memoryStream);

                newUser = new User()
                {
                    FirstName = resource.FirstName,
                    LastName = resource.LastName,
                    Email = resource.Email,
                    Password = resource.Password,
                    ImageProfile = memoryStream != null ? memoryStream.ToArray() : null,
                };
            }

            //var user = _mapper.Map<SaveUserResource, User>(resource);

            var result = await _userService.SaveAsync(newUser);

            if (!result.Success)
                return BadRequest(new
                {
                    result.Success,
                    result.Message,
                    result.Resource
                });

            var userResource = _mapper.Map<User, UserResource>(result.Resource);

            return Ok(userResource);
        }

        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update user data by id")]
        [SwaggerResponse(200, "The operation was successful", typeof(UserResource))]
        [SwaggerResponse(400, "Data is not valid")]
        public async Task<IActionResult> PutAsync(int id, [FromForm, SwaggerRequestBody("User infromation to edit.")] SaveUserResource resource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            // Validate email format
            var isEmail = new MailAddress(resource.Email);
            if (isEmail == null)
                return BadRequest(ModelState.GetErrorMessages());

            // Convert FromFile to byte[] to save image in SQL
            var newUser = new User();
            using (var memoryStream = new MemoryStream())
            {
                if (resource.ImageProfile != null)
                    await resource.ImageProfile.CopyToAsync(memoryStream);

                newUser = new User()
                {
                    FirstName = resource.FirstName,
                    LastName = resource.LastName,
                    Email = resource.Email,
                    Password = resource.Password,
                    ImageProfile = memoryStream != null ? memoryStream.ToArray() : null,
                };
            }


            //var user = _mapper.Map<SaveUserResource, User>(newUser);

            var result = await _userService.UpdateAsync(id, newUser);

            if (!result.Success)
                return BadRequest(new
                {
                    result.Success,
                    result.Message,
                    result.Resource
                });


            var userResource = _mapper.Map<User, UserResource>(result.Resource);

            return Ok(userResource);
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete a user by id")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _userService.DeleteAsync(id);

            if (!result.Success)
                return BadRequest(result.Message);

            var userResource = _mapper.Map<User, UserResource>(result.Resource);

            return Ok(userResource);
        }
    }
}
