using System;
using System.Collections.Generic;

namespace Factory.Models
{
  public class Engineer
  {
    public Engineer()
    {
      this.Machines = new HashSet<EngineerMachine>();
      this.Name = FirstName + "" + LastName;
    }

    public int EngineerId { get; set; }
    public string FirstName {get; set; }
    public string LastName { get; set; }
    public string Name { get; set; }
    public int Salary { get; set; }
    public DateTime DateOfHire { get; set; }
    public ICollection<EngineerMachine> Machines { get; set; }
  }
}