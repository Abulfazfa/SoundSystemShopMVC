using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SoundSystemShop.DAL;
using SoundSystemShop.Services.Interfaces;
namespace SoundSystemShop.Services
{
    public class PromoEmailSenderHostedService : IHostedService, IDisposable
    {
        private Timer _promoTimer;
        private Timer _saleTimer;
        private readonly IServiceProvider _serviceProvider;

        public PromoEmailSenderHostedService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _promoTimer = new Timer(DoWorkPromo, null, TimeSpan.Zero, TimeSpan.FromDays(7));
            _saleTimer = new Timer(DoWorkSale, null, TimeSpan.Zero, TimeSpan.FromSeconds(20));

            return Task.CompletedTask;
        }

        private async void DoWorkPromo(object state)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var promoService = scope.ServiceProvider.GetRequiredService<PromoService>();
               // promoService.GenerateLuckyPeopleAsync();
            }
        }

        private async void DoWorkSale(object state)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var saleService = scope.ServiceProvider.GetRequiredService<ISaleService>();
                //saleService.SendSaleEmail();
            }
            
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _promoTimer?.Change(Timeout.Infinite, 0);
            _saleTimer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _promoTimer?.Dispose();
            _saleTimer?.Dispose();
        }
    }


}