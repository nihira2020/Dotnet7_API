using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace firstapi.Repos.Models;

[Table("tbl_designation")]
public partial class TblDesignation
{
    [Key]
    [Column("code")]
    [StringLength(10)]
    public string Code { get; set; } = null!;

    [StringLength(50)]
    public string? Name { get; set; }
}
