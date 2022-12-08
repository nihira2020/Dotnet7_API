using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace firstapi.Repos.Models;

[Table("tbl_Orgin")]
public partial class TblOrgin
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    public string OrginName { get; set; } = null!;

    public bool IsActive { get; set; }
}
