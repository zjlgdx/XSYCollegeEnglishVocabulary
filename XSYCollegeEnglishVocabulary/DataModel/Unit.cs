using System.Collections.Generic;

namespace XSYCollegeEnglishVocabulary.DataModel
{
    public class Unit
    {
        public string UnitTitle { get; set; }
        public ICollection<Vocabulary> Vocabularies { get; set; }
    }
}