using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;
using ServiceCore.Domain.Models;

namespace WebApiService.EndPoints.Products
{
    /// <summary>
    ///     Кастомный форматер для продукта в текстовом представлении
    /// </summary>
    internal class ProductTextFormatter : TextInputFormatter
    {
        private const char FIELD_SEPARATOR = '~';
        private const char VALUE_SEPARATOR = '=';


        public ProductTextFormatter()
        {
            SupportedMediaTypes.Add("text/product");
            SupportedEncodings.Add(Encoding.UTF8);
        }


        /// <inheritdoc />
        protected override bool CanReadType(Type type)
        {
            return type == typeof(ProductInputViewModel);
        }


        /// <inheritdoc />
        public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context, Encoding encoding)
        {
            string productBody = null;
            using (var streamReader = context.ReaderFactory(context.HttpContext.Request.Body, encoding))
                productBody = await streamReader.ReadToEndAsync();

            if (string.IsNullOrWhiteSpace(productBody))
                return InputFormatterResult.NoValue();

            var productBodyParts = productBody                   //productBody должен выглядеть, как "Name=Имя~Description=Описание"
                .Split(FIELD_SEPARATOR, StringSplitOptions.RemoveEmptyEntries)          //Разбиваем на описание полей new[] {"Name=Страховой коробочный продукт", "Description=Описание"}
                .Select(bp => bp.Split(VALUE_SEPARATOR).ToList())                 //Далее получаем коллекцию колекций, new[] {new[] {"Name","Страховой коробочный продукт"}, new[]{"Description","Описание"}}
                .Select(x => x.Append(null).ToList())                        //Добавляем во внутреннии коллекции пустой элемент, чтобы корректно обработать пустые описания
                .ToDictionary(x => x[0], x => x[1]);              //Собираем всё в словарь имя поля => значение

            var productInfo = new ProductInputViewModel();
            if (productBodyParts.ContainsKey(nameof(Product.Name)))
                productInfo.Name = productBodyParts[nameof(Product.Name)];

            if (productBodyParts.ContainsKey(nameof(Product.Description)))
                productInfo.Description = productBodyParts[nameof(Product.Description)];

            return InputFormatterResult.Success(productInfo);
        }
    }
}