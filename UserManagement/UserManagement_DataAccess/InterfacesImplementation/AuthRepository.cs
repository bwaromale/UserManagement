using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Usermanagement_Domain.Interfaces;
using Usermanagement_Domain.Models;

namespace UserManagement_DataAccess.InterfacesImplementation
{
    public class AuthRepository : UserRepository, IAuth
    {
        private readonly ApiSettings _apiSettings;
        private readonly IUser _userRepo;

        public AuthRepository(UserManagementContext context, IOptions<ApiSettings> apiSettings,IUser userRepo, ICaching cacheService) : base(context, cacheService)
        {
            _apiSettings = apiSettings.Value;
            _userRepo = userRepo;
        }

        public async Task<LoginResponse> Login(LoginRequest loginRequest)
        {
            if (loginRequest == null)
            {
                return new LoginResponse() { 
                    Response = new ApiResponse() 
                    { 
                        IsSuccess = false, 
                        StatusCode = HttpStatusCode.BadRequest,
                        ErrorMessage = "Invalid Input"
                    }
                };
            }
            var userExist = await _userRepo.GetAsync(u => u.UserName == loginRequest.Username);
            if (userExist == null)
            {
                return new LoginResponse()
                {
                    Response = new ApiResponse()
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.NotFound,
                        ErrorMessage = "User not found"
                    }
                };
            }
            if(userExist.Password != loginRequest.Password)
            {
                return new LoginResponse()
                {
                    Response = new ApiResponse()
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.BadRequest,
                        ErrorMessage = "Invalid Credentials"
                    }
                };
            }
            string token = CreateToken(userExist);
            return new LoginResponse() { 
                Token = token,
                Response = new ApiResponse()
                {
                    StatusCode = HttpStatusCode.OK,
                }
            };
        }
        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>() 
            { 
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_apiSettings.SecretKey));
            var credentials = new SigningCredentials(key,SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: credentials,
                expires: DateTime.Now.AddDays(1)
                );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
    }
}
