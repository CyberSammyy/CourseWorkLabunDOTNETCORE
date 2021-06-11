using System;
using System.Collections.Generic;
using System.Text;

namespace CourseWorkLabunDOTNETCORE
{
    static class HistoryViewerContent
    {
        public static MainPage Instance { get; set; }
        public static string SenderWalletNumber { get; set; }
        public static List<string> history = null;
        public static List<string> historyAll = null;
        public static List<string> filtered = null;
        public static List<string> filteredAll = null;
        public static List<Category> CategoriesAsSearchOption = Categories.GetCategories();
        public static (MainPage, string) historyDefaultInitializer;
        /// <summary>
        /// CASO - Categories As Search Option
        /// </summary>
        public static void UpdateCASO()
        {
            CategoriesAsSearchOption = Categories.GetCategories();
        }
    }
}
