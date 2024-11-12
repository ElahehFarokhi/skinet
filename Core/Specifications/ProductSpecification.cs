using Core.Entities;

namespace Core.Specifications
{
    public class ProductSpecification : BaseSpecification<Product>
    {
        public ProductSpecification(string? brand, string? type, string? sort)
            : base(x =>
                (string.IsNullOrEmpty(brand) ||
                x.Brand.Equals(brand)) &&
                (string.IsNullOrEmpty(type) ||
                x.Type.Equals(type))
                )
        {
            switch (sort)
            {
                case "PriceAsc": AddOrderBy(x => x.Price);
                    break;
                case "PriceDesc": AddOrderByDesc(x => x.Price);
                    break;
                default:
                    AddOrderBy(x => x.Name);
                    break;
            }

        }
    }
}
