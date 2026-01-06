using ManhwaDimension.Models;
using ManhwaDimension.Repository.Interface;
using ManhwaDimension.ULT.Entities;
using ManhwaDimension.Util.Entities;
using Microsoft.EntityFrameworkCore;

namespace ManhwaDimension.Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class, IEntityBase
    {
        protected readonly BookwormDbContext db;
        public BaseRepository(BookwormDbContext _db)
        {
            db = _db;
        }
        public async Task<T> Add(T obj)
        {
            if (db != null)
            {
                await db.Set<T>().AddAsync(obj);
                await db.SaveChangesAsync();
                return obj;
            }
            return null;
        }

        public int Count()
        {
            if (db != null)
            {
                return db.Set<T>().Where(x => x.Active).Count();
            }
            return 0;
        }

        public async Task Delete(T obj)
        {
            if (db != null)
            {
                db.Set<T>().Attach(obj);
                db.Entry(obj).Property(x => x.Active).IsModified = true;
                await db.SaveChangesAsync();
            }
        }

        public Task<long> DeletePermanently(long id)
        {
            throw new NotImplementedException();
        }

        public async Task<T> Detail(long id)
        {
            if (db != null)
            {
               return await db.Set<T>().AsNoTracking().FirstOrDefaultAsync(x => x.Active && x.Id == id);
            }
            return null;
        }

        public async Task<List<T>> List()
        {
            if (db != null)
            {
                return await db.Set<T>()
                    .AsNoTracking()
                    .Where(x => x.Active)
                    .OrderByDescending(x => x.Id)
                    .ToListAsync();
            }
            return new List<T>();
        }

        public async Task<List<T>> ListPaging(int pageIndex, int pageSize)
        {
            int offSet = 0;
            offSet = (pageIndex - 1) * pageSize;
            if (db != null)
            {
                return await db.Set<T>()
                    .AsNoTracking()
                    .Where(x => x.Active)
                    .OrderByDescending(x => x.Id)
                    .Skip(offSet).Take(pageSize)
                    .ToListAsync();
            }
            return new List<T>();
        }

        //public async Task<List<T>> Search(string keyword)
        //{
        //    if (db != null)
        //    {
        //        return await db.Set<T>()
        //                       .Where(x => x.ToString().ToLower().Trim().Contains(keyword.ToLower().Trim()) && x.Active)
        //                       .ToListAsync();
        //    }
        //    return new List<T>();
        //}
        public async Task<List<T>> Search(string keyword)
        {
            if (db != null)
            {
                var lower = keyword.ToLower().Trim();

                return await db.Set<T>()
                               .Where(x => x.Active &&
                                           x.Name.ToLower().Contains(lower))
                               .ToListAsync();
            }
            return new List<T>();
        }


        public async Task Update(T obj)
        {
            if (db != null)
            {
                var existingAccount = await db.Set<T>().FindAsync(obj.Id);
                if (existingAccount == null) return;

                var activeValue = typeof(T).GetProperty("Active")?.GetValue(existingAccount);

                var entry = db.Entry(existingAccount);

                foreach (var property in typeof(T).GetProperties())
                {
                    if (property.Name == "Id") continue; // Bỏ qua Id, không cập nhật

                    var newValue = property.GetValue(obj);
                    if (newValue != null && newValue.ToString() != "0")
                    {
                        property.SetValue(existingAccount, newValue);
                    }
                }

                if (activeValue != null)
                {
                    typeof(T).GetProperty("Active")?.SetValue(existingAccount, activeValue);
                    entry.Property("Active").IsModified = false; // Giữ nguyên giá trị cũ
                }

                await db.SaveChangesAsync();
            }
        }
    }
}
