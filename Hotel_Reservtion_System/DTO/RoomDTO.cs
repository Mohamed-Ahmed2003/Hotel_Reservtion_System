using Hotel_Reservtion_System.CastuomValidation;
using Hotel_Reservtion_System.Entity;
using Microsoft.AspNetCore.DataProtection.KeyManagement.Internal;
using System.ComponentModel.DataAnnotations;

namespace Hotel_Reservtion_System.DTO
{
    public class RoomDTO
    {
        [Required(ErrorMessage = "Room type is required")]
        [RoomTypeValidation(ErrorMessage = "Room type must be either 'Single', 'Double', or 'Suite'.")]
        public string? roomType { get; set; }
        [Required(ErrorMessage = "Room code is required")]
        public string? roomCode { get; set; }
        [Required(ErrorMessage = "Room price is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive number")]
        public double? price { get; set; }

        public bool availability { get; set; } = true;
        [Required(ErrorMessage = "branch is required")]
        public Guid? branchID { get; set; }



        public Room convertToRoom()
        {
            Room room = new Room();
            room.id = Guid.NewGuid();
            room.roomType = this.roomType;
            room.roomCode = this.roomCode;
            room.price = this.price;
            room.availability = this.availability;
            room.cheakout=DateTime.Parse("2000-1-1");
            return room;
        }
       /* public RoomDTO convertToDto(Room room)
        {
            RoomDTO roomDTO = new RoomDTO();
            roomDTO.roomType = room.roomType;
            roomDTO.roomCode = room.roomCode;
            roomDTO.price = room.price;
            roomDTO.availability = room.availability;
            if (room.branch != null)
            {
                roomDTO.branchID = room.branch.id;
            }
            return roomDTO;
        }
*/
    }
}
