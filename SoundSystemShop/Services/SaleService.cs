using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using SoundSystemShop.Helper;
using SoundSystemShop.Models;
using SoundSystemShop.Services.Interfaces;
using SoundSystemShop.ViewModels;

namespace SoundSystemShop.Services
{
    public interface ISaleService
    {
        Task<List<Sale>> GetAll();
        Task<Sale> Get(int id);
        Task Create(SaleVM saleVM);
        Task<bool> Delete(int id);
        Task<bool> Update(int id, SaleVM saleVM);
        SaleVM MapSale(int id);

    }
    public class SaleService : ISaleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMapper _mapper;

        public SaleService(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
            _mapper = mapper;
        }
        public async Task<List<Sale>> GetAll()
        {
            return await _unitOfWork.SaleRepo.GetAllAsync();
        }
        public async Task<Sale> Get(int id)
        {
            return await _unitOfWork.SaleRepo.GetByIdAsync(id);
        }
        public async Task Create(SaleVM saleVM)
        {
            if (!saleVM.Photo.CheckFileType())
            {
                throw new ArgumentException("Select an image.");
            }

            Sale sale = _mapper.Map<Sale>(saleVM);
            foreach (var item in saleVM.ProductIds)
            {
                sale.Products.Add(_unitOfWork.ProductRepo.GetByIdAsync(item).Result);
            }
            sale.ImgUrl = saleVM.Photo.SaveImage(_webHostEnvironment, "assets/img/sale");
            await _unitOfWork.SaleRepo.AddAsync(sale);
            _unitOfWork.Commit();
        }
        public async Task<bool> Delete(int id)
        {
            var sale = _unitOfWork.SaleRepo.GetByIdAsync(id).Result;

            if (sale != null)
            {
                string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets/img/sale", sale.ImgUrl);
                DeleteHelper.DeleteFile(path);
                await _unitOfWork.SaleRepo.DeleteAsync(sale);
                _unitOfWork.Commit();
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task<bool> Update(int id, SaleVM saleVM)
        {
            var sale = await _unitOfWork.SaleRepo.GetByIdAsync(id);
            if (sale == null) return false;
            _mapper.Map<SaleVM, Sale>(saleVM, sale);
            if (saleVM.Photo != null)
            {
                bool exist = _unitOfWork.SaleRepo.ExistsWithImgUrl(saleVM.Photo.FileName, id);
                if (!exist)
                {
                    string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets/img/sale", sale.ImgUrl);
                    DeleteHelper.DeleteFile(path);
                    sale.ImgUrl = saleVM.Photo.SaveImage(_webHostEnvironment, "assets/img/sale");
                }
            }
            
            _unitOfWork.Commit();
            return true;
        }
        public SaleVM MapSale(int id)
        {
            var sale = Get(id).Result;
            if (sale == null) return null;
            SaleVM saleVM = _mapper.Map<SaleVM>(sale);
            saleVM.ImgUrl = sale.ImgUrl;
            return saleVM;
        }
        
    }
}
