using System.ComponentModel.DataAnnotations;
using ServiceCore.Domain.Models;

namespace WebApiService.EndPoints.Products
{
    public class ProductInputViewModel : Product
    {
        /// <inheritdoc />
        [Required(ErrorMessage = "Не указан Id продукта")]
        [Range(0, 0, ErrorMessage = "Id нового продукта должно принимать стандартное значение - 0")]
        public override long Id { get; set; }


        public Product GetCopyToInsert()
        {
            return new Product()
            {
                Name = Name,
                Description = Description
            };
        }
    }
}