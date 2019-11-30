using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spice.Models.Viewmodel
{
    public class OrderDetailsConfirm_ViewModel
    {
        public OrderHeaders orderHeaders { get; set; }
        public List<OrderDatails> orderDatails { get; set; }
    }
}
