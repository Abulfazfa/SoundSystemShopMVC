using AutoMapper;
using SoundSystemShop.Helper;
using SoundSystemShop.Models;
using SoundSystemShop.Services.Interfaces;
using SoundSystemShop.ViewModels;

namespace SoundSystemShop.Services
{
    public interface IBannerService
    {
        Task<List<Banner>> GetAllBanners();
        Task CreateBanner(BannerVM bannerVM);
        Task<bool> DeleteBanner(int id);
        Banner GetBannerById(int id);
        BannerVM MapBannerVMAndBanner(int id);
        Task<bool> UpdateBanner(int id, BannerVM bannerVM);
    }
    public class BannerService : IBannerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMapper _mapper;

        public BannerService(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
            _mapper = mapper;
        }

        public async Task<List<Banner>> GetAllBanners()
        {
            return await _unitOfWork.BannerRepo.GetAllAsync();
        }

        public async Task CreateBanner(BannerVM bannerVM)
        {
            if (!bannerVM.Photo.CheckFileType())
            {
                throw new ArgumentException("Select an image.");
            }

            Banner banner = _mapper.Map<Banner>(bannerVM);
            banner.ImgUrl = bannerVM.Photo.SaveImage(_webHostEnvironment, "assets/img/banner");
            await _unitOfWork.BannerRepo.AddAsync(banner);
            _unitOfWork.Commit();
        }

        public async Task<bool> DeleteBanner(int id)
        {
            var banner = _unitOfWork.BannerRepo.GetByIdAsync(id).Result;

            if (banner != null)
            {
                string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets/img/banner", banner.ImgUrl);
                DeleteHelper.DeleteFile(path);
                await _unitOfWork.BannerRepo.DeleteAsync(banner);
                _unitOfWork.Commit();
                return true;
            }
            else
            {
                return false;
            }
        }

        public Banner GetBannerById(int id)
        {
            return _unitOfWork.BannerRepo.GetByIdAsync(id).Result;
        }

        public async Task<bool> UpdateBanner(int id, BannerVM bannerVM)
        {
            var banner = await _unitOfWork.BannerRepo.GetByIdAsync(id);
            if(banner == null) return false;
            if (bannerVM.Photo != null)
            {
                bool exist = _unitOfWork.BannerRepo.ExistsWithImgUrl(bannerVM.Photo.FileName, id);
                if (!exist)
                {
                    string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets/img/banner", banner.ImgUrl);
                    DeleteHelper.DeleteFile(path);
                    banner.ImgUrl = bannerVM.Photo.SaveImage(_webHostEnvironment, "assets/img/banner");
                }
            }
            banner = _mapper.Map<Banner>(bannerVM);
            _unitOfWork.Commit();
            return true;
        }
        public BannerVM MapBannerVMAndBanner(int id)
        {
            var banner = GetBannerById(id);
            if (banner == null) return null;
            BannerVM bannerVM = _mapper.Map<BannerVM>(banner);
            bannerVM.ImgUrl = banner.ImgUrl;
            return bannerVM;
        }
    }

}
