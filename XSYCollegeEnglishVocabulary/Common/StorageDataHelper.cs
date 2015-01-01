using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace XSYCollegeEnglishVocabulary.Common
{
    class StorageDataHelper
    {
        // public const string RootFolder = "WP.CE";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="unitId">1_1</param>
        /// <returns></returns>
        public static async Task<string> GetUnit(string unitId, string bookText)
        {
            var book_unit_ids = unitId.Split('_');
            var bookId = book_unit_ids[0];

            var subfolder = Constants.DATA_BASE_PATH + ("horizonread" + bookId);
            var unitName = GetUnitFolder(book_unit_ids[1]);
            var subsubfolder = subfolder + "/" + unitName;

            var file = subsubfolder + "/" + (GetUnitJsonFileFolder(unitName, bookText));
            StorageFile sourceFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri(file.Replace("\\", "/")));
            string result = await FileIO.ReadTextAsync(sourceFile);
            return result;
        }

        private static string GetUnitFolder(string unitId)
        {
            return "UNIT" + unitId.PadLeft(2, '0');
        }

        private static string GetUnitJsonFileFolder(string unitName, string bookText)
        {
            return unitName + "_" + bookText + ".json";
        }

        //E:\collegeEnglish\integrated1\unitlist
        public static async Task<string> GetUnitlist(string bookId)
        {
            var basefoler = Constants.DATA_BASE_PATH;
            var file = basefoler + ("integrated" + bookId + "/unitlist/UnitList.json");

            StorageFile sourceFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri(file.Replace("\\", "/")));
            string result = await FileIO.ReadTextAsync(sourceFile);
            return result;
        }
    }
}
