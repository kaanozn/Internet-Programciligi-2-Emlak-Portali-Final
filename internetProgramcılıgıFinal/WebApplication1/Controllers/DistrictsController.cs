using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Dtos;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DistrictsController : ControllerBase
    {
        private readonly Context _context;
        private readonly IMapper _mapper;
        ResultDto result = new ResultDto();

        public DistrictsController(Context context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpGet]
        public List<DistrictDto> GetList()
        {
            var districts = _context.Districts.ToList();
            var districtsDtos = _mapper.Map<List<DistrictDto>>(districts);
            return districtsDtos;
        }
        [HttpGet("{id}")]
        public DistrictDto Get(int id)
        {
            var district = _context.Districts.FirstOrDefault(x => x.Id == id);
            var districtDtos = _mapper.Map<DistrictDto>(district);
            return districtDtos;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllDistrictsWithHouses")]
        public List<DistrictDto> GetAllDistrictsWithHouses()
        {
        
            var districts = _context.Districts
                                   .Include(d => d.Houses) 
                                   .ToList();

            var districtDtos = districts.Select(d => new DistrictDto
            {
                Id = d.Id,
                Name = d.Name,
               
                Houses = d.Houses.Select(h => new HouseDto
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
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("GetAllDistrictsWithHouses/{id}")]
        public List<DistrictDto> GetAllDistrictsWithHouses(int id)
        {

            var districts = _context.Districts
                                   .Include(d => d.Houses)
                                   .Where(d=>d.Id== id)
                                   .ToList();

            var districtDtos = districts.Select(d => new DistrictDto
            {
                Id = d.Id,
                Name = d.Name,

                Houses = d.Houses.Select(h => new HouseDto
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
        public DistrictAddDto DistrictAddDto(DistrictAddDto districtDto)
        {
            var district = _mapper.Map<District>(districtDto);
            _context.Districts.Add(district);
            _context.SaveChanges();
            return districtDto;
        }
        [HttpPut]
        public DistrictAddDto DistrictUpdateDto(DistrictAddDto districtDto)
        {
            var district = _mapper.Map<District>(districtDto);
            _context.Districts.Update(district);
            _context.SaveChanges();
            return districtDto;
        }
        [HttpDelete("{id}")]
        public ResultDto Delete(int id)
        {
            var district = _context.Districts.FirstOrDefault(x => x.Id == id);
            if (district != null)
            {
                _context.Districts.Remove(district);
                _context.SaveChanges();
                result.Status = true;
                result.Message = "District deleted successfully";
            }
            else
            {
                result.Status = false;
                result.Message = "District not found";
            }
            return result;
        }
    }
}
