using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todo_App.Domain.Exceptions;
public class DuplicateTagException : Exception
{
    public DuplicateTagException(string name)
       : base($"Tag Name \"{name}\" already exist.")
    {
    }
}
