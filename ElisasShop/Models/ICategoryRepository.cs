using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElisasShop.Models
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> Categories { get; }
    }
}
