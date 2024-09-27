using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities;

public class BaseEntities
{
    [Key]
    public int Id { get; set; }
}
