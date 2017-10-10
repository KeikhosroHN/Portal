using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyPro.ViewModels
{
    public class PerModel
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "نام منطقه")]
        public string RegionName { get; set; }

        [Display(Name = "عنوان دوره")]
        public string OD { get; set; }

        [Display(Name = "نام مربی دوره")]
        public string NMD { get; set; }

        [Display(Name = "مکان برگذاری دوره")]
        public string MBD { get; set; }

        [Display(Name = "تاریخ شروع دوره")]
        public string TSHD { get; set; }

        [Display(Name = "تاریخ پایان دوره")]
        public string TPD { get; set; }

        [Display(Name = "روز و ساعت برگذاری دوره")]
        public string RVSBD { get; set; }

        [Display(Name = "تاریخ آزمون")]
        public string TA { get; set; }

        [Display(Name = "تعداد فراگیر")]
        public int TF { get; set; }

        [Display(Name = "درآمد گواهینامه")]
        public int DG { get; set; }
    }
}