using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ImmersiveQuiz.Models
{
    public class Location
    {
        public int LocationId { get; set; }

        public string Name { get; set; }

        public Guid ImageGuid { get; set; }

        public string ImageExtension { get; set; }

        public string ImagePath {
            get
            {
                return $"/images/{ImageGuid}{ImageExtension}";
            }
        }

        //public int CourseId { get; set; }

        [NotMapped]
        public Course Course { get; set; }
    }
}
