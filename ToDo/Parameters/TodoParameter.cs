using System.Text.RegularExpressions;

namespace ToDo.Parameters;

public class TodoParameter
{
    public string? Name { get; set; }
    public bool? Enable { get; set; }
    public DateTime? InsertTime { get; set; }
    public int? MinOrder { get; set; }
    public int? MaxOrder { get; set; }


    #region 藉由一個屬性蒐集MinOrder 和 MaxOrder訊息

    private string? _order;

    public string? Order
    {
        get => _order;
        set
        {
            Regex regex = new Regex(@"^\d-\d$");
            if (value != null && regex.IsMatch(value))
            {
                MinOrder = Int32.Parse(value.Split('-')[0]);
                MaxOrder = Int32.Parse(value.Split('-')[1]);
            }
            _order = value;
        }
    }

    #endregion

}