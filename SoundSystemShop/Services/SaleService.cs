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
        List<Sale> GetAll();
        Sale Get(int id);
        Task Create(SaleVM saleVM);
        Task<bool> Delete(int id);
        bool Update(int id, SaleVM saleVM);
        SaleVM MapSale(int id);
        void SendSaleEmail();
    }
    public class SaleService : ISaleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;
        private readonly IEmailService _emailService;

        public SaleService(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment, IMapper mapper, IFileService fileService, IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
            _mapper = mapper;
            _fileService = fileService;
            _emailService = emailService;
        }
        public List<Sale> GetAll()
        {
            return _unitOfWork.SaleRepo.GetSaleWithIncludes().ToList();
        }
        public Sale Get(int id)
        {
            return _unitOfWork.SaleRepo.GetSaleWithIncludes().FirstOrDefault(s => s.Id == id);
        }
        public async Task Create(SaleVM saleVM)
        {
            if (!saleVM.Photo.CheckFileType())
            {
                throw new ArgumentException("Select an image.");
            }

            Sale sale = _mapper.Map<Sale>(saleVM);
            if(saleVM.ProductIds != null)
            {
                foreach (var item in saleVM.ProductIds)
                {
                    sale.Products.Add(_unitOfWork.ProductRepo.GetByIdAsync(item).Result);
                }
            }
            else
            {
                foreach (var item in _unitOfWork.ProductRepo.GetAllAsync().Result)
                {
                    sale.Products.Add(item);
                }
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
        public bool Update(int id, SaleVM saleVM)
        {
            
            var sale = _unitOfWork.SaleRepo.GetSaleWithIncludes().FirstOrDefault(s => s.Id == id);
            if (sale == null) return false;
            _mapper.Map<SaleVM, Sale>(saleVM, sale);
            sale.Products = new List<Product>();
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
            if (saleVM.ProductIds != null)
            {
                
                foreach (var item in saleVM.ProductIds)
                {
                    sale.Products.Add(_unitOfWork.ProductRepo.GetByIdAsync(item).Result);
                }
            }
            else
            {
                sale.Products = new List<Product>();
                foreach (var item in _unitOfWork.ProductRepo.GetAllAsync().Result)
                {
                    sale.Products.Add(item);
                }
            }

            _unitOfWork.Commit();
            return true;
        }
        public SaleVM MapSale(int id)
        {
            var sale = Get(id);
            if (sale == null) return null;
            SaleVM saleVM = _mapper.Map<SaleVM>(sale);
            saleVM.ImgUrl = sale.ImgUrl;
            return saleVM;
        }
        public void SendSaleEmail()
        {
            var sale = _unitOfWork.SaleRepo.GetSaleWithIncludes().FirstOrDefault(s => s.Name == "NightBargain");
            if (sale == null) return;

            DateTime startTime = sale.StartDate;
            DateTime endTime = sale.FinishDate;
            var users = _unitOfWork.AppUserRepo.GetAllAsync().Result;

            TimeSpan timeRemaining = startTime - DateTime.Now;

            if (timeRemaining <= TimeSpan.FromMinutes(3) && timeRemaining > TimeSpan.FromMinutes(2))
            {
                foreach (var item in users.Where(u => u.UserName != "Admin"))
                {
                    SendSaleEmail(item, sale);
                }
            }
        }
        public void SendSaleEmail(AppUser user, Sale sale)
        {
            user.Location = user.Location.Replace(",", "/");

            // Convert Sale's StartDate from Baku time to Ankara time
            TimeZoneInfo bakuTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Baku"); // Replace with the correct time zone ID for Baku
            TimeZoneInfo userTimeZone = TimeZoneInfo.FindSystemTimeZoneById(user.Location); // Replace with the correct time zone ID for Ankara

            DateTime bakuStartDate = TimeZoneInfo.ConvertTimeFromUtc(sale.StartDate, bakuTimeZone);
            DateTime userStartDate = TimeZoneInfo.ConvertTime(bakuStartDate, userTimeZone);



            EmailMember emailMember = new EmailMember();
            emailMember.email = user.Email;
            emailMember.subject = "Information";
            emailMember.path = "SaleEmail.html";
            emailMember.salePercent = sale.Percent.ToString();
            emailMember.saleDesc = userStartDate.ToString("M");
            emailMember.time = userStartDate.ToString("t");

            _emailService.PrepareEmail(emailMember);
        }


    }
}
