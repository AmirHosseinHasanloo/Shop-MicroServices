﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace Ordering.Application.Mappings
{
    public class MappingProfile : Profile
    {
        protected MappingProfile()
        {
        }

        protected internal MappingProfile(string profileName) : base(profileName)
        {
        }
    }
}
