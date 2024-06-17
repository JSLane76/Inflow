using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace UserManagement.Web.Models.LogItems;

public class LogListViewModel
{
    public List<LogListItemViewModel> Items { get; set; } = new();
}

public class LogListItemViewModel
{
    public long Id { get; set; }

}


