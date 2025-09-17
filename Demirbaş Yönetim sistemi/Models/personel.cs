using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Demirbaş_Yönetim_sistemi.Models
{
    public class personel
    {
        public string  soyad { get; set; }
        public string ad { get; set; }
        public int durum { get; set; }

        public int departman_Id { get; set; }
        public DateTime ise_baslangic { get; set; }
        public DateTime isten_cikis { get; set; }
        public string departman { get; set; }
        public int? id { get; set; }
        public string kayit_kullanici_id { get; set; }
        public DateTime kayit_tarihi {  get; set; }
    }

    
  
}