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

        public static async Task<string> GetCourse(string courseId)
        {
            var bookId = courseId.Substring(0, 1);

            var subfolder = Constants.DATA_BASE_PATH + ("integrated" + bookId);
            var subsubfolder = subfolder + "/" + (GetUnitFolder(courseId));

            var file = subsubfolder + "/" + (GetUnitJsonFileFolder(courseId));
            StorageFile sourceFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri(file.Replace("\\", "/")));
            string result = await FileIO.ReadTextAsync(sourceFile);
            return result;
        }

        private static string GetUnitFolder(string courseId)
        {
            var index = courseId.IndexOf("/");
            var unitIndex = courseId.Substring(index + 1, 2);
            return "Unit" + unitIndex;
        }

        private static string GetUnitJsonFileFolder(string courseId)
        {
            if (courseId.Contains("p3newword1"))
            {
                return "TextB.json";
            }

            return "TextA.json";
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
