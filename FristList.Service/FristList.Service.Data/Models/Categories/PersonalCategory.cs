using FristList.Service.Data.Models.Account;
using FristList.Service.Data.Models.Categories.Base;

namespace FristList.Service.Data.Models.Categories;

public class PersonalCategory : BaseCategory
{
    public User Owner { get; set; }
}