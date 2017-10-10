using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace MyPro.Models
{
    public class Per
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
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{yyyy/mm/dd}")]
        public string TSHD { get; set; }

        [Display(Name = "تاریخ پایان دوره")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{yyyy/mm/dd}")]
        public string TPD { get; set; }

        [Display(Name = "روز و ساعت برگذاری دوره")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{yyyy/mm/dd 00:00}")]
        public string RVSBD { get; set; }

        [Display(Name = "تاریخ آزمون")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{yyyy/mm/dd 00:00}")]
        public string TA { get; set; }

        [Display(Name = "تعداد فراگیر")]
        public int TF { get; set; }

        [Display(Name = "درآمد گواهینامه")]
        public int DG { get; set; }
    }
}