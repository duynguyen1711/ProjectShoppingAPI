using Microsoft.EntityFrameworkCore;
using TrainingBE.Data;
using TrainingBE.Model;

namespace TrainingBE.Repository_Linq
{
    public class OrderItemRepository_Linq : Repository<OrderItem>, IOrderItemRepository_Linq
    {
        public OrderItemRepository_Linq(MyDBContext context) : base(context)
        {
        }
    }
}
