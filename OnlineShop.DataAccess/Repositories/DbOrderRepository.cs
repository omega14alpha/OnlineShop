using OnlineShop.DataAccess.Contexts;
using OnlineShop.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.DataAccess.Repositories
{
    public class DbOrderRepository : IRepository
    {
        private DbOrderContext _context;

        public DbOrderRepository(DbOrderContext context)
        {
            _context = context;
        }

        
    }
}
