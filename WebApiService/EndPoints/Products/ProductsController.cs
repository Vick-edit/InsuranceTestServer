using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceCore.DataAccess.SettingsEF;
using ServiceCore.Domain.Models;
using WebApiService.Authorization;
using WebApiService.Filters;

namespace WebApiService.EndPoints.Products
{
    /// <summary>
    ///     Контроллер доступа к данным продуктов
    /// </summary>
    [Route("{controller}")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IDbContextFactory _dbContextFactory;


        public ProductsController(IDbContextFactory dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        [Authorize, UserOddId]
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> Get(long id)
        {
            using (var dbContext = _dbContextFactory.GetApplicationContext())
            {
                var product = await dbContext
                    .Set<Product>()
                    .FirstOrDefaultAsync(p => p.Id == id);

                return product;
            }
        }
    }
}