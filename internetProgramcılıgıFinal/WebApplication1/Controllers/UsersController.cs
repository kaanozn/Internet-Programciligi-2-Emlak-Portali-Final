using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplication1.Dtos;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/User/[action]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly Context _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        ResultDto result = new ResultDto();

        public UsersController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, SignInManager<AppUser> signInManager, Context context, IConfiguration configuration, IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _context = context;
            _configuration = configuration;
            _mapper = mapper;
        }
     
        [HttpGet]
        public List<UserDto> UserList()
        {
            var users = _userManager.Users.ToList();
            return _mapper.Map<List<UserDto>>(users);
        }

        [HttpGet]
        public UserDto GetUser(string id)
        {
            var user = _userManager.FindByIdAsync(id).Result;
            return _mapper.Map<UserDto>(user);
        }
  
        [HttpGet]
        [Route("GetUserHouses/{id}")]
        public List<UserDto> GetUserHouses(string id)
        {
            var districts = _context.Users
                                    .Include(d => d.House)
                                    .Where(d=>d.Id == id)
                                    .ToList();

            var districtDtos = districts.Select(d => new UserDto
            {
                Id=d.Id.ToString(),
                FullName = d.FullName,
                UserName = d.UserName,
                Email = d.Email,
                PhoneNumber = d.PhoneNumber,


                Houses = d.House.Select(h => new HouseDto
                {
                    Id = h.Id,
                    HouseImagesId = h.HouseImagesId,
                    ProvinceId = h.ProvinceId,
                    DistrictId = h.DistrictId,
                    AppUserId = h.AppUserId,
                    Name = h.Name,
                    Address = h.Address,
                    RoomCount = h.RoomCount,
                    FloorCount = h.FloorCount,
                    Area = h.Area,
                    Price = h.Price,
                    Description = h.Description
                }).ToList()
            }).ToList();
            return districtDtos;
        }
      
        [HttpGet]
        [Route("GetUserHouses/")]
        public List<UserDto> GetUserHouses()
        {
            var districts = _context.Users
                                    .Include(d => d.House)
                                    .ToList();

            var districtDtos = districts.Select(d => new UserDto
            {
                Id = d.Id.ToString(),
                FullName = d.FullName,
                UserName = d.UserName,
                Email = d.Email,
                PhoneNumber = d.PhoneNumber,


                Houses = d.House.Select(h => new HouseDto
                {
                    Id = h.Id,
                    HouseImagesId = h.HouseImagesId,
                    ProvinceId = h.ProvinceId,
                    DistrictId = h.DistrictId,
                    AppUserId = h.AppUserId,
                    Name = h.Name,
                    Address = h.Address,
                    RoomCount = h.RoomCount,
                    FloorCount = h.FloorCount,
                    Area = h.Area,
                    Price = h.Price,
                    Description = h.Description
                }).ToList()
            }).ToList();
            return districtDtos;
        }



        [HttpPost]
        public async Task<ResultDto> Add(RegisterDto dto)
        {
            var identityResult = await _userManager.CreateAsync(new() { UserName = dto.UserName, Email = dto.Email, FullName = dto.FullName, PhoneNumber = dto.PhoneNumber }, dto.Password);

            if (!identityResult.Succeeded)
            {
                result.Status = false;
                foreach (var item in identityResult.Errors)
                {
                    result.Message += "<p>" + item.Description + "<p>";
                }

                return result;
            }
            var user = await _userManager.FindByNameAsync(dto.UserName);
            var roleExist = await _roleManager.RoleExistsAsync("Kullanici");
            if (!roleExist)
            {
                var role = new AppRole { Name = "Kullanici" };
                await _roleManager.CreateAsync(role);
            }

            await _userManager.AddToRoleAsync(user, "Kullanici");
            result.Status = true;
            result.Message = "Üye Eklendi";
            return result;
        }

        [HttpPost]
        public async Task<UserRole> AddRole(UserRole dto)
        {
            var user = await _userManager.FindByIdAsync(dto.UserId);
            var roleExist = await _roleManager.RoleExistsAsync(dto.RoleName);
            if (!roleExist)
            {
                var role = new AppRole { Name = dto.RoleName };
                await _roleManager.CreateAsync(role);
            }

            await _userManager.AddToRoleAsync(user, dto.RoleName);
            return dto;
        }
        

        [HttpPost]
        public async Task<ResultDto> SignIn(LoginDto dto)
        {
            var user = await _userManager.FindByNameAsync(dto.UserName);

            if (user is null)
            {
                result.Status = false;
                result.Message = "Üye Bulunamadı!";
                return result;
            }
            var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, dto.Password);

            if (!isPasswordCorrect)
            {
                result.Status = false;
                result.Message = "Kullanıcı Adı veya Parola Geçersiz!";
                return result;
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim("JWTID", Guid.NewGuid().ToString()),

            };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var token = GenerateJWT(authClaims);

            result.Status = true;
            result.Message = token;
            return result;

        }
        private string GenerateJWT(List<Claim> claims)
        {

            var accessTokenExpiration = DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["AccessTokenExpiration"]));


            var authSecret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            var tokenObject = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    expires: accessTokenExpiration,
                    claims: claims,
                    signingCredentials: new SigningCredentials(authSecret, SecurityAlgorithms.HmacSha256)
                );

            string token = new JwtSecurityTokenHandler().WriteToken(tokenObject);

            return token;
        }
        [HttpGet]
        [Route("{id}")]
        public async Task<ResultDto> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user is null)
            {
                result.Status = false;
                result.Message = "Üye Bulunamadı!";
                return result;
            }
            var identityResult = await _userManager.DeleteAsync(user);
            if (!identityResult.Succeeded)
            {
                result.Status = false;
                foreach (var item in identityResult.Errors)
                {
                    result.Message += "<p>" + item.Description + "<p>";
                }

                return result;
            }
            result.Status = true;
            result.Message = "Üye Silindi";
            return result;
        }

        [HttpGet]
        public async Task<ResultDto> SignOut()
        {
            await _signInManager.SignOutAsync();
            result.Status = true;
            result.Message = "";
            return result;
        }
    }
}
