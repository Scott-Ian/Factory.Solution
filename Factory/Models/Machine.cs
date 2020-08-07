using System.Collections.Generic;
using System;

namespace Factory.Models
{
  public class Machine
  {
    public Machine()
    {
      this.Engineers = new HashSet<EngineerMachine>();
    }

    public int MachineId { get; set; }
    public string Name { get; set; }
    public string Status { get; set; }
    public String Description { get; set; }
    public DateTime DateOfLastRepair { get; set; }
    public DateTime NextInspection { get; set; }

    public ICollection<EngineerMachine> Engineers { get; set; }
  }
}