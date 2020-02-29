using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceCore.DataAccess.SettingsEF;
using ServiceCore.Domain.Models;

namespace WebApiService.EndPoints.Products
{
    /// <summary>
    ///     Контроллер доступа к данным продуктов
    /// </summary>
    [Route("{controller}/{id?}")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IDbContextFactory _dbContextFactory;


        public ProductsController(IDbContextFactory dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }


        [HttpGet]
        public async Task<IActionResult> Get(long id)
        {
            using (var dbContext = _dbContextFactory.GetApplicationContext())
            {
                var product = await dbContext
                    .Set<Product>()
                    .FirstOrDefaultAsync(p => p.Id == id);

                return Ok(product);
            }
        }
    }
}