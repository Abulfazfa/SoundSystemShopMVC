using AutoMapper;
using SoundSystemShop.Helper;
using SoundSystemShop.Models;
using SoundSystemShop.Services.Interfaces;
using SoundSystemShop.ViewModels;

namespace SoundSystemShop.Services
{
    public interface ISliderService
    {
        Task<List<Slider>> GetAllSliders();
        Task CreateSlider(SliderVM sliderVM);
        Task<bool> DeleteSlider(int id);
        Slider GetSliderById(int id);
        SliderVM MapSliderVMAndSlider(int id);
        Task<bool> UpdateSlider(int id, SliderVM sliderVM);
    }
    public class SliderService : ISliderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMapper _mapper;
        public SliderService(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
            _mapper = mapper;
        }

        public async Task CreateSlider(SliderVM sliderVM)
        {
            if (!sliderVM.Photo.CheckFileType())
            {
                throw new ArgumentException("Select an image.");
            }

            Slider slider = _mapper.Map<Slider>(sliderVM);
            if (sliderVM.Photo != null)
            {
                slider.ImgUrl = sliderVM.Photo.SaveImage(_webHostEnvironment, "assets/img/hero");
            }

            await _unitOfWork.SliderRepo.AddAsync(slider);
            _unitOfWork.Commit();
        }

        public async Task<bool> DeleteSlider(int id)
        {
            var slider = _unitOfWork.SliderRepo.GetByIdAsync(id).Result;

            if (slider != null)
            {
                string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets/img/slider", slider.ImgUrl);
                DeleteHelper.DeleteFile(path);
                await _unitOfWork.SliderRepo.DeleteAsync(slider);
                _unitOfWork.Commit();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<List<Slider>> GetAllSliders()
        {
            return await _unitOfWork.SliderRepo.GetAllAsync();
        }

        public Slider GetSliderById(int id)
        {
            return _unitOfWork.SliderRepo.GetByIdAsync(id).Result;
        }

        public SliderVM MapSliderVMAndSlider(int id)
        {
            var slider = GetSliderById(id);
            if (slider == null) return null;
            SliderVM sliderVM = _mapper.Map<SliderVM>(slider);
            sliderVM.ImgUrl = slider.ImgUrl;
            return sliderVM;
        }

        public async Task<bool> UpdateSlider(int id, SliderVM sliderVM)
        {
            var slider = await _unitOfWork.SliderRepo.GetByIdAsync(id);
            if(slider == null) return false;
            if (sliderVM.Photo != null)
            {
                bool exist = _unitOfWork.BannerRepo.Any(b => b.ImgUrl.ToLower() == sliderVM.Photo.FileName.ToLower() && b.Id != id);
                if (!exist)
                {
                    string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets/img/banner", slider.ImgUrl);
                    DeleteHelper.DeleteFile(path);
                    slider.ImgUrl = sliderVM.Photo.SaveImage(_webHostEnvironment, "assets/img/hero");
                }
            }
            slider.Header = sliderVM.Header;
            slider.Title = sliderVM.Title;
            slider.Description = sliderVM.Description;
            _unitOfWork.Commit();
            return true;
        }
    }
}
