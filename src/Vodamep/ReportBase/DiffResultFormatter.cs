using System;
using System.Text;
using Vodamep.ReportBase;

namespace Vodamep.Hkpv.Validation
{
    public class DiffResultFormatter
    {

        private readonly bool _hideUnchanged;

        public DiffResultFormatter(bool hideUnchanged = true)
        {
            _hideUnchanged = hideUnchanged;
        }

        public string Format(DiffResult diffResult)
        {
            var stringBuilder = new StringBuilder();

            this.Format(diffResult, stringBuilder, 0);

            return stringBuilder.ToString();
        }

        public void Format(DiffResult diffResult, StringBuilder stringBuilder, int level)
        {
            if (diffResult == null || (_hideUnchanged && diffResult.Status == Status.Unchanged)) return;

            var isHeader = string.IsNullOrWhiteSpace(diffResult.PropertyName) && diffResult.Value1 == null &&
                           diffResult.Value2 == null;

            var tabs = GetTabs(level);

            stringBuilder.Append(Environment.NewLine);

            if (!isHeader)
            {
                stringBuilder.Append(tabs);
                stringBuilder.Append($"{nameof(diffResult.PropertyName)}:\t");
                stringBuilder.Append(diffResult.PropertyName);
                stringBuilder.Append(Environment.NewLine);
            }

            stringBuilder.Append(tabs);
            stringBuilder.Append($"{nameof(diffResult.Status)}:\t");
            stringBuilder.Append(diffResult.Status);
            stringBuilder.Append(Environment.NewLine);

            stringBuilder.Append(tabs);
            stringBuilder.Append($"{nameof(diffResult.Type)}:\t");
            stringBuilder.Append(diffResult.Type);
            stringBuilder.Append(Environment.NewLine);

            if (!isHeader)
            {
                stringBuilder.Append(tabs);
                stringBuilder.Append($"{nameof(diffResult.Value1)}:\t");
                stringBuilder.Append(diffResult.Value1);
                stringBuilder.Append(Environment.NewLine);
            }


            if (!isHeader)
            {
                stringBuilder.Append(tabs);
                stringBuilder.Append($"{nameof(diffResult.Value2)}:\t");
                stringBuilder.Append(diffResult.Value2);
                stringBuilder.Append(Environment.NewLine);
            }

            foreach (var child in diffResult.Children)
            {
                this.Format(child, stringBuilder, level + 1);
            }
        }

        private string GetTabs(int level)
        {
            var result = string.Empty;
            for (int i = 0; i < level; i++)
            {
                result += "\t";
            }

            return result;
        }
    }
}