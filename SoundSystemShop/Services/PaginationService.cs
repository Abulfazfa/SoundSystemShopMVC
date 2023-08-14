namespace SoundSystemShop.Services
{
    public class PaginationService
    {
        public int PageCount(int productCount, int take)
        {
            return (int)Math.Ceiling((decimal)productCount / take);
        }
    }
}
