using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spice.Models.Viewmodel
{
    public class OrderDetails_ViewModel
    {
        public List<ShopingCart> Listcart { get; set; }
        public OrderHeaders OrderHeaders { get; set; }
    }
}
 