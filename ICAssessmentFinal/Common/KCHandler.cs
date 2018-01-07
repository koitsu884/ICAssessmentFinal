namespace ICAssessmentFinal.Common
{
    public class KCHandler
    {
        public static string ConvertBindTypeFromDataType(string columnName, string dataType, bool readOnly = false)
        {
            if (readOnly)
            {
                return "<span data-bind=\"text: " + columnName + "\"><span/>";
            }
            string elementData;
            switch (dataType)
            {
                case "ProductCategoryEnum":
                    elementData = "<select data-bind=\"options : $root.ProductCategoryEnum, value: " + columnName + "\" />";
                    break;
                case "Boolean":
                    elementData = "<input type=\"checkbox\" data-bind=\"checked: " + columnName + "\" />";
                    break;
                default:
                    elementData = "<input data-bind=\"value: " + columnName + "\" />";
                    break;
            }
            return elementData;
        }
    }
}