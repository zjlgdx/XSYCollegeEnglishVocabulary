using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;
using XSYCollegeEnglishVocabulary.Common;

namespace XSYCollegeEnglishVocabulary.DataModel
{
    public sealed class VocabularyDataSource
    {
        private static VocabularyDataSource _vocabularyDataSource = new VocabularyDataSource();

        public Unit Unit { get; private set; }

        private string unitId;

        public static async Task<Unit> GetVocabulariesAsync(string unitId, string bookText)
        {
            await _vocabularyDataSource.GetDataAsync(unitId, bookText);

            return _vocabularyDataSource.Unit;
        }

        private async Task GetDataAsync(string unitId, string bookText)
        {
            this.unitId = unitId;

            string jsonText = await StorageDataHelper.GetUnit(unitId, bookText);

            this.Unit = JsonConvert.DeserializeObject<Unit>(jsonText);

        }
    }
}
