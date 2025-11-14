using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace src.Dto
{
    public class ShareNoteDto
    {
        public string IdUser { get; set; }
        public string IdNote { get; set; }
        public string IdReferido { get; set; }
        public bool ReadPermits { get; set; }
        public bool WritePermits { get; set; }
    }
}