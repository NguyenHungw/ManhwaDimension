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
                // Nếu entity có Active → đếm theo Active, không thì đếm tất cả
                if (typeof(IHasActive).IsAssignableFrom(typeof(T)))
                {
                    return db.Set<T>().Cast<IHasActive>().Count(x => x.Active);
                }
                return db.Set<T>().Count();
            }
            return 0;
        }

        public async Task Delete(T obj)
        {
            if (db != null)
            {
                // Nếu entity có Active → soft delete, không thì hard delete
                if (obj is IHasActive)
                {
                    db.Set<T>().Attach(obj);
                    ((IHasActive)obj).Active = false;
                    db.Entry(obj).Property("Active").IsModified = true;
                    await db.SaveChangesAsync();
                }
                else
                {
                    db.Set<T>().Remove(obj);
                    await db.SaveChangesAsync();
                }
            }
        }

        public async Task<int> DeletePermanently(int id)
        {
            var entity = await db.Set<T>().FindAsync(id);
            if (entity == null)
                return 0;

            db.Set<T>().Remove(entity);
            await db.SaveChangesAsync();

            return id;
        }

        public async Task<T> Detail(long id)
        {
            if (db != null)
            {
                return await db.Set<T>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            }
            return null;
        }

        public async Task<List<T>> List()
        {
            if (db != null)
            {
                IQueryable<T> query = db.Set<T>().AsNoTracking();

                // Filter Active nếu entity có
                if (typeof(IHasActive).IsAssignableFrom(typeof(T)))
                {
                    query = query.Where(x => ((IHasActive)x).Active);
                }

                return await query.OrderByDescending(x => x.Id).ToListAsync();
            }
            return new List<T>();
        }

        public async Task<List<T>> ListPaging(int pageIndex, int pageSize)
        {
            int offSet = 0;
            offSet = (pageIndex - 1) * pageSize;
            if (db != null)
            {
                IQueryable<T> query = db.Set<T>().AsNoTracking();

                if (typeof(IHasActive).IsAssignableFrom(typeof(T)))
                {
                    query = query.Where(x => ((IHasActive)x).Active);
                }

                return await query
                    .OrderByDescending(x => x.Id)
                    .Skip(offSet).Take(pageSize)
                    .ToListAsync();
            }
            return new List<T>();
        }

        public async Task<List<T>> Search(string keyword)
        {
            if (db != null)
            {
                var lower = keyword.ToLower().Trim();
                IQueryable<T> query = db.Set<T>();

                // Chỉ search theo Name nếu entity có Name
                if (typeof(IHasName).IsAssignableFrom(typeof(T)))
                {
                    query = query.Where(x => ((IHasName)x).Name.ToLower().Contains(lower));
                }

                // Filter Active nếu có
                if (typeof(IHasActive).IsAssignableFrom(typeof(T)))
                {
                    query = query.Where(x => ((IHasActive)x).Active);
                }

                return await query.ToListAsync();
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
