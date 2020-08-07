using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Factory.Models;
using System.Collections.Generic;
using System.Linq;

namespace Factory.Controllers
{
  public class MachinesController : Controller
  {
    private readonly FactoryContext _db;

    public MachinesController (FactoryContext db)
    {
      _db = db;
    }

    public ActionResult Index(string sortOrder, string searchString)
    {
      ViewBag.NameSortParam = string.IsNullOrEmpty(sortOrder) ?"name_desc" : "";
      ViewBag.StatusSortParam = sortOrder == "Status" ? "status_desc" : "Status";
      ViewBag.LastRepairSortParam = sortOrder == "LastRepair" ? "repair_desc" : "LastRepair";
      ViewBag.NextInspectionParam = sortOrder == "NextInspection" ? "inspection_desc" : "NextInspection";

      IQueryable<Machine> machines = _db.Machines
        .Include(machine => machine.Engineers)
        .ThenInclude(join => join.Engineer);

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
    public ActionResult Create(Machine machine)
    {
      _db.Machines.Add(machine);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Details(int id)
    {
      var machine = _db.Machines
        .Include(m => m.Engineers)
        .ThenInclude(join => join.Engineer)
        .FirstOrDefault(m => m.MachineId == id);
      return View(machine);
    }

    public ActionResult Edit (int id)
    {
      var machine = _db.Machines.FirstOrDefault(m => m.MachineId == id);
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
      var machine = _db.Machines.FirstOrDefault(m => m.MachineId == id);
      return View(machine);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      var machine = _db.Machines.FirstOrDefault(m => m.MachineId == id);
      _db.Machines.Remove(machine);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    [HttpPost]
    public ActionResult DeleteEngineer(int joinId)
    {
      var joinEntry = _db.EngineerMachine.FirstOrDefault(entry => entry.EngineerMachineId == joinId);
      _db.EngineerMachine.Remove(joinEntry);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult AddEngineer (int id)
    {
      var machine = _db.Machines.FirstOrDefault(m => m.MachineId == id);
      ViewBag.Engineers = new SelectList(_db.Engineers, "EngineerId", "Name");
      return View(machine);
    }

    [HttpPost]
    public ActionResult AddEngineer (Engineer engineer, int MachineId)
    {
      if (MachineId != 0)
      {
        _db.EngineerMachine.Add(new EngineerMachine() { MachineId = MachineId, EngineerId = engineer.EngineerId } );
      }
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

  }
}