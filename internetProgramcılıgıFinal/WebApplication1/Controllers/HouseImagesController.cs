using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Dtos;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class HouseImagesController : ControllerBase
    {
        private readonly Context _context;
        private readonly IMapper _mapper;
        ResultDto result = new ResultDto();
        public HouseImagesController(Context context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpGet]
        public List<HouseImagesDto> GetList()
        {
            var HouseImages = _context.HouseImages.ToList();
            var HouseImagesDtos = _mapper.Map<List<HouseImagesDto>>(HouseImages);
            return HouseImagesDtos;
        }
        [HttpGet("{id}")]
        public HouseImagesDto Get(int id)
        {
            var house = _context.HouseImages.FirstOrDefault(x => x.Id == id);
            var HouseImagesDtos = _mapper.Map<HouseImagesDto>(house);
            return HouseImagesDtos;
        }
        [HttpPost]
        public HouseImagesDto HouseAddDto(HouseImagesDto HouseImagesDto)
        {
            var house = _mapper.Map<HouseImages>(HouseImagesDto);
            _context.HouseImages.Add(house);
            _context.SaveChanges();
            return HouseImagesDto;
        }
        [HttpPut]
        public HouseImagesDto HouseUpdateDto(HouseImagesDto HouseImagesDto)
        {
            var house = _mapper.Map<HouseImages>(HouseImagesDto);
            _context.HouseImages.Update(house);
            _context.SaveChanges();
            return HouseImagesDto;
        }
        [HttpDelete("{id}")]
        public ResultDto Delete(int id)
        {
            var house = _context.HouseImages.FirstOrDefault(x => x.Id == id);
            if (house != null)
            {
                _context.HouseImages.Remove(house);
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
