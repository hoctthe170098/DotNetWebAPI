using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAPI1.Data;
using WebAPI1.Models;
using System.Security.Cryptography;
using Microsoft.IdentityModel.JsonWebTokens;
using WebAPI1.Migrations;
using Microsoft.EntityFrameworkCore;
namespace WebAPI1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly MyDbContext _context;
        private readonly AppSettings _appSettings;
        public UserController(MyDbContext context,IOptionsMonitor<AppSettings>optionsMonitor) 
        {
            _context=context;
            _appSettings = optionsMonitor.CurrentValue;
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Validate(LoginModel model)
        {
            var user = _context.NguoiDungs.SingleOrDefault(p=>p.UserName == model.UserName&&p.Password==model.Password);
            if (user == null)
            {
                return Ok(new ApiResponse
                {
                    Success = false,
                    Message = "Invalid username/password"
                });
            }
            // cap Token
            var token = await GenerateToken(user);
            return Ok(new ApiResponse
            {
                Success = true,
                Message = "Authenticate success",
                Data = token
            }); 
        }
        private async Task<TokenModel> GenerateToken(NguoiDung nguoiDung)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secretKeyBytes = Encoding.UTF8.GetBytes(_appSettings.SecretKey);
            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, nguoiDung.HoTen),
                    new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Email, nguoiDung.Email),
                    new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("Username", nguoiDung.UserName),
                    new Claim("Id", nguoiDung.Id.ToString()),
                }),
                Expires = DateTime.UtcNow.AddSeconds(20),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(secretKeyBytes),SecurityAlgorithms.HmacSha256Signature)
            };
            var token = jwtTokenHandler.CreateToken(tokenDescription);

            var accessToken= jwtTokenHandler.WriteToken(token);
            var refreshToken = GenerateRefreshToken();
            var refreshTokenEntity = new Data.RefreshToken
            {
                Id = Guid.NewGuid(),
                JwtId = token.Id,
                UserId=nguoiDung.Id,
                Token = refreshToken,
                IsUsed = false,
                IsRevoked = false,
                IssuedAt = DateTime.UtcNow,
                Exprired=DateTime.UtcNow.AddHours(1)
            };
            await _context.AddAsync(refreshTokenEntity);
            await _context.SaveChangesAsync();
            return new TokenModel { 
                AccessToken = accessToken,
                RefreshToken = GenerateRefreshToken()
            };
        }

        private string GenerateRefreshToken()
        {
            var random = new byte[32];
            using(var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(random);
                return Convert.ToBase64String(random);
            }
        }
        [HttpPost("RenewToken")]
        public async Task<IActionResult> RenewToken(TokenModel model)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var secretKeyBytes = Encoding.UTF8.GetBytes(_appSettings.SecretKey);
            var TokenValidateParam  = new TokenValidationParameters
            {
                //tu cap token
                ValidateIssuer = false,
                ValidateAudience = false,

                // ky vao token
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),
                ClockSkew = TimeSpan.Zero,
                ValidateLifetime=false//ko kiem tra token het han
            };
            try
            {
                //check1: AccessToken valid Format
                var tokenVerification = jwtTokenHandler.ValidateToken(model.AccessToken, TokenValidateParam, out var validatedToken);

                // check 2: check thuat toan
                if(validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg
                        .Equals(SecurityAlgorithms.HmacSha256,StringComparison.InvariantCultureIgnoreCase);
                    if(!result) {
                        return Ok(new ApiResponse
                        {
                            Success = false,
                            Message = "Invalid Token"
                        });
                    }
                }
                //check 3: check accessToken exprire?
                var utcExpireDate=long
                    .Parse(tokenVerification
                    .Claims.FirstOrDefault(x=>x.Type== System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Exp).Value);
                var expireDate = ConvertUnixTimeToDateTime(utcExpireDate);
                if(expireDate>DateTime.UtcNow)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Access token has not yet expirted"
                    });
                }
                //check 4: check refresh token exist in DB
                var stiredToken = _context.RefreshTokens.FirstOrDefault(x => x.Token == model.RefreshToken);
                if(stiredToken == null)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Refresh token does exist"
                    });
                }
                //check 5: check RefreshToken is used/revoked?
                if (stiredToken.IsUsed)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = " Refresh Token has been used"
                    });
                }
                if (stiredToken.IsRevoked)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = " Refresh Token has been revoked"
                    });
                }
                //check 6: AccessToken id==jwtId in RefreshToken
                var jti = tokenVerification.Claims
                    .FirstOrDefault(x=>x.Type== System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Jti).Value;
                if (stiredToken.JwtId != jti)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = " Token does not match"
                    });
                }
                // Update token is used
                stiredToken.IsRevoked = true;
                stiredToken.IsUsed = true;
                _context.Update(stiredToken);
                _context.SaveChangesAsync();
                //create new token
                var user = await _context.NguoiDungs.SingleOrDefaultAsync(nd => nd.Id == stiredToken.UserId);
                var token = await GenerateToken(user);
                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Renew success",
                    Data = token
                });
                return Ok(new ApiResponse
                {
                    Success = true,
                });
            }
            catch(Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Something went wrong"
                });
            }
        }

        private DateTime ConvertUnixTimeToDateTime(long utcExpireDate)
        {
            var datetimeInterval = new DateTime(1970,1,1,0,0,0,0, DateTimeKind.Utc);
            datetimeInterval.AddSeconds(utcExpireDate).ToUniversalTime();
            return datetimeInterval;
        }
    }
}
