using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceCore.DataAccess.SettingsEF;
using ServiceCore.Domain.Models;
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

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> Get(long id)
        {
            using (var dbContext = _dbContextFactory.GetApplicationContext())
            {
                var product = await dbContext
                    .Set<Product>()
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (product == null)
                    return NotFound();
                return product;
            }
        }

       
        [HttpPost]
        [Authorize, UserOddId]
        [ProductValidationFilter]
        public async Task<ActionResult<Product>> Post([FromBody]ProductInputViewModel inputProductViewModel)
        {
            using (var dbContext = _dbContextFactory.GetApplicationContext())
            {
                var newProduct = inputProductViewModel.GetCopyToInsert();
                await dbContext
                    .Set<Product>()
                    .AddAsync(newProduct);
                dbContext.Commit();

                return newProduct;
            }

            this.ValidationProblem();
        }
    }
}