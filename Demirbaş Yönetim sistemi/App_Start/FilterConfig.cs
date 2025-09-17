using System.Web;
using System.Web.Mvc;

namespace Demirbaş_Yönetim_sistemi
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
