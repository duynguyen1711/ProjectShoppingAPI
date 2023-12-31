﻿using Microsoft.EntityFrameworkCore;
using TrainingBE.Data;
using TrainingBE.Model;
using TrainingBE.Repository;

namespace TrainingBE.Repository_Linq
{
    public class ProductRepository_Linq : Repository<Product>, IProductRepository_Linq
    {
        public ProductRepository_Linq(MyDBContext context) : base(context)
        {
        }
        public Product GetProductByProductName(string productName)
        {
            return _dbSet.FirstOrDefault(p => p.Name == productName);
        }
        public IEnumerable<Product> GetAllProductsIncludingCategory()
        {
            return _dbSet.Include(p => p.Category).ToList();
        }

        public List<Product> GetProductsByCategoryId(int categoryId)
        {
            return _dbSet.Where(p => p.CategoryID == categoryId).ToList();
        }
        public IQueryable<Product> GetAllWithDiscounts()
        {
            return _dbSet.Include(p => p.Product_Discounts);
        }
    }
}
