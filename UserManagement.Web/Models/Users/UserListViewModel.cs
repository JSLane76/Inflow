using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

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

}


