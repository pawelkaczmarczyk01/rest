using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.WebClientModels.Responses
{
    public class AssortmentResponse
    {
        public int id { get; set; }
        public bool roomTv { get; set; }
        public bool roomBathroom { get; set; }
        public bool roomDesk { get; set; }
        public bool roomSafe { get; set; }
        public bool roomFridge { get; set; }
    }
}
