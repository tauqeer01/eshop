using System;
using Microsoft.AspNetCore.Identity;

namespace Core.Entities;

public class AppUser : IdentityUser
{
   public string FistName { get; set; }= string.Empty;
   public string LastName { get; set; }= string.Empty;
   public Address? Address { get; set; }
}
