﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace stackblob.Domain.Entities.Defaults;

public class BaseEntityUserTracking : BaseEntity
{
    public User? CreatedBy { get; set; } = null!;
    public string? CreatedById { get; set; }

}
