using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Demirbaş_Yönetim_sistemi.Models
{
    public class ustune_zimmet
    {

        public string demirbas_id { get; set; }
        public string personel_id { get; set; }
       

        public DateTime zimmet_baslangic { get; set; }
        public DateTime zimmet_bitis { get; set; }
     
        public string id { get; set; }
        public string kayit_kullanici_id { get; set; }
        public DateTime kayit_tarihi { get; set; }
        
        public int durum { get; set; }

        public personel Personel { get; set; }
        public demirbas Demirbas { get; set; }
        



       
    }
 
    
}