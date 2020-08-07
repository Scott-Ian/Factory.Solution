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

    public ActionResult Create()
    {
      return View();
    }

    [HttpPost]
    public ActionResult Create(Engineer engineer)
    {
      _db.Engineers.Add(engineer);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Details(int id)
    {
      var engineer = _db.Engineers
        .Include(engineer => engineer.Machines)
        .ThenInclude(join => join.Machine)
        .FirstOrDefault(engineer => engineer.EngineerId == id);
      return View(engineer);
    }

    public ActionResult Edit (int id)
    {
      var engineer = _db.Engineers.FirstOrDefault(engineer => engineer.EngineerId == id);
      ViewBag.Machines = new SelectList(_db.Machines, "MachineId", "Name");
      return View(engineer);
    }

    [HttpPost]
    public ActionResult Edit(Engineer engineer, int MachineId)
    {
      if(MachineId !=0)
      {
        _db.EngineerMachine.Add(new EngineerMachine () { MachineId = MachineId, EngineerId = engineer.EngineerId});
      }
      _db.Entry(engineer).State = EntityState.Modified;
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Delete(int id)
    {
      var engineer = _db.Engineers.FirstOrDefault(engineer => engineer.EngineerId == id);
      return View(engineer);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed (int id)
    {
      var engineer = _db.Engineers.FirstOrDefault(engineer => engineer.EngineerId == id);
      _db.Engineers.Remote(engineer);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    [HttpPost]
    public ACtionResult DeleteMachine(int joinid)
    {
      var joinEntry = _db.EngineerMachine.FirstOrDefault(joinEntry => joinEntry.EngineerMachineId == joinId);
      _db.CourseStudent.Remove(joinEntry);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult AddMachine (int id)
    {
      var engineer = _db.Engineer.FirstOrDefault(engineer => engineer.EngineerId == id);     ViewBag.Machines = new SelectList(_db.Machines, "MachineId", "Name");
      return View(engineer);
    }

    [HttpPost]
    public ActionResult AddMachine (Machine machine, int EngineerId)
    {
      if (EngineerId != 0)
      {
        _db.EngineerMachine.Add(new EngineerMachine() { EngineerId = MachineId, EngineerId = engineer.EngineerId } );
      }
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
  }
}