﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DTO_s
{
    public class WorkOutDTO
    {
        public string Name { get; set; }
        public string? Comment { get; set; }

        public DateTime? ScheduleTime { get; set; }
        public List<string> ExerciseNames { get; set; }
    }
}
