using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Factory.Models;
using System.Collections.Generic;
using System.Linq;

namespace Factory.Controllers
{
  public class MachinesController : Controllers
  {
    private readonly FactoryContext _db;

    public MachinesController (FactoryContext db)
    {
      _db = db;
    }

    public Actionresult Index(string sortOrder, string searchString)
    {
      ViewBag.NameSortParam = string.IsNullOrEmpty(sortOrder) ?"name_desc" : "";
      ViewBag.StatusSortParam = sortOrder == "Status" ? "status_desc" : "Status";
      Viewbag.LastRepairSortParam = sortOrder == "LastRepair" ? "repair_desc" : "LastRepair";
      Viewbag.NextInspectionParam = sortOrder == "NextInspection" ? "inspection_desc" : "NextInspection";

      IQueryable<Machine> machines = _db.Machines
        .Include(machine => machine.Engineers)
        .ThenInclude(join => join.engineer);

      if (!string.IsNullOrEmpty(searchString))
      {
        machines = machines.Where(machine => machine.Name.Contains(searchString));
      }

      switch (sortOrder)
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
      return View(machines.ToList());
    }
  }
}