using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICAssessmentFinal.Common
{
    public class EditableTableOption
    {
        public List<Tuple<string, string>> Columnelemnts { get; set; }
        public bool CanAdd { get; set; }
        public bool CanRemove { get; set; }

        public EditableTableOption(List<Tuple<string, string>> columElements, bool canAdd = true, bool canRemove = true)
        {
            Columnelemnts = columElements;
            CanAdd = canAdd;
            CanRemove = canRemove;
        }
    }
}