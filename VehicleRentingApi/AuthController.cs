using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VehicleRentingApi.Data;
using VehicleRentingApi.DTO;
using VehicleRentingApi.Entities;

namespace VehicleRentingApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public AuthController(AppDbContext appDbContext, IConfiguration configuration)
        {
            _appDbContext = appDbContext;
            _configuration = configuration;
        }

        AppDbContext _appDbContext { get; }
        IConfiguration _configuration {  get; }
        [HttpPost]
        public async Task<IActionResult> GetToken([FromBody] UserDTO user)
        {
            if (user == null)
                return new ApiResponse<string>("user credentials should not be empty",401);

             if (user.Code == null||user.Password is null)
                return new ApiResponse<string>("user credentials should not be empty", 401);

           var usr=await _appDbContext.Users.FirstOrDefaultAsync(p=>p.Code==user.Code);

            if (user == null)
            {
                return new ApiResponse<string>("Invalid USer Code",401);
            }
        
            if (new PasswordHasher<User>().VerifyHashedPassword(usr, usr.Password, user.Password) == 0)
            {
                return new ApiResponse<string>("Wrong password",401);
            }

            return Ok(new ApiResponse<string>(GenerateJwtToken(usr!.Code)));
        }
        private string GenerateJwtToken(string userCode)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]!));
            var credentials=new SigningCredentials(securityKey,SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>()
            {
                new Claim("Code",userCode)
            };
            var token = new JwtSecurityToken
                (
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                claims:claims,
                expires:DateTime.Now.AddMinutes(30),
                signingCredentials:credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
