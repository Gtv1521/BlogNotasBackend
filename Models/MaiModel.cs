using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BackEndNotes.Dto;

namespace BackEndNotes.Models
{
    public class MailModel : MailDto
    {
        public string? Motivo { get; set; }
        public string? Message { get; set; }
    }
}