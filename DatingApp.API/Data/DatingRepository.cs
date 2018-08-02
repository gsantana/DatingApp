using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class DatingRepository : IDatingRepository
    {
        public DataContext DataContext { get; }

        public DatingRepository(DataContext dataContext)
        {
            DataContext = dataContext;
        }

        public void Add<T>(T entity) where T : class
        {
            DataContext.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            DataContext.Remove(entity);
        }

        public async Task<User> GetUser(int id)
        {
            return await DataContext.Users.Include(p => p.Photos).FirstOrDefaultAsync(u => u.Id == id);
        }

        public  async Task<IEnumerable<User>> GetUsers()
        {
            return await DataContext.Users.Include(p => p.Photos).ToListAsync();
        }

        public async Task<bool> SaveAll()
        {
            return await DataContext.SaveChangesAsync() > 0;
        }

        public async Task<Photo> GetPhoto(int id)
        {
            return await DataContext.Photos.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Photo> GetMainPhotoForUser(int userId)
        {
            return await DataContext.Photos.Where(u => u.UserId == userId)
                .FirstOrDefaultAsync(p => p.IsMain);
        }

    }
}
