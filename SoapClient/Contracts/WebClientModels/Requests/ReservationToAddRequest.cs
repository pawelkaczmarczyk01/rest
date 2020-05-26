using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.WebClientModels.Requests
{
    public class ReservationToAddRequest
    {
        public int roomId { get; set; }
        public DateTime reservationFrom { get; set; }
        public DateTime reservationTo { get; set; }
        public int userId { get; set; }
    }
}
