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
    
  }
}