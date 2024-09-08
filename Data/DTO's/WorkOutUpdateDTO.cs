using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DTO_s
{
    public class WorkOutUpdateDTO
    {
        public string Name { get; set; }
        public string? Comment { get; set; }

        public List<string> ExerciseNames { get; set; }
    }
}
