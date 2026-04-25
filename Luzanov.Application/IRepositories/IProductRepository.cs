using Luzanov.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luzanov.Application.IRepositories
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<List<Product>> SearchByNameAsync(string name);
        Task<List<Product>> GetByCategoryIdAsync(int categoryId);
    }
}
