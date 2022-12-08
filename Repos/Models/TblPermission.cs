using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace firstapi.Repos.Models;

[PrimaryKey("RoleId", "MenuId")]
[Table("tbl_permission")]
public partial class TblPermission
{
    [Key]
    [StringLength(50)]
    [Unicode(false)]
    public string RoleId { get; set; } = null!;

    [Key]
    [StringLength(50)]
    [Unicode(false)]
    public string MenuId { get; set; } = null!;
}
