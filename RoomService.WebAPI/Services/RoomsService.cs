using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RoomService.WebAPI.Data;

namespace RoomService.WebAPI.Services
{
    public class RoomsService : IRoomsService
    {
        private readonly RoomContext _roomContext;

        public RoomsService(RoomContext roomContext)
        {
            _roomContext = roomContext;
        }

        public async Task<IEnumerable<Room>> Get(int[] ids, Filters filters)
        {
            var books = _roomContext.Rooms.AsQueryable();

            if (filters == null)
                filters = new Filters();

            if (filters.Categories != null && filters.Categories.Any())
                books = books.Where(x => filters.Categories.Contains(x.Category));

            if (filters.Floors != null && filters.Floors.Any())
                books = books.Where(x => filters.Floors.Contains(x.Floor));

            if (ids != null && ids.Any())
                books = books.Where(x => ids.Contains(x.Id));

            return await books.ToListAsync();
        }

        public async Task<Room> Add(Room room)
        {
            await _roomContext.Rooms.AddAsync(room);
            room.AddedDate = DateTime.UtcNow;

            await _roomContext.SaveChangesAsync();
            return room;
        }

        public async Task<IEnumerable<Room>> AddRange(IEnumerable<Room> rooms)
        {
            await _roomContext.Rooms.AddRangeAsync(rooms);
            await _roomContext.SaveChangesAsync();
            return rooms;
        }

        public async Task<Room> Update(Room room)
        {
            var bookForChanges = await _roomContext.Rooms.SingleAsync(x => x.Id == room.Id);
            bookForChanges.IsAvailable = room.IsAvailable;
            bookForChanges.Category = room.Category;
            bookForChanges.Floor = room.Floor;
            bookForChanges.Number = room.Number;

            _roomContext.Rooms.Update(bookForChanges);
            await _roomContext.SaveChangesAsync();
            return room;
        }

        public async Task<bool> Delete(Room room)
        {
            _roomContext.Rooms.Remove(room);
            await _roomContext.SaveChangesAsync();

            return true;
        }
    }

    public interface IRoomsService
    {
        Task<IEnumerable<Room>> Get(int[] ids, Filters filters);

        Task<Room> Add(Room room);

        Task<IEnumerable<Room>> AddRange(IEnumerable<Room> rooms);

        Task<Room> Update(Room room);

        Task<bool> Delete(Room room);
    }

    public class Filters
    {
        public string[] Categories { get; set; }
        public int[] Floors { get; set; }
    }
}
