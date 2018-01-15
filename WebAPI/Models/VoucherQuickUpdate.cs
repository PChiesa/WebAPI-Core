using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Enums;

namespace WebAPI.Models
{
    public class VoucherQuickUpdate
    {
        public int Id { get; set; }
        public VoucherStatus CurrentStatus { get; set; }
        public DateTime? EntryDate { get; set; }
        public string Gate { get; set; }
    }
}
