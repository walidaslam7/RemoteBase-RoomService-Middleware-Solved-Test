using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace RoomService.WebAPI.Data
{
    public class RoomContext : DbContext
    {
        public RoomContext(DbContextOptions<RoomContext> options)
            : base(options)
        { }

        public DbSet<Room> Rooms { get; set; }
    }

    public class Room
    {
        [Key]
        public int Id { get; set; }

        public string Category { get; set; }

        public int Number { get; set; }

        public int Floor { get; set; }

        public bool IsAvailable { get; set; }

        public DateTime AddedDate { get; set; }
    }
}
