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

    public ActionResult Create()
    {
      return View();
    }

    [HttpPost]
    public ActionResult Create(MachinesController machines)
    {
      _db.Machines.Add(Machine);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Details(int id)
    {
      var machine = _db.Machines
        .Include(machine => machine.Engineers)
        .ThenInclude(join => join.Engineer)
        .FirstOrDefault(machine => machine.MachineId == id);
      return View(machine);
    }

    public ActionResult Edit (int id)
    {
      var machine = _db.Machines.FirstOrDefault(machine => machine.MachineId == id);
      ViewBag.Engineers = new SelectList(_db.Engineers, "MachineId", "Name");
      return View(machine);
    }

    [HttpPost]
    public ActionResult Edit(Machine machine, int EngineerId)
    {
      if(EngineerId !=0)
      {
        _db.EngineerMachine.Add(new EngineerMachine () { EngineerId = EngineerId, MachineId = machine.MachineId});
      }
      _db.Entry(machine).State = EntityState.Modified;
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Delete(int id)
    {
      var machine = _db.Machines.FirstOrDefault(machine => machine.MachineId == id);
      return View(machine);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      var machine = _db.Machines.FirstOrDefault(machine => machine.MachineId == id);
      _db.Machines.Remove(machine);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    [HttpPost]
    public ActionResult DeleteEngineer(int joinId)
    {
      var joinEntry = _db.EngineerMachine.FirstOrDefault(joinEntry => joinEntry.EngineerMachineId == joinId);
      _db.EngineerMachine.Remove(joinEntry);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

  }
}