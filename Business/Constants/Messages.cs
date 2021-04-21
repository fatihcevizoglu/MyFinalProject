using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Constants
{
    public static class Messages
    {
        public static string ProductAdded = "Ürün eklendi";
        public static string ProductNameInvalid = "Ürün İsmi Geçersizdir";
        public static string MaintenanceTime = "Sistem Bakımda";
        public static string ProductListed = "Ürün Listelendi";
        public static string ProductCountCategoryError = "bir kadegoryde en fazla 10 ürün olabilir";
        public static string ProductNameAlreadyExists = "Bu isimde zaten başka bir ürün var";
        public static string CategoryLimitExceded = "Kategory Limiti Aşıldığı için sisteme eklenemiyor";
    }
}
