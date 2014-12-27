using System.Collections.Generic;

namespace XSYCollegeEnglishVocabulary.DataModel
{
    public class Vocabulary
    {
        public string Word { get; set; }
        public string Voice { get; set; }
        public ICollection<Definition> Definitions { get; set; }
    }
}
