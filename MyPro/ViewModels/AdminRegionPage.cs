using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyPro.ViewModels
{
    public class AdminRegionPage
    {
        [Display(Name = "نام منطقه")]
        public string RegionName { get; set; }

        [Display(Name = "عنوان دوره")]
        public string OD { get; set; }

        [Display(Name = "نام مربی دوره")]
        public string NMD { get; set; }

        [Display(Name = "مکان برگذاری دوره")]
        public string MBD { get; set; }

        [Display(Name = "تاریخ شروع دوره")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{yyyy/mm/dd 00:00}")]
        public DateTime TSHD { get; set; }

        [Display(Name = "تاریخ پایان دوره")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{yyyy/mm/dd 00:00}")]
        public DateTime TPD { get; set; }

        [Display(Name = "روز و ساعت برگذاری دوره")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{yyyy/mm/dd 00:00}")]
        public DateTime RVSBD { get; set; }

        [Display(Name = "تاریخ آزمون")]
        public DateTime TA { get; set; }

        [Display(Name = "تعداد فراگیر")]
        public int TF { get; set; }

        [Display(Name = "درآمد گواهینامه")]
        public int DG { get; set; }


        public List<PerModel> PerModels { get; set; }
    }
}