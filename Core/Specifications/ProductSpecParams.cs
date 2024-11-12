
using System.Collections.Generic;
using System.Linq;

namespace Core.Specifications
{
    public class ProductSpecParams
    {

        private const int MaxPageSize = 50;
        public int PageIndex { get; set; } = 1;
        private int _pageSize = 6;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize ? MaxPageSize: value);
        }



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

        public string? Sort { get; set; }

    }
}
