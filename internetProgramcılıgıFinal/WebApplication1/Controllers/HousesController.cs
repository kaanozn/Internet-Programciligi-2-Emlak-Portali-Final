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
    //[Authorize]
    public class HousesController : ControllerBase
    {
        private readonly Context _context;
        private readonly IMapper _mapper;
        ResultDto result = new ResultDto();

        public HousesController(Context context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpGet]
        public List<HouseDto> GetList()
        {
            var houses = _context.Houses.ToList();
            var housesDtos = _mapper.Map<List<HouseDto>>(houses);
            return housesDtos;
        }
       
        [HttpGet("{id}")]
        public HouseDto Get(int id)
        {
            var house = _context.Houses.FirstOrDefault(x => x.Id == id);
            var houseDtos = _mapper.Map<HouseDto>(house);
            return houseDtos;
        }
        [HttpPost]
        public HouseDto HouseAddDto(HouseDto houseDto)
        {
            var house = _mapper.Map<House>(houseDto);
            _context.Houses.Add(house);
            _context.SaveChanges();
            return houseDto;
        }
        [HttpPut]
        public HouseDto HouseUpdateDto(HouseDto houseDto)
        {
            var house = _mapper.Map<House>(houseDto);
            _context.Houses.Update(house);
            _context.SaveChanges();
            return houseDto;
        }
        [HttpGet]
        [Route("GetDetailList")]
        public List<GetDetailHostDto> GetDetailList()
        {
            var houses = _context.Houses
                .Include(x => x.HouseImages)
                .Include(x => x.AppUser)
                .Include(x => x.District)
                .Include(x => x.Province)
                .ToList();

            var housesDtos = _mapper.Map<List<GetDetailHostDto>>(houses);
            return housesDtos;
        }
        [HttpDelete("{id}")]
        public ResultDto Delete(int id)
        {
            var house = _context.Houses.FirstOrDefault(x => x.Id == id);
            if (house != null)
            {
                _context.Houses.Remove(house);
                _context.SaveChanges();
                result.Status = true;
                result.Message = "House deleted successfully";
            }
            else
            {
                result.Status = false;
                result.Message = "House not found";
            }
            return result;
        }
    }
}
