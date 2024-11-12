using Core.Entities;
using System.Linq;

namespace Core.Specifications
{
    public class ProductSpecification : BaseSpecification<Product>
    {
        public ProductSpecification(ProductSpecParams specParams)
            : base(x =>
                (specParams.Brands.Count == 0 || specParams.Brands.Contains(x.Brand)) &&
            
            (specParams.Types.Count == 0 || specParams.Types.Contains(x.Type)))
        {
            ApplyPaging(specParams.PageSize * (specParams.PageIndex - 1), specParams.PageSize);

            switch (specParams.Sort)
            {
                case "PriceAsc":
                    AddOrderBy(x => x.Price);
                    break;
                case "PriceDesc":
                    AddOrderByDesc(x => x.Price);
                    break;
                default:
                    AddOrderBy(x => x.Name);
                    break;
            }

        }
    }
}
