using ManhwaDimension.Models;
using ManhwaDimension.Repository.Interface;
using ManhwaDimension.Service.Interface;
using ManhwaDimension.ULT;
using ManhwaDimension.Util;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.Formula.Functions;
using System.Globalization;

namespace ManhwaDimension.Repository
{
    public class SubscriptionRepository : ISubscriptionRepository
    {
        private readonly BookwormDbContext db;

        public SubscriptionRepository(BookwormDbContext _db)
        {
            db = _db;
        }

        public async Task<Subscription> Add(Subscription entity)
        {
            if (db != null)
            {
                await db.Set<Subscription>().AddAsync(entity);
                await db.SaveChangesAsync();
                return entity;
            }
            return null;
        }

        public int Count()
        {
            if (db != null)
            {
                return db.Set<Subscription>().Where(x => x.Active).Count();
            }
            return 0;
        }

        public async Task Delete(Subscription entity)
        {
            if (db != null)
            {
                db.Set<Subscription>().Attach(entity);
                db.Entry(entity).Property(x => x.Active).IsModified = true;
                await db.SaveChangesAsync();
            }
        }

        public async Task<int> DeletePermanently(int id)
        {
            var entity = await db.Set<Subscription>().FindAsync(id);
            if (entity == null)
                return 0;

            db.Set<Subscription>().Remove(entity);
            await db.SaveChangesAsync();

            return id;
        }

        public async Task<Subscription> Detail(long id)
        {
            if (db != null)
            {
                return await db.Set<Subscription>().AsNoTracking().FirstOrDefaultAsync(x => x.Active && x.Id == id);
            }
            return null;
        }

        public async Task<List<Subscription>> List()
        {
            if (db != null)
            {
                return await db.Set<Subscription>()
                    .AsNoTracking()
                    .Where(x => x.Active)
                    .OrderByDescending(x => x.Id)
                    .ToListAsync();
            }
            return new List<Subscription>();
        }

        public async Task<List<Subscription>> ListPaging(int pageIndex, int pageSize)
        {
            int offSet = 0;
            offSet = (pageIndex - 1) * pageSize;
            if (db != null)
            {
                return await db.Set<Subscription>()
                    .AsNoTracking()
                    .Where(x => x.Active)
                    .OrderByDescending(x => x.Id)
                    .Skip(offSet).Take(pageSize)
                    .ToListAsync();
            }
            return new List<Subscription>();
        }

        public async Task<DTResult<Subscription>> ListServerSide(SubscriptionDTParameters parameters)
        {
            //0. Options
            string searchAll = parameters.SearchAll.Trim();//Trim text
            string orderCritirea = "Id";//Set default critirea
            int recordTotal, recordFiltered;
            bool orderDirectionASC = true;//Set default ascending
            if (parameters.Order != null)
            {
                orderCritirea = parameters.Columns[parameters.Order[0].Column].Data;
                orderDirectionASC = parameters.Order[0].Dir == DTOrderDir.ASC;
            }
            //1. Join
            var query = from row in db.Subscriptions
                        where row.Active

                        select new
                        {
                            row
                        };

            recordTotal = await query.CountAsync();
            //2. Fillter
            if (!String.IsNullOrEmpty(searchAll))
            {
                searchAll = searchAll.ToLower();
                query = query.Where(c =>
       EF.Functions.Collate(c.row.Type.ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll, SQLParams.Latin_General)));
                //EF.Functions.Collate(c.row.Slug.ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll, SQLParams.Latin_General)));

                // Search Id riêng
                if (int.TryParse(searchAll, out int id))
                {
                    query = query.Where(c => c.row.Id == id);
                }

                if (int.TryParse(searchAll, out int userid))
                {
                    query = query.Where(c => c.row.UserId == userid);
                }
                if (int.TryParse(searchAll, out int comicid))
                {
                    query = query.Where(c => c.row.ComicId == comicid);
                }
                if (int.TryParse(searchAll, out int authorid))
                {
                    query = query.Where(c => c.row.AuthorId == authorid);
                }
                // Search Active riêng
                if (bool.TryParse(searchAll, out bool active))
                {
                    query = query.Where(c => c.row.Active == active);
                }
            }
            foreach (var item in parameters.Columns)
            {
                var fillter = item.Search.Value.Trim();
                if (fillter.Length > 0)
                {
                    switch (item.Data)
                    {
                        case "id":
                            query = query.Where(c => c.row.Id.ToString().Trim().Contains(fillter));
                            break;
                        case "createdTime":
                            if (fillter.Contains(" - "))
                            {
                                var dates = fillter.Split(" - ");
                                var startDate = DateTime.ParseExact(dates[0], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                var endDate = DateTime.ParseExact(dates[1], "dd/MM/yyyy", CultureInfo.InvariantCulture).AddDays(1).AddSeconds(-1);
                                query = query.Where(c => c.row.CreatedAt >= startDate && c.row.CreatedAt <= endDate);
                            }
                            else
                            {
                                var date = DateTime.ParseExact(fillter, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                query = query.Where(c => c.row.CreatedAt.Date == date.Date);
                            }
                            break;
                        case "active":
                            query = query.Where(c => c.row.Active.ToString().Trim().Contains(fillter));
                            break;
                    }
                }
            }

            //3.Query second
            var query2 = query.Select(c => new Subscription()
            {
                Id = c.row.Id,
                UserId = c.row.UserId,
                ComicId = c.row.ComicId,
                AuthorId = c.row.AuthorId,

                Type = c.row.Type,
                CreatedAt = c.row.CreatedAt,
                Active = c.row.Active,


            });
            //4. Sort
            query2 = query2.OrderByDynamic<Subscription>(orderCritirea, orderDirectionASC ? LinqExtensions.Order.Asc : LinqExtensions.Order.Desc);
            recordFiltered = await query2.CountAsync();
            //5. Return data
            return new DTResult<Subscription>()
            {
                data = await query2.Skip(parameters.Start).Take(parameters.Length).ToListAsync(),
                draw = parameters.Draw,
                recordsFiltered = recordFiltered,
                recordsTotal = recordTotal
            };
        }

        public Task<List<Subscription>> Search(string keyword)
        {
            throw new NotImplementedException();
        }

        public Task Update(Subscription entity)
        {
            throw new NotImplementedException();
        }
    }
}
