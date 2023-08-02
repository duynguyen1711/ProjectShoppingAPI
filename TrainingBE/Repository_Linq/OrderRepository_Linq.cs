using Microsoft.EntityFrameworkCore;
using TrainingBE.Data;
using TrainingBE.Model;

namespace TrainingBE.Repository_Linq
{
    public class OrderRepository_Linq : Repository<Order>, IOrderRepository_Linq
    {
        public OrderRepository_Linq(MyDBContext context) : base(context) { 
        }
    
    
    }
}
