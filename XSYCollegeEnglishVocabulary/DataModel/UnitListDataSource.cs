using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XSYCollegeEnglishVocabulary.DataModel
{
    public class UnitList
    {
        public UnitList(string unitId, string unitName)
        {
            UnitID = unitId;
            UnitName = unitName;
        }

        public string UnitID { get; set; }
        public string UnitName { get; set; }

        public override string ToString()
        {
            return UnitName;
        }
    }

    public sealed class UnitListDataSource
    {
        private static UnitListDataSource _unitListDataSource = new UnitListDataSource();

        private ObservableCollection<UnitList> _unitLists = new ObservableCollection<UnitList>();
        public ObservableCollection<UnitList> UnitLists
        {
            get { return this._unitLists; }
        }

        private string bookId;

        public static IEnumerable<UnitList> GetUnitList(string bookId)
        {
            _unitListDataSource.GetDataAsync(bookId);

            return _unitListDataSource.UnitLists;
        }

        private void GetDataAsync(string bookId)
        {
            this.bookId = bookId;

            if (this.UnitLists.Count > 0)
            {
                return;
            }

            for (int unit = 1; unit <= 10; unit++)
            {
                UnitList unitTitle = new UnitList(unit.ToString(), string.Format("UNIT{0}", unit.ToString().PadLeft(2, '0')));
                this.UnitLists.Add(unitTitle);
            }
        }


    }
}
