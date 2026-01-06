using ManhwaDimension.Models;
using ManhwaDimension.Models.Response;
using ManhwaDimension.Service.Interface;
using ManhwaDimension.ULT;
using Microsoft.AspNetCore.Mvc;

namespace ManhwaDimension.Controllers.ManhwaDimension
{
    public class GenreController : Controller
    {
        IGenreService service;
        public GenreController(IGenreService _service)
        {
            service = _service;
        }

        [HttpGet]
        [Route("admin/List")]
        public async Task<IActionResult> AdminListServerSide()
        {
            return View();
        }

        [HttpGet]
        [Route("api/List")]
        public async Task<IActionResult> List()
        {
            try
            {
                var dataList = await service.List();
                if (dataList == null || dataList.Count == 0)
                {
                    return NotFound();
                }
                var geneStoryResponse = ManhwaDimensionResponse.SUCCESS(dataList.Cast<object>().ToList());
                return Ok(geneStoryResponse);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpGet]
        [Route("api/Detail/{Id}")]
        public async Task<IActionResult> Detail(long Id)
        {
            if (Id == null || Id ==0)
            {
                return BadRequest();
            }
            try
            {
                var dataList = await service.Detail(Id);
                
                if (dataList == null)
                {
                    return NotFound();
                }
                var geneStoryResponse = ManhwaDimensionResponse.SUCCESS(dataList);
                return Ok(geneStoryResponse);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpGet]
        [Route("api/Search")]
        public async Task<IActionResult> Search(string keyword)
        {
            try
            {
                var dataList = await service.Search(keyword);
                if (dataList == null || dataList.Count == 0)
                {
                    return NotFound();
                }
                var geneStoryResponse = ManhwaDimensionResponse.SUCCESS(dataList.Cast<object>().ToList());
                return Ok(geneStoryResponse);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }


        [HttpGet]
        [Route("api/ListPaging")]
        public async Task<IActionResult> ListPaging(int pageIndex, int pageSize)
        {
            if (pageIndex < 0 || pageSize < 0) return BadRequest();
            try
            {
                var dataList = await service.ListPaging(pageIndex, pageSize);

                if (dataList == null || dataList.Count == 0)
                {
                    return NotFound();
                }

                var geneStoryResponse = ManhwaDimensionResponse.SUCCESS(dataList.Cast<object>().ToList());
                return Ok(geneStoryResponse);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpPost]
        [Route("api/add")]
        public async Task<IActionResult> Add([FromBody] Genre model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await service.Add(model);
                    var manhwaDimensionResponse = ManhwaDimensionResponse.CREATED(model);
                    return Created("", manhwaDimensionResponse);
                }
                catch (Exception ex)
                {

                    return BadRequest(ex.Message);
                }
            }
            return BadRequest();
        }
        [HttpPut]
        [Route("api/update")]
        public async Task<IActionResult> Update([FromBody] Genre model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await service.Update(model);
                    var manhwaDimensionResponse = ManhwaDimensionResponse.CREATED(model);
                    return Ok(manhwaDimensionResponse);
                }
                catch (Exception ex)
                {
                    if (ex.GetType().FullName == "Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException")
                    {
                        return NotFound();
                    }
                    return BadRequest();

                }
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("api/ListServerSide")]
        public async Task<IActionResult> ListServerSide([FromBody] GenreDTParameters parameters)
        {
            try
            {
                var data = await service.ListServerSide(parameters);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
