using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp2.Models
{
    [Table("TMixedZoneSteelGroup")]
    public class MixedZoneSteelGroup
    {
        [Key]
        [Column(Order = 0)]
        public int Fsg_GroupId { get; set; }

        [Key]
        [Column(Order = 1)]
        public int Fsg_Row { get; set; }

        public int? Fsg_MixedZoneSteelGroup { get; set; }
    }
}
