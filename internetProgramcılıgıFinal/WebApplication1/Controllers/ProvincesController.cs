using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Dtos;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProvincesController : ControllerBase
    {
        private readonly Context _context;
        private readonly IMapper _mapper;
        ResultDto result = new ResultDto();

        public ProvincesController(Context context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpGet]
        public List<ProvinceDto> GetList()
        {
            var provinces = _context.Provinces.ToList();
            var provincesDtos = _mapper.Map<List<ProvinceDto>>(provinces);
            return provincesDtos;
        }
        [HttpGet("{id}")]
        public ProvinceDto Get(int id)
        {
            var Province = _context.Provinces.FirstOrDefault(x => x.Id == id);
            var ProvinceDtos = _mapper.Map<ProvinceDto>(Province);
            return ProvinceDtos;
        }
  
        [HttpGet("GetAllProvinceWithHouses")]
        public List<ProvinceDto> GetAllProvinceWithHouses()
        {

            var Province = _context.Provinces
                                   .Include(d => d.Houses)
                                   .ToList();

            var districtDtos = Province.Select(d => new ProvinceDto
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
       
        [HttpGet]
        [Route("GetAllProvinceWithHouses/{id}")]
        public List<ProvinceDto> GetAllProvinceWithHouses(int id)
        {

            var Province = _context.Provinces
                                   .Include(d => d.Houses)
                                   .Where(d => d.Id == id)
                                   .ToList();

            var ProvinceDtos = Province.Select(d => new ProvinceDto
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
            return ProvinceDtos;
        }




        [HttpPost]
        public ProvinceAddDto ProvinceAddDto(ProvinceAddDto ProvinceDto)
        {
            var Province = _mapper.Map<Province>(ProvinceDto);
            _context.Provinces.Add(Province);
            _context.SaveChanges();
            return ProvinceDto;
        }
        [HttpPut]
        public ProvinceAddDto ProvinceUpdateDto(ProvinceAddDto ProvinceDto)
        {
            var Province = _mapper.Map<Province>(ProvinceDto);
            _context.Provinces.Update(Province);
            _context.SaveChanges();
            return ProvinceDto;
        }
        [HttpDelete("{id}")]
        public ResultDto Delete(int id)
        {
            var Province = _context.Provinces.FirstOrDefault(x => x.Id == id);
            if (Province != null)
            {
                _context.Provinces.Remove(Province);
                _context.SaveChanges();
                result.Status = true;
                result.Message = "Province deleted successfully";
            }
            else
            {
                result.Status = false;
                result.Message = "Province not found";
            }
            return result;
        }
    }
}
