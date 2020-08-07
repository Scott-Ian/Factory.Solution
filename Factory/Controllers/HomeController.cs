using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Factory.Models;
using System.Collections.Generic;
using System.Linq;

namespace Factory.Controllers
{
  public class HomeController : Controller
  {
    private readonly FactoryContext _db;

    public HomeController (FactoryContext db)
    {
      _db = db;
    }
    [HttpGet("/")]
    public ActionResult Index(string engineerSortOrder, string engineerSearchString, string machineSortOrder, string machineSearchString)
    {
      ViewBag.LastNameSortParam = string.IsNullOrEmpty(engineerSortOrder) ?"last_desc" : "";
      ViewBag.FirstNameSortParam = engineerSortOrder == "FirstName" ? "first_desc" : "FirstName";
      ViewBag.SalarySortParam = engineerSortOrder == "Salary" ? "salary_desc" : "Salary";
      ViewBag.DateSortParam = engineerSortOrder == "Date" ? "date_desc" : "Date";

      IQueryable<Engineer> engineers = _db.Engineers
        .Include(engineer => engineer.Machines)
        .ThenInclude(join => join.Machine);

      if (!string.IsNullOrEmpty(engineerSearchString))
      {
        engineers = engineers.Where(engineer => engineer.FirstName.Contains(engineerSearchString) || engineer.LastName.Contains(engineerSearchString));
      }

      switch (engineerSortOrder)
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

      ViewBag.MachineNameSortParam = string.IsNullOrEmpty(machineSortOrder) ?"name_desc" : "";
      ViewBag.MachineStatusSortParam = machineSortOrder == "Status" ? "status_desc" : "Status";
      ViewBag.MachineLastRepairSortParam = machineSortOrder == "LastRepair" ? "repair_desc" : "LastRepair";
      ViewBag.MachineNextInspectionParam = machineSortOrder == "NextInspection" ? "inspection_desc" : "NextInspection";

      IQueryable<Machine> machines = _db.Machines
        .Include(machine => machine.Engineers)
        .ThenInclude(join => join.Engineer);

      if (!string.IsNullOrEmpty(machineSearchString))
      {
        machines = machines.Where(machine => machine.Name.Contains(machineSearchString));
      }

      switch (machineSortOrder)
      {
        case "name_desc":
          machines = machines.OrderByDescending(machine => machine.Name);
          break;
        case "Status":
          machines = machines.OrderBy(machine => machine.Status);
          break;
        case "statust_desc":
          machines = machines.OrderByDescending(machine => machine.Status);
          break;
        case "LastRepair":
          machines = machines.OrderBy(machine => machine.DateOfLastRepair);
          break;
        case "repair_desc":
          machines = machines.OrderByDescending(machine => machine.DateOfLastRepair);
          break;
        case "NextInspection":
          machines = machines.OrderBy(machine => machine.NextInspection);
          break;
        case "inspection_desc":
          machines = machines.OrderByDescending(machine => machine.NextInspection);
          break;
        default:
          machines = machines.OrderBy(machine => machine.Name);
          break;
      }
      ViewBag.Machines = machines.ToList();
      return View(engineers.ToList());
    }
  }
}