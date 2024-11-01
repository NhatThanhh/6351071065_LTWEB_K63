using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TH_Project.Data;

namespace TH_Project.ViewModel
{
    public class ProductVM
    {
        public IEnumerable<SACH> SACHes { get; set; }
        public IEnumerable<CHUDE> cHUDEs { get; set; }
        public IEnumerable<NHAXUATBAN> nxb { get; set; }
    }
}