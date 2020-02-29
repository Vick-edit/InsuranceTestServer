using ServiceCore.Domain.Models;

namespace ServiceCore
{
    /// <summary>
    ///     Набор тестовых данных
    /// </summary>
    internal static class SeedData
    {
        public static Product[] TestProducts => new []
        {
            new Product
            {
                Name = "Страховой коробочный продукт",
                Description = "Продукт для продаж через клиентские центры"
            },    
            
            new Product
            {
                Name = "ОСАГО",
                Description = "Продукт КАСКО"
            },   
            
            new Product
            {
                Name = "ЕОСАГО",
                Description = "Электронный полис ОСАГО"
            },
        };
    }
}
