using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Screen_Capture.MVVM.Model
{
    public partial class HotKeys
    {
        [Key]
        [Column("HotKeyID")]
        public int HotKeyId { get; set; }
        [Required]
        public string Name { get; set; }
        public  int Ctrl { get; set; }
        public int Shift { get; set; }
        public int  Alt { get; set; }
        [Required]
        public int Button { get; set; }
    }
}
