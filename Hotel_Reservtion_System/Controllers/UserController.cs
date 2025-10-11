using Hotel_Reservtion_System.DatabaseContext;
using Hotel_Reservtion_System.DTO;
using Hotel_Reservtion_System.Entity;
using Hotel_Reservtion_System.Services;
using Hotel_Reservtion_System.ServicesContracts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hotel_Reservtion_System.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly HoteldbContext _context;
        private readonly IJwtServices _jwtServices;
        public UserController(HoteldbContext context, IJwtServices jwtServices)
        {
            _jwtServices = jwtServices;
            _context = context;
        }


        [AllowAnonymous]
        [Route("api/register")]
        [HttpPost]
        public async Task<IActionResult> register(RegisterDTO registerDTO)
        {
            if (ModelState.IsValid)
            {
                User user = new User();
                user = registerDTO.ConvertToUser();
                User validUser = await _context.Users.FirstOrDefaultAsync(u => u.email == user.email);
                if (validUser == null)
                {


                    if (registerDTO.role.ToLower() == "admin")
                    {
                        var data = await _context.Users.FirstOrDefaultAsync(user => user.role == "admin");
                        if (data != null)
                        {
                            user.isApproved = "false";
                            await _context.Users.AddAsync(user);
                            await _context.SaveChangesAsync();
                            return Ok("your request has been sent to admin wait for approval");

                        }
                        else
                        {
                            user.isApproved = "true";
                        }
                    }
                    else if (registerDTO.role.ToLower() == "employee")
                    {
                        user.isApproved = "false";
                        await _context.Users.AddAsync(user);
                        await _context.SaveChangesAsync();
                        return Ok("your request has been sent to admin wait for approval");
                    }
                    else if (registerDTO.role.ToLower() == "user")
                    {
                        user.isApproved = "true";
                    }
                    await _context.Users.AddAsync(user);
                    await _context.SaveChangesAsync();
                    string token = _jwtServices.GenerateToken(user);
                    AuthenticationResponse response = new AuthenticationResponse(user.email, user.role, token);
                    return Ok(response);
                }
                return BadRequest("email is already in use");

            }
            else
            {
                String errors = string.Join("; ", ModelState.Values
                                       .SelectMany(x => x.Errors)
                                       .Select(x => x.ErrorMessage));

                return BadRequest(errors);
            }
        }


        [AllowAnonymous]
        [Route("api/login")]
        [HttpPost]
        public async Task<IActionResult> login(LoginDTO login)
        {
            User user = await _context.Users.FirstOrDefaultAsync(u => u.email == login.email && u.password == login.password);
            if (user == null)
            {
                return BadRequest("invalid email or password");
            }
            if (user != null)
            {
                if (user.isApproved == "true")
                {
                    string? token = _jwtServices.GenerateToken(user);
                    return Ok(new AuthenticationResponse(user.email, user.role, token));
                }
                else
                {
                    return BadRequest("your request is not approved yet");
                }

            }
            else
            {
                return BadRequest("invalid email or password");
            }
        }
        

        [Authorize(Roles = "admin")]
        [Route("api/pending")]
        [HttpGet]
        public async Task<IActionResult> GetPendingRequests()
        {
            IEnumerable<User> pendingUsers = await _context.Users.Where(u => u.isApproved == "false").ToListAsync();
            if (pendingUsers == null)
            {
                return NotFound("no pending requests");
            }
            return Ok(pendingUsers);

        }



        [Authorize(Roles = "admin")]
        [Route("api/approve")]
        [HttpPut]
        public async Task<IActionResult> ApproveRequest([FromBody]string email)
        {
            User user = await _context.Users.FirstOrDefaultAsync(u => u.email == email);
            if (user == null)
            {
                return NotFound("user not found");
            }
            user.isApproved = "true";
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return Ok("user approved successfully");
        }

        [Authorize(Roles = "user")]
        [HttpGet]
        [Route("api/googleLogin")]
        public IActionResult GoogleLogin()
        {
            var redirectUrl = "https://localhost:44394/api/googleResponse"; // Replace with your actual redirect URL
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet]
        [Route("api/googleResponse")]
        public async Task<IActionResult> googleResponse()
        {
            var result = HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme).Result;
            if (!result.Succeeded)
            {
                return BadRequest("Google authentication failed");
            }
            string? email = result.Principal.FindFirst(c => c.Type == System.Security.Claims.ClaimTypes.Email)?.Value;
            string? name = result.Principal.FindFirst(c => c.Type == System.Security.Claims.ClaimTypes.Name)?.Value;
            string? password = result.Principal.FindFirst(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if(email == null || password == null)
            {
                return BadRequest("Unable to retrieve email or password from Google");
            }
            User? user = await _context.Users.FirstOrDefaultAsync(u => u.email == email);
            if (user == null) { 
                user = new User
                {
                    id = Guid.NewGuid(),
                    email = email,
                    name = name,
                    password = password,
                    confirmPassword = password,
                    role = "user",
                    isApproved = "true"
                };
            }
            var token =_jwtServices.GenerateToken(user);
            return Ok( new AuthenticationResponse(user.email, user.role, token));
        }
    }
}
