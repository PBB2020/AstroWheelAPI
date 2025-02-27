﻿using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace AstroWheelAPI.Models
{
    public class Player
    {
        [Key]
        public int PlayerId { get; set; }
        [Required]
        [StringLength(255)]
        public string PlayerName { get; set; } = string.Empty;
        [Required]
        [ForeignKey("User")]
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser User { get; set; } = null!;
        [Required]
        [ForeignKey("Character")]
        public int CharacterId { get; set; }
        public Character? Character { get; set; }
        [Required]
        [ForeignKey("Island")]
        public int  IslandId { get; set; }
        public Island? Island { get; set; }
        [Required]
        [ForeignKey("Inventory")]
        public int InventoryId { get; set; }
        public Inventory? Inventory { get; set; }
        [Required]
        [ForeignKey("RecipeBook")]
        public int RecipeBookId { get; set; }
        public RecipeBook? RecipeBook { get; set; }
        public DateTime? LastLogin { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
