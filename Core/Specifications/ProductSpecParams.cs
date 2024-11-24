
using System.Collections.Generic;
using System.Linq;

namespace Core.Specifications
{
    public class ProductSpecParams : PagingParams
    {
        private List<string> _brands = [];

		public List<string> Brands // brnad=boards,glass
		{
			get => _brands;
			set 
			{
				_brands = value.SelectMany(x=>x.Split(',',
                    System.StringSplitOptions.RemoveEmptyEntries)).ToList();
			}
		}

        private List<string> _types = [];

        public List<string> Types // types=boards,glass
        {
            get => _types;
            set
            {
                _types = value.SelectMany(x => x.Split(',',
                    System.StringSplitOptions.RemoveEmptyEntries)).ToList();
            }
        }

        private string? _search;

        public string Search
        {
            get => _search ?? "";
            set 
            {
                _search = value.ToLower(); 
            }
        }


        public string? Sort { get; set; }

    }
}
