﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todo_App.Application.Tags.Queries.GetTags;
public class TagVm
{
    public IList<string> List { get; set; } = new List<string>();
    public IList<TagDto> Tags { get; set; } = new List<TagDto>();
}
