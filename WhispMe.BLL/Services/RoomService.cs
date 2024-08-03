using AutoMapper;
using MongoDB.Bson;
using WhispMe.BLL.Interfaces;
using WhispMe.DAL.Entities;
using WhispMe.DAL.Interfaces;
using WhispMe.DTO.DTOs;
using WhispMe.DTO.Exceptions;

namespace WhispMe.BLL.Services;

public class RoomService : IRoomService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public RoomService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<RoomDto> CreateAsync(RoomDto entity)
    {
        try
        {
            var room = _mapper.Map<Room>(entity);
            room.Id = ObjectId.GenerateNewId().ToString();

            await _unitOfWork.RoomRepository.CreateAsync(room);
            return entity;
        }
        catch (Exception)
        {
            throw new Exception("Error creating message");
        }
    }

    public async Task<RoomDto> DeleteByIdAsync(string id)
    {
        try
        {
            var room = await _unitOfWork.RoomRepository.GetByIdAsync(id)
                ?? throw new NotFoundException("Room not found");
            await _unitOfWork.RoomRepository.DeleteAsync(id);
            return _mapper.Map<RoomDto>(room);
        }
        catch (NotFoundException)
        {
            throw;
        }
        catch (Exception)
        {
            throw new Exception("Error deleting room");
        }
    }

    public async Task<RoomDto> GetByIdAsync(string id)
    {
        try
        {
            var room = await _unitOfWork.RoomRepository.GetByIdAsync(id)
                ?? throw new NotFoundException("Room not found");
            return _mapper.Map<RoomDto>(room);
        }
        catch (NotFoundException)
        {
            throw;
        }
        catch (Exception)
        {
            throw new Exception("Error getting room");
        }
    }

    public async Task<RoomDto> GetRoomByNameAycnc(string roomName)
    {
        try
        {
            var room = await _unitOfWork.RoomRepository.GetByNameAsync(roomName)
                ?? throw new NotFoundException("Room not found");
            return _mapper.Map<RoomDto>(room);
        }
        catch (NotFoundException)
        {
            throw;
        }
        catch (Exception)
        {
            throw new Exception("Error getting room by name");
        }
    }

    public async Task<IEnumerable<RoomDto>> GetWithPaginationAsync(int pageNumber, int pageSize)
    {
        try
        {
            var rooms = await _unitOfWork.RoomRepository.GetWithPaginationAsync(pageNumber, pageSize);
            return _mapper.Map<IEnumerable<RoomDto>>(rooms);
        }
        catch (Exception)
        {
            throw new Exception("Error getting rooms with pagination");
        }
    }

    public async Task<RoomDto> UpdateAsync(string id, RoomDto entity)
    {
        try
        {
            var room = _mapper.Map<Room>(entity);
            await _unitOfWork.RoomRepository.UpdateAsync(id, room);
            return entity;
        }
        catch (Exception)
        {
            throw new Exception("Error updating room");
        }
    }
}
