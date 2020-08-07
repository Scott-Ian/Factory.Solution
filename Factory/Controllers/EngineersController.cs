using Factory.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Factory.Controllers
{
  public class EngineersController : Controllers
  {
    private readonly FactoryContext _db;

    public EngineersController (FactoryContext db)
    {
      _db = db;
    }

    public Actionresult Index(string sortOrder, string searchString)
    {
      ViewBag.LastNameSortParam = string.IsNullOrEmpty(sortOrder) ?"last_desc" : "";
      ViewBag.FirstNameSortParam = sortOrder == "FirstName" ? "first_desc" : "FirstName";
      ViewBag.SalarySortParam = sortOrder == "Salary" ? "salary_desc" : "Salary";
      Viewbag.DateSortParam = sortOrder == "Date" ? "date_desc" : "Date";

      IQueryable<Engineer> engineers = _db.Engineers
        .Include(engineer => engineer.Machines)
        .ThenInclude(join => join.Machine);

      if (!string.IsNullOrEmpty(searchString))
      {
        engineers = engineers.Where(engineer => engineer.FirstName.Contains(searchString) || engineer.LastName.Contains(searchString));
      }

      switch (sortOrder)
      {
        case "last_desc":
          engineers = engineers.OrderByDescending(engineer => engineer.LastName);
          break;
        case "FirstName":
          engineers = engineers.OrderBy(engineer => engineer.FirstName);
          break;
        case "first_desc":
          engineers = engineers.OrderByDescending(engineer => engineer.FirstName);
          break;
        case "Salary":
          engineers = engineers.OrderBy(engineer => engineer.Salary);
          break;
        case "salary_desc":
          engineers = engineers.OrderByDescending(engineer => engineer.Salary);
          break;
        case "Date":
          engineers = engineers.OrderBy(engineer => engineer.DateOfHire);
          break;
        case "date_desc":
          engineers = engineers.OrderByDescending(engineer => engineer.DateOfHire);
          break;
        default:
          engineers = engineers.OrderBy(engineer => engineer.LastName);
          break;
      }
      return View(engineers.ToList());
    }
  }
}