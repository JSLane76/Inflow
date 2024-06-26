﻿using System;
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
    [Route("ViewUserLog")]
    public ViewResult ViewUserLog(int id, string command)
    {
        if (UserManagement.Data.Entities.Logs.Entries != null)
        {
            var items = UserManagement.Data.Entities.Logs.Entries.Select(p => new LogListItemViewModel
            {
                Id = p.UserID,
                Fields = p.FieldValues,
                Action = p.Action,
                Created = p.CreatedDate
            }).Where(p => p.Id == id);
            ViewBag.id = id;

            if (!string.IsNullOrWhiteSpace(command))
            {
                items = items.Where(p=>p.Action== command);
            }

            var model = new LogListViewModel
            {
                Items = items.ToList()
            };

            return View(model);
        }
        else
        {
            return View("NoLogs");
        }
    }

    [HttpGet]
    [Route("ViewUsersDeletedLog")]
    public ViewResult ViewUsersDeletedLog()
    {
        if (UserManagement.Data.Entities.Logs.Entries != null)
        {
            var items = UserManagement.Data.Entities.Logs.Entries.Select(p => new LogListItemViewModel
            {
                Id = p.UserID,
                Fields = p.FieldValues,
                Action = p.Action,
                Created = p.CreatedDate
            }).Where(p => p.Action == "Deleted");

            var model = new LogListViewModel
            {
                Items = items.ToList()
            };

            return View(model);
        }
        else
        {
            return View("NoDeletedLogs");
        }
    }

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
        }).Where(p=>p.Id==id).FirstOrDefault();


        return View(item);
    }

    [HttpGet]
    public ViewResult List(string active)
    {
        
        IEnumerable<UserListItemViewModel> items;

        if (!string.IsNullOrWhiteSpace(active))
        {
            bool showActive = Convert.ToBoolean(active);
            items = _userService.FilterByActive(showActive).Select(p => new UserListItemViewModel
            {
                Id = p.Id,
                Forename = p.Forename,
                Surname = p.Surname,
                Email = p.Email,
                IsActive = p.IsActive,
                DateOfBirth = p.DateOfBirth,
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


