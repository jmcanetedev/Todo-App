using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todo_App.Application.Common.Models;
public class BaseDto
{
    public DateTime? DeletedOn { get; private set; }
    public bool IsDeleted => DeletedOn != null;
}
