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
    public string Fields { get; set; } = default!;
    public string Action { get; set; } = default!;

    public DateTime Created { get; set; } = default!;
}


