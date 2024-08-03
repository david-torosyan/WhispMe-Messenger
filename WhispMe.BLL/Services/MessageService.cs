using AutoMapper;
using MongoDB.Bson;
using WhispMe.BLL.Interfaces;
using WhispMe.DAL.Entities;
using WhispMe.DAL.Interfaces;
using WhispMe.DTO.DTOs;
using WhispMe.DTO.Exceptions;

namespace WhispMe.BLL.Services;

public class MessageService : IMessageService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public MessageService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<MessageDto> CreateAsync(MessageDto entity)
    {
        try
        {
            entity.Timestamp = DateTime.UtcNow;
            var message = _mapper.Map<Message>(entity);
            message.Id = ObjectId.GenerateNewId().ToString();

            await _unitOfWork.MessageRepository.CreateAsync(message);
            return entity;
        }
        catch (Exception)
        {
            throw new Exception("Error creating message");
        }
    }

    public async Task<MessageDto> DeleteByIdAsync(string id)
    {
        try
        {
            var message = await _unitOfWork.MessageRepository.GetByIdAsync(id)
                ?? throw new NotFoundException("Message not found");
            await _unitOfWork.MessageRepository.DeleteAsync(id);
            return _mapper.Map<MessageDto>(message);
        }
        catch (NotFoundException)
        {
            throw;
        }
        catch (Exception)
        {
            throw new Exception("Error deleting message");
        }
    }

    public async Task<MessageDto> GetByIdAsync(string id)
    {
        try
        {
            var message = await _unitOfWork.MessageRepository.GetByIdAsync(id)
                ?? throw new NotFoundException("Message not found");
            return _mapper.Map<MessageDto>(message);
        }
        catch (NotFoundException)
        {
            throw;
        }
        catch (Exception)
        {
            throw new Exception("Error getting message by id");
        }
    }

    public async Task<IEnumerable<MessageDto>> GetMessagesByRoomNameAsync(string roomName)
    {
        try
        {
            var room = await _unitOfWork.RoomRepository.GetByNameAsync(roomName)
                ?? throw new NotFoundException("Room not found");

            var messages = await _unitOfWork.MessageRepository.GetByRoomAsync(room.Name)
                ?? throw new NotFoundException("Messages not found");

            return _mapper.Map<IEnumerable<MessageDto>>(messages);
        }
        catch (NotFoundException)
        {
            throw;
        }
        catch (Exception)
        {
            throw new Exception("Error getting messages by room name");
        }
    }

    public async Task<IEnumerable<MessageDto>> GetWithPaginationAsync(int pageNumber, int pageSize)
    {
        try
        {
            var messages = await _unitOfWork.MessageRepository.GetWithPaginationAsync(pageNumber, pageSize);
            return _mapper.Map<IEnumerable<MessageDto>>(messages);
        }
        catch (Exception)
        {
            throw new Exception("Error getting messages with pagination");
        }
    }

    public async Task<MessageDto> UpdateAsync(string id, MessageDto entity)
    {
        try
        {
            var exists = await _unitOfWork.MessageRepository.GetByIdAsync(id)
                    ?? throw new NotFoundException("Message not found");

            var message = _mapper.Map<Message>(entity);
            await _unitOfWork.MessageRepository.UpdateAsync(id, message);
            return _mapper.Map<MessageDto>(message);
        }
        catch (NotFoundException)
        {
            throw;
        }
        catch (Exception)
        {
            throw new Exception("Error updating message");
        }
    }
}
