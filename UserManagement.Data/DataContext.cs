using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using UserManagement.Models;

namespace UserManagement.Data;

public class DataContext : DbContext, IDataContext
{
    public DataContext() => Database.EnsureCreated();

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseInMemoryDatabase("UserManagement.Data.DataContext");

    protected override void OnModelCreating(ModelBuilder model)
        => model.Entity<User>().HasData(new[]
        {
            new User { Id = 1, Forename = "Peter", Surname = "Loew", Email = "ploew@example.com", IsActive = true, DateOfBirth=new System.DateOnly(1976,10,10)},
            new User { Id = 2, Forename = "Benjamin Franklin", Surname = "Gates", Email = "bfgates@example.com", IsActive = true, DateOfBirth=new System.DateOnly(1976,10,10) },
            new User { Id = 3, Forename = "Castor", Surname = "Troy", Email = "ctroy@example.com", IsActive = false, DateOfBirth=new System.DateOnly(1976,10,10) },
            new User { Id = 4, Forename = "Memphis", Surname = "Raines", Email = "mraines@example.com", IsActive = true, DateOfBirth=new System.DateOnly(1976,10,10) },
            new User { Id = 5, Forename = "Stanley", Surname = "Goodspeed", Email = "sgodspeed@example.com", IsActive = true, DateOfBirth=new System.DateOnly(1976,10,10) },
            new User { Id = 6, Forename = "H.I.", Surname = "McDunnough", Email = "himcdunnough@example.com", IsActive = true, DateOfBirth=new System.DateOnly(1976,10,10) },
            new User { Id = 7, Forename = "Cameron", Surname = "Poe", Email = "cpoe@example.com", IsActive = false, DateOfBirth=new System.DateOnly(1976,10,10) },
            new User { Id = 8, Forename = "Edward", Surname = "Malus", Email = "emalus@example.com", IsActive = false, DateOfBirth=new System.DateOnly(1976,10,10) },
            new User { Id = 9, Forename = "Damon", Surname = "Macready", Email = "dmacready@example.com", IsActive = false, DateOfBirth=new System.DateOnly(1976,10,10) },
            new User { Id = 10, Forename = "Johnny", Surname = "Blaze", Email = "jblaze@example.com", IsActive = true, DateOfBirth=new System.DateOnly(1976,10,10) },
            new User { Id = 11, Forename = "Robin", Surname = "Feld", Email = "rfeld@example.com", IsActive = true, DateOfBirth=new System.DateOnly(1976,10,10) },
        });

    public DbSet<User>? Users { get; set; }

    public IQueryable<TEntity> GetAll<TEntity>() where TEntity : class
        => base.Set<TEntity>();

    public void Create<TEntity>(TEntity entity) where TEntity : class
    {
        base.Add(entity);
        SaveChanges();
    }

    public new void Update<TEntity>(TEntity entity) where TEntity : class
    {
        base.Update(entity);
        SaveChanges();
    }

    //private void AddLogEntry(User? user, IEnumerable<EntityEntry> Entries)
    //{
    //    foreach (EntityEntry e in Entries)
    //    {

    //        LogEntry entry = new LogEntry();

    //        entry.Action =e.State.ToString();

    //        var originalValues = e.OriginalValues.Clone();
    //        originalValues.SetValues(e.OriginalValues);
    //        User original = (User)originalValues.ToObject();

    //        Type myType = original.GetType();
    //        string fieldValues = "";
    //        IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
    //        foreach (PropertyInfo prop in props)
    //        {
    //            object? propValue = prop.GetValue(original, null);
    //            string? propStringValue = propValue!=null? propValue.ToString() : "";
    //            string Name = prop.Name;

    //            fieldValues += $"{Name}: {propStringValue}, ";

    //            if(string.Compare(Name,"ID",true)==0)
    //            {
    //                int id;
    //                int.TryParse(propStringValue, out id);
    //                entry.ID = id;
    //            }
    //        }
            
    //        entry.CreatedDate = DateTime.Now;
    //        entry.FieldValues = fieldValues;
    //        if (user != null)
    //        {
    //            if(user.Entries==null)
    //            {
    //                user.Entries = new List<LogEntry>();
    //            }
    //            user.Entries.Add(entry);
    //        }
    //    }
    //}

    public void Delete<TEntity>(TEntity entity) where TEntity : class
    {
        base.Remove(entity);
        SaveChanges();
    }
}
