using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserManagement.Web.Models.Users;

public class UserListViewModel
{
    public List<UserListItemViewModel> Items { get; set; } = new();
}

public class UserListItemViewModel
{
    public long Id { get; set; }
    [Required]
    public string? Forename { get; set; }
    [Required]
    public string? Surname { get; set; }
    [Required]
    public string? Email { get; set; }
    public bool IsActive { get; set; }
    [Required]
    [DisplayName("Date Of Birth")]
    public DateOnly? DateOfBirth { get; set; }

    public List<LogEntry>? Logs { get; set; }
}

public class LogEntry
{
    public long Id { get; set; }
    public string Forename { get; set; } = default!;
}


