using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElisasShop.Models
{
    // We will use a Mock repository for Category (not using the Db yet)
    public class MockCategoryRepository : ICategoryRepository
    {
        public IEnumerable<Category> Categories
        {
            get
            {
                // Returns a List of 3 categories
                return new List<Category>
                {
                    new Category
                    {
                        CategoryId = 1,
                        CategoryName = "Fruit pies",
                        Description = "All-fruity pies"
                    },
                    new Category
                    {
                        CategoryId = 1,
                        CategoryName = "Cheese cakes",
                        Description = "Cheesy all the way"
                    },
                    new Category
                    {
                        CategoryId = 1,
                        CategoryName = "Seasonal pies",
                        Description = "Get in the mood for a seasonal pie"
                    }
                };
            }
        }
    }
}
