using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElisasShop.Models
{
    public class CategoryRepository: ICategoryRepository
    {
        private readonly AppDbContext _appDbContext;

        // AppDbContext added to our ConfigureServices method from Startup.cs, we can use constructor injection
        public CategoryRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IEnumerable<Category> Categories => _appDbContext.Categories;
    }
}
