using ManhwaDimension.Models;
using ManhwaDimension.Repository.Interface;
using ManhwaDimension.ULT;
using ManhwaDimension.Util;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace ManhwaDimension.Repository
{
    public class ChapterRepository : BaseRepository<Chapter>, IChapterRepository
    {
        public ChapterRepository(BookwormDbContext _db) : base(_db)
        {

        }

        public async Task<DTResult<Chapter>> ListServerSide(ChapterDTParameters parameters)
        {
            //0. Options
            string searchAll = parameters.SearchAll.Trim();
            string orderCritirea = "Id";
            int recordTotal, recordFiltered;
            bool orderDirectionASC = true;
            if (parameters.Order != null)
            {
                orderCritirea = parameters.Columns[parameters.Order[0].Column].Data;
                orderDirectionASC = parameters.Order[0].Dir == DTOrderDir.ASC;
            }
            //1. Join
            var query = from row in db.Chapters
                        select new
                        {
                            row
                        };

            recordTotal = await query.CountAsync();
            //2. Filter
            if (!String.IsNullOrEmpty(searchAll))
            {
                searchAll = searchAll.ToLower();
                query = query.Where(c =>
                    (c.row.Title != null && EF.Functions.Collate(c.row.Title.ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll, SQLParams.Latin_General))) ||
                    c.row.ChapterNumber.ToString().Contains(searchAll)
                );
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
                        case "title":
                            query = query.Where(c => c.row.Title != null && c.row.Title.Trim().Contains(fillter));
                            break;
                        case "chapterNumber":
                            query = query.Where(c => c.row.ChapterNumber.ToString().Trim().Contains(fillter));
                            break;
                        case "comicId":
                            if (int.TryParse(fillter, out int comicId))
                            {
                                query = query.Where(c => c.row.ComicId == comicId);
                            }
                            break;
                        case "createdAt":
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
                    }
                }
            }

            //3. Query second
            var query2 = query.Select(c => new Chapter()
            {
                Id = c.row.Id,
                ComicId = c.row.ComicId,
                Title = c.row.Title,
                ChapterNumber = c.row.ChapterNumber,
                VolumeNumber = c.row.VolumeNumber,
                IsVisible = c.row.IsVisible,
                CreatedAt = c.row.CreatedAt,
            });
            //4. Sort
            query2 = query2.OrderByDynamic<Chapter>(orderCritirea, orderDirectionASC ? LinqExtensions.Order.Asc : LinqExtensions.Order.Desc);
            recordFiltered = await query2.CountAsync();
            //5. Return data
            return new DTResult<Chapter>()
            {
                data = await query2.Skip(parameters.Start).Take(parameters.Length).ToListAsync(),
                draw = parameters.Draw,
                recordsFiltered = recordFiltered,
                recordsTotal = recordTotal
            };
        }
    }
}
