using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todo_App.Domain.Entities;
public class Tag : BaseAuditableEntity
{
    public string Name { get; private set; } = string.Empty;
    public Tag(string name) => Name = name;
}
