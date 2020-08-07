using System;
using System.Collections.Generic;

namespace Factory.Models
{
  public class Engineer
  {
    public Engineer()
    {
      this.Machines = new HashSet<EngineerMachine>();
    }

    public int EngineerId { get; set; }
    public string FirstName {get; set; }
    public string LastName { get; set; }
    public string Name { get { return this.FirstName + " " + this.LastName;} }
    public int Salary { get; set; }
    public DateTime DateOfHire { get; set; }
    public ICollection<EngineerMachine> Machines { get; set; }
  }
}