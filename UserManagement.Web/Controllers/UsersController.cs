using System;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UserManagement.Data;
using UserManagement.Models;
using UserManagement.Services.Domain.Implementations;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Models.LogItems;
using UserManagement.Web.Models.Users;

namespace UserManagement.WebMS.Controllers;

[Route("users")]
public class UsersController : Controller
{
    private readonly IUserService _userService;
    public UsersController(IUserService userService) => _userService = userService;

    [HttpGet]
    [Route("View")]
    public ViewResult ViewUser(int id)
    {
        if (_userService.GetAll().Where(p => p.Id == id).Count() == 0)
            return View("NotFound");

        string? val = Convert.ToString(TempData["Status"]);
        ViewBag.Message = !string.IsNullOrWhiteSpace(val)? val:"View User";

        var item = _userService.GetAll().Select(p => new UserListItemViewModel
        {
            Id = p.Id,
            Forename = p.Forename,
            Surname = p.Surname,
            Email = p.Email,
            IsActive = p.IsActive,
            DateOfBirth = p.DateOfBirth,
            Logs = p.Logs.Select(q => new Web.Models.Users.LogEntry { Forename = q.Entry}).ToList(),
        }).Where(p=>p.Id==id).FirstOrDefault();

        if (item != null)
        {
            if(item.Logs!=null)
                item.Logs.Add(new Web.Models.Users.LogEntry());
        }
        return View(item);
    }

    [HttpGet]
    public ViewResult List(string active)
    {
        bool showActive = Convert.ToBoolean(active);
        IEnumerable<UserListItemViewModel> items;

        if (!string.IsNullOrWhiteSpace(active))
        {
            items = _userService.FilterByActive(showActive).Select(p => new UserListItemViewModel
            {
                Id = p.Id,
                Forename = p.Forename,
                Surname = p.Surname,
                Email = p.Email,
                IsActive = p.IsActive,
                DateOfBirth = p.DateOfBirth,
                Logs = p.Logs.Select(q => new Web.Models.Users.LogEntry { Forename = q.Entry }).ToList(),
            });
        }
        else
        {
            items = _userService.GetAll().Select(p => new UserListItemViewModel
            {
                Id = p.Id,
                Forename = p.Forename,
                Surname = p.Surname,
                Email = p.Email,
                IsActive = p.IsActive,
                DateOfBirth = p.DateOfBirth,
                Logs = p.Logs.Select(q => new Web.Models.Users.LogEntry {  Forename = q.Entry }).ToList(),
            });
        }

        var model = new UserListViewModel
        {
            Items = items.ToList()
        };

        return View(model);
    }

    [HttpGet]
    [Route("Create")]
    public ActionResult create()
    {
        return View();
    }

    [HttpPost]
    [Route("Create")]
    public ActionResult create(UserListItemViewModel model)
    {
        Models.User usermodel = new Models.User
        {
            DateOfBirth = model.DateOfBirth ?? new DateOnly(),
           Email = model.Email ?? string.Empty,
            IsActive = model.IsActive,
            Forename = model.Forename ?? string.Empty,
            Surname = model.Surname ?? string.Empty
        };

        usermodel.Logs = new List<Models.LogEntry>();
        Models.LogEntry en = new Models.LogEntry();
        en.Entry = "JAMES";
        en.Id = usermodel.Id;
        en.UId = usermodel;
        usermodel.Logs.Add(en);

        if (!ModelState.IsValid)
        {
            return View(usermodel);
        }

        _userService.Create(usermodel);

        TempData["Status"] = "User record created successfully";

        return RedirectToAction("ViewUser", new { id = usermodel.Id });
    }

    [HttpGet]
    [Route("Edit")]
    public ViewResult EditUser(int id)
    {
        if (_userService.GetAll().Where(p => p.Id == id).Count() == 0)
            return View("NotFound");

        var item = _userService.GetAll().Select(p => new Models.User
        {
            Id = p.Id,
            Forename = p.Forename,
            Surname = p.Surname,
            Email = p.Email,
            IsActive = p.IsActive,
            DateOfBirth = p.DateOfBirth,
        }).Where(p => p.Id == id).FirstOrDefault();

        return View(item);
    }

    [HttpPost]
    [Route("Edit")]
    public ActionResult EditUser(UserListItemViewModel model)
    {
        Models.User usermodel = new Models.User
        {
            DateOfBirth = model.DateOfBirth ?? new DateOnly(),
            Email = model.Email ?? string.Empty,
            IsActive = model.IsActive,
            Forename = model.Forename ?? string.Empty,
            Surname = model.Surname ?? string.Empty,
            Id = model.Id
        };

        if (!ModelState.IsValid)
        {
            return View(usermodel);
        }

        _userService.Update(usermodel);

        TempData["Status"] = "User record updated successfully";
        return RedirectToAction("ViewUser", new { id = usermodel.Id });
    }

    [HttpGet]
    [Route("Delete")]
    public ActionResult DeleteUser(int id)
    {
        if (_userService.GetAll().Where(p => p.Id == id).Count()==0)
            return View("NotFound");

        var item = _userService.GetAll().Select(p => new Models.User
        {
            Id = p.Id,
            Forename = p.Forename,
            Surname = p.Surname,
            Email = p.Email,
            IsActive = p.IsActive,
            DateOfBirth = p.DateOfBirth
        }).Where(p => p.Id == id).FirstOrDefault();

        return View(item);

    }

    [HttpPost]
    [Route("Delete")]
    public ActionResult DeleteUser(UserListItemViewModel model)
    {
        Models.User usermodel = new Models.User
        {
            DateOfBirth = model.DateOfBirth ?? new DateOnly(),
            Email = model.Email ?? string.Empty,
            IsActive = model.IsActive,
            Forename = model.Forename ?? string.Empty,
            Surname = model.Surname ?? string.Empty,
            Id = model.Id
        };

        _userService.Delete(usermodel);

        return View("DeleteConfirmed");

    }
}


