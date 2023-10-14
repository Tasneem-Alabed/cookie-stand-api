using cookie_stand_api.Data;
using cookie_stand_api.Model.DTO;
using cookie_stand_api.Model.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace cookie_stand_api.Model.Services
{
    public class CookieStandService : ICookieStand
    {
        private readonly CookieSalmonDbContext _context;

        public CookieStandService(CookieSalmonDbContext cookieSalmonDbContext )
        {
            _context = cookieSalmonDbContext;
        }

        
        public async Task<CookieStand> AddCookieStand(CookieStandPostDTO cookieStandDTO)
        {
            CookieStand cookieStandToAdd = new CookieStand
            {
                Location = cookieStandDTO.Location,
                Description = cookieStandDTO.Description,
                Minimum_Customers_Per_Hour = cookieStandDTO.Minimum_Customers_Per_Hour,
                Maximum_Customers_Per_Hour = cookieStandDTO.Maximum_Customers_Per_Hour,
                Average_Cookies_Per_Sale = cookieStandDTO.Average_Cookies_Per_Sale,
                Owner = cookieStandDTO.Owner
            };

            await _context.CookieStands.AddAsync(cookieStandToAdd);

            cookieStandToAdd.HourlySales = await GenerateHourlySales(cookieStandToAdd.ID, cookieStandToAdd.Minimum_Customers_Per_Hour, cookieStandToAdd.Maximum_Customers_Per_Hour, cookieStandToAdd.Average_Cookies_Per_Sale);

            await _context.SaveChangesAsync();

            return cookieStandToAdd;
        }

        
        public async Task<List<HourlySales>> GenerateHourlySales(int CookieStandID, int Minimum_Customers_Per_Hour,
            int Maximum_Customers_Per_Hour, double Average_Cookies_Per_Sale)
        {
            List<HourlySales> hourlySalesList = new List<HourlySales>();

            Random random = new Random();

            for (int i = 1; i <= 14; i++)
            {
                int customers = random.Next(Minimum_Customers_Per_Hour, Maximum_Customers_Per_Hour + 1);

                int sales = (int)(customers * Average_Cookies_Per_Sale);

                HourlySales hourlySale = new HourlySales
                {
                    ID = i,
                    CookieStandID = CookieStandID,
                    HourlySale = sales
                };
                await _context.HourlySales.AddAsync(hourlySale);

                hourlySalesList.Add(hourlySale);

            }
            return hourlySalesList;
        }

       
        public async Task DeleteCookieStand(int id)
        {
            var cookieStand = await _context.CookieStands.FindAsync(id);

            if (cookieStand != null)
            {
                var hourlySale = await _context.HourlySales.Where(x => x.CookieStandID == id).ToListAsync();

                foreach (var item in hourlySale)
                {
                    _context.Entry(item).State = EntityState.Deleted;
                }
                _context.Entry(cookieStand).State = EntityState.Deleted;

                await _context.SaveChangesAsync();
            }
            

        }

        public async Task<List<CookieStandDTO>> GetAllCookieStands()
        {
            return await _context.CookieStands.Select(cookieStand => new CookieStandDTO
            {
                ID = cookieStand.ID,
                Location = cookieStand.Location,
                Description = cookieStand.Description,
                HourlySales = cookieStand.HourlySales.Select(x => x.HourlySale).ToList(),
                Minimum_Customers_Per_Hour = cookieStand.Minimum_Customers_Per_Hour,
                Maximum_Customers_Per_Hour = cookieStand.Maximum_Customers_Per_Hour,
                Average_Cookies_Per_Sale = cookieStand.Average_Cookies_Per_Sale,
                Owner = cookieStand.Owner
            }).ToListAsync();
        }

        
       
        public async Task<CookieStandDTO> GetCookieStandById(int id)
        {
            var cookieStand =  await _context.CookieStands.Select(cookieStand => new CookieStandDTO
            {
                ID = cookieStand.ID,
                Location = cookieStand.Location,
                Description = cookieStand.Description,
                HourlySales = cookieStand.HourlySales.Select(x => x.HourlySale).ToList(),
                Minimum_Customers_Per_Hour = cookieStand.Minimum_Customers_Per_Hour,
                Maximum_Customers_Per_Hour = cookieStand.Maximum_Customers_Per_Hour,
                Average_Cookies_Per_Sale = cookieStand.Average_Cookies_Per_Sale,
                Owner = cookieStand.Owner
            }).FirstOrDefaultAsync(x=> x.ID == id);

            return cookieStand;
        }

        public async Task<CookieStand> UpdateCookieStand(int id, CookieStandPostDTO cookieStandDTO)
        {
            var currentCookieStand = await _context.CookieStands.FindAsync(id);

            if (currentCookieStand != null)
            {

                currentCookieStand.Location = cookieStandDTO.Location;
                currentCookieStand.Description = cookieStandDTO.Description;
                currentCookieStand.Minimum_Customers_Per_Hour = cookieStandDTO.Minimum_Customers_Per_Hour;
                currentCookieStand.Maximum_Customers_Per_Hour = cookieStandDTO.Maximum_Customers_Per_Hour;
                currentCookieStand.Average_Cookies_Per_Sale = cookieStandDTO.Average_Cookies_Per_Sale;

                currentCookieStand.HourlySales = await UpdateGenerateHourlySales(id, currentCookieStand.Minimum_Customers_Per_Hour,
                    currentCookieStand.Maximum_Customers_Per_Hour, currentCookieStand.Average_Cookies_Per_Sale);

                _context.Entry(currentCookieStand).State = EntityState.Modified;

                await _context.SaveChangesAsync();
                return currentCookieStand;
            }
            else
                return null;
            
        }
        public async Task<List<HourlySales>> UpdateGenerateHourlySales(int CookieStandID, int Minimum_Customers_Per_Hour,
           int Maximum_Customers_Per_Hour, double Average_Cookies_Per_Sale)
        {
            List<HourlySales> hourlySalesList = new List<HourlySales>();

            Random random = new Random();

            for (int i = 1; i <= 14; i++)
            {
                int customers = random.Next(Minimum_Customers_Per_Hour, Maximum_Customers_Per_Hour + 1);

                int sales = (int)(customers * Average_Cookies_Per_Sale);

                HourlySales hourlySale = new HourlySales
                {
                    ID = i,
                    CookieStandID = CookieStandID,
                    HourlySale = sales
                };

                _context.Entry(hourlySale).State = EntityState.Modified;

                await _context.SaveChangesAsync();
                
                hourlySalesList.Add(hourlySale);

            }
            return hourlySalesList;
        }
    }
}
