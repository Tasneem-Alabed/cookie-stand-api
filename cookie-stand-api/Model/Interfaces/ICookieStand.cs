using cookie_stand_api.Model.DTO;

namespace cookie_stand_api.Model.Interfaces
{
    public interface ICookieStand
    {
        public Task<CookieStand> AddCookieStand(CookieStandPostDTO cookieStandDTO);

        public Task<List<CookieStandDTO>> GetAllCookieStands();

        public Task<CookieStandDTO> GetCookieStandById(int id);

        public Task<CookieStand> UpdateCookieStand(int id, CookieStandPostDTO cookieStandDTO );

        public Task DeleteCookieStand(int id);

        public Task<List<HourlySales>> GenerateHourlySales(int CookieStandID, int Minimum_Customers_Per_Hour, 
            int Maximum_Customers_Per_Hour, double Average_Cookies_Per_Sale);

        public Task<List<HourlySales>> UpdateGenerateHourlySales(int CookieStandID, int Minimum_Customers_Per_Hour,
           int Maximum_Customers_Per_Hour, double Average_Cookies_Per_Sale);
    }
}
